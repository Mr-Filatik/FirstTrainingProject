using FirstTrainingProject;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static MapController;

public class MapController : MonoBehaviour
{
    #region Serialize Fields

    [Header("Main")]

    [SerializeField]
    private ApplicationManager _applicationManager;

    [Header("Enviroment parent")]

    [SerializeField]
    private GameObject _parentForEviroment;

    [Header("Enviroment prefabs")]

    [SerializeField]
    private EnviromentPrefabsData _enviromentPrefabsData;

    [Header("Other")]

    [SerializeField]
    private NavMeshSurface _navMeshSurface;

    #endregion

    #region Private Fields

    private MapType Map = new MapType(19, 27);
    private MapType MapAdditional = new MapType(19, 27);

    #endregion

    #region Public Methods

    public void SetMapSize(int height, int width)
    {
        if (height >= MapType.MinHeight && width > MapType.MinWidth && height % 2 == 1 && width % 2 == 1)
        {
            Map = new MapType(height, width);
        }
    }

    public void SetStartPointPosition(int posH, int posW)
    {
        if (Map.CellInMap(posH, posW) && posH != Map.EndPoint.PosH && posW != Map.EndPoint.PosW)
        {
            Map[Map.StartPoint.PosH, Map.StartPoint.PosW] = (int)CellType.Empty;
            Map.StartPoint.PosH = posH;
            Map.StartPoint.PosW = posW;
            Map[Map.StartPoint.PosH, Map.StartPoint.PosW] = (int)CellType.Start;
        }
    }

    public void SetEndPointPosition(int posH, int posW)
    {
        if (Map.CellInMap(posH, posW) && posH != Map.StartPoint.PosH && posW != Map.StartPoint.PosW)
        {
            Map[Map.EndPoint.PosH, Map.EndPoint.PosW] = (int)CellType.Empty;
            Map.EndPoint.PosH = posH;
            Map.EndPoint.PosW = posW;
            Map[Map.EndPoint.PosH, Map.EndPoint.PosW] = (int)CellType.End;
        }
    }

    public void SetInternalRooms(MapType map, params RoomPoint[] rooms)
    {
        foreach (RoomPoint room in rooms)
        {
            if (map.CellInMap(room.PosH, room.PosW) && map.CellInMap(room.PosH + room.SizeH, room.PosW + room.SizeW))
            {
                map.InternalRooms.Add(room);
                //change for SizeH and SizeW logic
                for (int h = room.PosH; h <= room.PosH + room.SizeH; h++)
                {
                    for (int w = room.PosW; w <= room.PosW + room.SizeW; w++)
                    {
                        map[h, w] = (int)CellType.Room;
                    }
                }
                if (room.DoorDirection == CellDirection.Left)
                {
                    map[room.PosH + room.DoorDistance * 2, room.PosW - 1] = (int)CellType.Marked;
                }
                if (room.DoorDirection == CellDirection.Top)
                {
                    map[room.PosH + room.SizeH + 1, room.PosW + room.DoorDistance * 2] = (int)CellType.Marked;
                }
                if (room.DoorDirection == CellDirection.Right)
                {
                    map[room.PosH + room.SizeH - room.DoorDistance * 2, room.PosW + room.SizeW + 1] = (int)CellType.Marked;
                }
                if (room.DoorDirection == CellDirection.Bottom)
                {
                    map[room.PosH - 1, room.PosW + room.SizeW - room.DoorDistance * 2] = (int)CellType.Marked;
                }
            }
        }
    }

    public void CreateMap()
    {
        Map = new MapType(19, 27);
        MapAdditional = new MapType(19, 27);

        var oneRoom = new RoomPoint()
        {
            PosH = 7,
            PosW = 7,
            SizeH = 3,
            SizeW = 2,
            DoorDirection = CellDirection.Left,
            DoorDistance = 1
        };
        SetInternalRooms(Map, oneRoom);
        SetInternalRooms(MapAdditional, oneRoom);

        var twoRoom = new RoomPoint()
        {
            PosH = 11,
            PosW = 15,
            SizeH = 1,
            SizeW = 1,
            DoorDirection = CellDirection.Top,
            DoorDistance = 0
        };
        SetInternalRooms(Map, twoRoom);
        SetInternalRooms(MapAdditional, twoRoom);

        var threeRoom = new RoomPoint()
        {
            PosH = 3,
            PosW = 19,
            SizeH = 2,
            SizeW = 2,
            DoorDirection = CellDirection.Bottom,
            DoorDistance = 0
        };
        SetInternalRooms(Map, threeRoom);
        SetInternalRooms(MapAdditional, threeRoom);

        AddingMaze(Map);

        AddingMaze(MapAdditional);

        MergedMaps(Map, MapAdditional, 0.33F);

        //Info();

        CellTypeDefinition();

        ClearEnvironments();

        DrawMapCells();

        DrawMapInternalRooms();

        _navMeshSurface.BuildNavMesh();



        void AddingMaze(MapType map)
        {
            List<CellPoint> points = new List<CellPoint>();

            map[map.StartPoint.PosH, map.StartPoint.PosW] = (int)CellType.Marked;

            points.Add(map.StartPoint);
            var newPoint = GetNextCell(map.StartPoint);
            CreationPath(points[^1], newPoint, map);
            points.Add(newPoint);

            newPoint = GetNextCell(map.EndPoint);
            CreationPath(newPoint, map.EndPoint, map);

            while (points.Any())
            {
                var availableNeighbors = FindAvailableNeighbors(points[^1], map);
                var selectedNeighbors = SelectAvailableNeighbors(availableNeighbors);
                if (selectedNeighbors != null)
                {
                    CreationPath(points[^1], selectedNeighbors, map);
                    points.Add(selectedNeighbors);
                }
                else
                {
                    points.RemoveAt(points.Count - 1);
                }
            }
        }

        void MergedMaps(MapType map, MapType addMap, float percent)
        {
            for (int h = 0; h < map.Height; h++)
            {
                for (int w = h % 2 == 0 ? 1 : 0; w < map.Width - 1; w += 2)
                {
                    if (addMap[h, w] == (int)CellType.Marked && map[h, w] == (int)CellType.Empty && Random.Range(0, (int)(1 / percent)) == 0)
                    {
                        map[h, w] = addMap[h, w];
                    }
                }
            }
        }

        CellPoint GetNextCell(CellPointWithDirection cell)
        {
            var newPoint = new CellPoint();
            if (cell.Direction == CellDirection.Left)
            {
                newPoint.PosH = cell.PosH;
                newPoint.PosW = cell.PosW - 2;
            }
            if (cell.Direction == CellDirection.Top)
            {
                newPoint.PosH = cell.PosH + 2;
                newPoint.PosW = cell.PosW;
            }
            if (cell.Direction == CellDirection.Right)
            {
                newPoint.PosH = cell.PosH;
                newPoint.PosW = cell.PosW + 2;
            }
            if (cell.Direction == CellDirection.Bottom)
            {
                newPoint.PosH = cell.PosH - 2;
                newPoint.PosW = cell.PosW;
            }
            return newPoint;
        }
    }

    private List<CellPoint> FindAvailableNeighbors(CellPoint points, MapType map)
    {
        var availableNeighbors = new List<CellPoint>();

        if (points.PosW > 2 && map[points.PosH, points.PosW - 2] == (int)CellType.Empty)
        {
            availableNeighbors.Add(new CellPoint() { PosH = points.PosH, PosW = points.PosW - 2 });
        }
        if (points.PosH < map.Height - 2 && map[points.PosH + 2, points.PosW] == (int)CellType.Empty)
        {
            availableNeighbors.Add(new CellPoint() { PosH = points.PosH + 2, PosW = points.PosW });
        }
        if (points.PosW < map.Width - 2 && map[points.PosH, points.PosW + 2] == (int)CellType.Empty)
        {
            availableNeighbors.Add(new CellPoint() { PosH = points.PosH, PosW = points.PosW + 2 });
        }
        if (points.PosH > 2 && map[points.PosH - 2, points.PosW] == (int)CellType.Empty)
        {
            availableNeighbors.Add(new CellPoint() { PosH = points.PosH - 2, PosW = points.PosW });
        }

        return availableNeighbors;
    }

    private CellPoint SelectAvailableNeighbors(List<CellPoint> availableNeighbors)
    {
        if (availableNeighbors.Any())
        {
            return availableNeighbors[Random.Range(0, availableNeighbors.Count)];
        }
        else
        {
            return null;
        }
    }

    private void CreationPath(CellPoint latestNeighbor, CellPoint newNeighbor, MapType map)
    {
        map[(latestNeighbor.PosH + newNeighbor.PosH) / 2, (latestNeighbor.PosW + newNeighbor.PosW) / 2] = (int)CellType.Marked;
        map[newNeighbor.PosH, newNeighbor.PosW] = (int)CellType.Marked;
    }

    private void CellTypeDefinition()
    {
        int numberOfNeighbors;
        bool[] directionOfNeighbors = new bool[4]; // Left - Up - Right - Down

        for (int h = 1; h <= Map.Height - 2; h += 2)
        {
            for (int w = 1; w <= Map.Width - 2; w += 2)
            {
                if (h == Map.StartPoint.PosH && w == Map.StartPoint.PosW)
                {
                    Map[h, w] = (int)CellType.Start * 1000 + (int)Map.StartPoint.Direction;
                }
                if (h == Map.EndPoint.PosH && w == Map.EndPoint.PosW)
                {
                    Map[h, w] = (int)CellType.End * 1000 + (int)Map.EndPoint.Direction;
                }
                if (Map[h, w] == (int)CellType.Marked)
                {
                    numberOfNeighbors = 0;
                    for (int i = 0; i < directionOfNeighbors.Length; i++)
                    {
                        directionOfNeighbors[i] = false;
                    }

                    if (Map[h, w - 1] == (int)CellType.Marked)
                    {
                        directionOfNeighbors[0] = true;
                        numberOfNeighbors++;
                    }
                    if (Map[h + 1, w] == (int)CellType.Marked)
                    {
                        directionOfNeighbors[1] = true;
                        numberOfNeighbors++;
                    }
                    if (Map[h, w + 1] == (int)CellType.Marked)
                    {
                        directionOfNeighbors[2] = true;
                        numberOfNeighbors++;
                    }
                    if (Map[h - 1, w] == (int)CellType.Marked)
                    {
                        directionOfNeighbors[3] = true;
                        numberOfNeighbors++;
                    }

                    if (numberOfNeighbors == 1)
                    {
                        Map[h, w] = (int)CellType.Dead * 1000;
                        if (directionOfNeighbors[0])
                        {
                            Map[h, w] += (int)CellDirection.Left;
                        }
                        if (directionOfNeighbors[1])
                        {
                            Map[h, w] += (int)CellDirection.Top;
                        }
                        if (directionOfNeighbors[2])
                        {
                            Map[h, w] += (int)CellDirection.Right;
                        }
                        if (directionOfNeighbors[3])
                        {
                            Map[h, w] += (int)CellDirection.Bottom;
                        }
                    }

                    if (numberOfNeighbors == 2)
                    {
                        if (directionOfNeighbors[0] && directionOfNeighbors[2])
                        {
                            Map[h, w] = (int)CellType.Straight * 1000;
                            Map[h, w] += (int)CellDirection.Left;
                        }
                        if (directionOfNeighbors[1] && directionOfNeighbors[3])
                        {
                            Map[h, w] = (int)CellType.Straight * 1000;
                            Map[h, w] += (int)CellDirection.Top;
                        }

                        if (directionOfNeighbors[0] && directionOfNeighbors[1])
                        {
                            Map[h, w] = (int)CellType.Corner * 1000;
                            Map[h, w] += (int)CellDirection.Left;
                        }
                        if (directionOfNeighbors[1] && directionOfNeighbors[2])
                        {
                            Map[h, w] = (int)CellType.Corner * 1000;
                            Map[h, w] += (int)CellDirection.Top;
                        }
                        if (directionOfNeighbors[2] && directionOfNeighbors[3])
                        {
                            Map[h, w] = (int)CellType.Corner * 1000;
                            Map[h, w] += (int)CellDirection.Right;
                        }
                        if (directionOfNeighbors[3] && directionOfNeighbors[0])
                        {
                            Map[h, w] = (int)CellType.Corner * 1000;
                            Map[h, w] += (int)CellDirection.Bottom;
                        }
                    }

                    if (numberOfNeighbors == 3)
                    {
                        Map[h, w] = (int)CellType.Branch * 1000;
                        if (directionOfNeighbors[0] && directionOfNeighbors[1] && directionOfNeighbors[2])
                        {
                            Map[h, w] += (int)CellDirection.Left;
                        }
                        if (directionOfNeighbors[1] && directionOfNeighbors[2] && directionOfNeighbors[3])
                        {
                            Map[h, w] += (int)CellDirection.Top;
                        }
                        if (directionOfNeighbors[2] && directionOfNeighbors[3] && directionOfNeighbors[0])
                        {
                            Map[h, w] += (int)CellDirection.Right;
                        }
                        if (directionOfNeighbors[3] && directionOfNeighbors[0] && directionOfNeighbors[1])
                        {
                            Map[h, w] += (int)CellDirection.Bottom;
                        }
                    }

                    if (numberOfNeighbors == 4)
                    {
                        Map[h, w] = (int)CellType.Cross * 1000;
                    }
                }
            }
        }
    }

    private void ClearEnvironments()
    {
        Vector3 position = Vector3.zero;
        for (int i = _parentForEviroment.transform.childCount - 1; i >= 0; i--)
        {
            // fix nav mesh bake method
            position = _parentForEviroment.transform.GetChild(i).gameObject.transform.position;
            position.y -= 10;
            _parentForEviroment.transform.GetChild(i).gameObject.transform.position = position;

            Destroy(_parentForEviroment.transform.GetChild(i).gameObject);
        }
    }

    private void DrawMapCells()
    {
        float height = 0F;
        float size = _enviromentPrefabsData.CellSize / 2;

        for (int h = 1; h <= Map.Height - 2; h += 2)
        {
            for (int w = 1; w <= Map.Width - 2; w += 2)
            {
                var coordinates = new Vector3(w * size, height, h * size);
                var cellType = (CellType)(Map[h, w] / 1000);
                var cellDirection = (DirectionForInstance)(Map[h, w] % 1000) - 90; //delete - 90 and rotate models

                //добавить метод который вернЄт нужный объект, будет смотреть на свободные клетки в нужном направлении
                //тогда получитс€ метод, в который € передаю вид €чейки, направление и координаты на карте, она возвращает объект
                //add a method that will return the desired object, will look at free cells in the desired direction
                //someday we will get a method in which I pass the cell type, directions and coordinates on the map, it returns an object
                GameObject go = null;

                if (cellType == CellType.Start)
                {
                    go = _enviromentPrefabsData.StartCell;
                }

                if (cellType == CellType.End)
                {
                    go = _enviromentPrefabsData.EndCell;
                }

                if (cellType == CellType.Dead)
                {
                    go = _enviromentPrefabsData.DeadEndCell;
                }

                if (cellType == CellType.Corner)
                {
                    go = _enviromentPrefabsData.CornerCell;
                }

                if (cellType == CellType.Straight)
                {
                    go = _enviromentPrefabsData.StraightCell;
                }

                if (cellType == CellType.Branch)
                {
                    //until these points are deployed correctly
                    cellDirection = (DirectionForInstance)(Map[h, w] % 1000);
                    go = _enviromentPrefabsData.BranchCell;
                }

                if (cellType == CellType.Cross)
                {
                    go = _enviromentPrefabsData.CrossCell;
                }

                if (go != null)
                {
                    var obj = CreateEnvironmentInstance(go, coordinates, cellDirection);
                    //return for start and end position

                    if (cellType == CellType.Start)
                    {
                        coordinates = obj.transform.position;
                        coordinates += new Vector3(0F, 0.001F, 0F);
                        var angle = new Vector3(0F, (float)cellDirection, 0F); //need change
                        _applicationManager.GameController?.SetNewSpawnPlaceInLevel(coordinates, angle);
                    }

                    if (cellType == CellType.End)
                    {
                        coordinates = obj.transform.position;
                        coordinates += new Vector3(0F, 0.001F, 0F);
                        var angle = new Vector3(0F, (float)cellDirection, 0F); //need change
                        _applicationManager.EnemyController?.SetPosition(coordinates, angle);
                        _applicationManager.EnemyController?.SetTarget(_applicationManager.PlayerController.transform);
                    }
                }
            }
        }
    }

    private void DrawMapInternalRooms()
    {
        float height = 0F;
        float size = _enviromentPrefabsData.CellSize / 2;

        foreach (var room in Map.InternalRooms)
        {
            var h = room.PosH + room.SizeH / 2;
            var w = room.PosW + room.SizeW / 2;

            var coordinates = new Vector3(w * size, height, h * size);
            var cellDirection = (DirectionForInstance)((int)room.DoorDirection) - 90;

            GameObject go = null;
            if (room.SizeH == 0 && room.SizeW == 0)
            {
                go = _enviromentPrefabsData.Room1x1;
            }
            if (room.SizeH == 2 && room.SizeW == 2)
            {
                go = _enviromentPrefabsData.Room2x2Left;
            }
            if ((room.SizeH == 4 && room.SizeW == 2) || (room.SizeH == 2 && room.SizeW == 4)) //change get room Size logic
            {
                go = _enviromentPrefabsData.Room3x2Center;
            }

            if (go != null)
            {
                CreateEnvironmentInstance(go, coordinates, cellDirection);
            }
        }
    }

    private GameObject CreateEnvironmentInstance(GameObject prefab, Vector3 coordinates, DirectionForInstance direction = DirectionForInstance.Left)
    {
        var objectBeingCreated = Instantiate(prefab, _parentForEviroment.transform);
        objectBeingCreated.transform.localPosition = coordinates;
        objectBeingCreated.transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (float)direction, transform.eulerAngles.z);
        return objectBeingCreated;
    }

    public void Info()
    {
        Debug.Log($"Map size: H {Map.Height}, W {Map.Width}");
        Debug.Log($"Start point: H {Map.StartPoint.PosH}, W {Map.StartPoint.PosW}");
        Debug.Log($"End point: H {Map.EndPoint.PosH}, W {Map.EndPoint.PosW}");

        string map = "";
        for (int h = 0; h < this.Map.Height; h++)
        {
            map = "";
            for (int w = 0; w < this.Map.Width; w++)
            {
                map += this.Map[h, w];
                map += " ";
            }
            Debug.Log(map);
        }
    }

    #endregion

    #region Unity Medhods

    private void Awake()
    {
        if (_parentForEviroment == null) throw new System.Exception($"ParentForEnviroment not set!");
        if (_enviromentPrefabsData == null) throw new System.Exception($"EnviromentPrefabsData not set!");
        //if (_navMeshSurface == null) throw new System.Exception($"NavMeshSurface not set!");
        if (_applicationManager == null) throw new System.Exception($"ApplicationManager not set!");

        //_applicationManager.MapController = this;

        _applicationManager.ApplicationGameInited += CreateMap;
    }

    private void OnDestroy()
    {
        _applicationManager.ApplicationGameInited -= CreateMap;
    }

    #endregion

    #region Other

    public enum CellType
    {
        Empty = 0,
        Marked = 1,
        Room = 2, // m.b. removed
        Door = 3,

        Start = 11,
        Action = 12,
        Dead = 13,
        End = 14,

        Corner = 21,
        Straight = 22,

        Branch = 31,

        Cross = 41
    }

    public enum CellDirection
    {
        Left = 0,
        Top = 90,
        Right = 180,
        Bottom = 270
    }

    #endregion
}
