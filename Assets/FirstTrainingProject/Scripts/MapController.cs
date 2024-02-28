using FirstTrainingProject;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Rendering;

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

    private int[,] _map = new int[15, 21];
    private int _height = 15;
    private int _width = 21;
    private readonly CellPoint _startPoint = new CellPoint() { PosH = 1, PosW = 1 };
    private readonly CellPointWithDirection _startPointDir = new CellPointWithDirection() { PosH = 1, PosW = 1, Direction = CellDirection.Right };
    private readonly CellPoint _endPoint = new CellPoint() { PosH = 13, PosW = 19 };
    private readonly CellPointWithDirection _endPointDir = new CellPointWithDirection() { PosH = 13, PosW = 19, Direction = CellDirection.Left };
    //private readonly CellPoint[] 

    #endregion

    #region Public Methods

    public void SetMapSize(int height, int width)
    {
        if (height > 6 && width > 6 && height % 2 == 1 && width % 2 == 1)
        {
            _height = height;
            _width = width;

            _startPoint.PosH = 1;
            _startPoint.PosW = 1;

            _endPoint.PosH = height - 1;
            _endPoint.PosW = width - 1;

            _map = new int[_height, _width];
            for (int h = 0; h < _map.GetLength(0); h++)
            {
                for (int w = 0; w < _map.GetLength(1); w++)
                {
                    _map[h, w] = (int)CellType.Empty;
                }
            }
            _map[_startPoint.PosH, _startPoint.PosW] = (int)CellType.Start;
            _map[_endPoint.PosH, _endPoint.PosW] = (int)CellType.End;
        }
    }

    public void SetStartPointPosition(int posH, int posW)
    {
        if (posH > 0 && posH < _height && posW > 0 && posW < _width && posH % 2 == 1 && posW % 2 == 1)
        {
            if (posH != _endPoint.PosH && posW != _endPoint.PosW)
            {
                _map[_startPoint.PosH, _startPoint.PosW] = (int)CellType.Empty;
                _startPoint.PosH = posH;
                _startPoint.PosW = posW;
                _map[_startPoint.PosH, _startPoint.PosW] = (int)CellType.Start;
            }
        }
    }

    public void SetEndPointPosition(int posH, int posW)
    {
        if (posH > 0 && posH < _height && posW > 0 && posW < _width && posH % 2 == 1 && posW % 2 == 1)
        {
            if (posH != _startPoint.PosH && posW != _startPoint.PosW)
            {
                _map[_endPoint.PosH, _endPoint.PosW] = (int)CellType.Empty;
                _endPoint.PosH = posH;
                _endPoint.PosW = posW;
                _map[_endPoint.PosH, _endPoint.PosW] = (int)CellType.End;
            }
        }
    }

    public void SetRooms(params RoomPoint[] rooms)
    {
        foreach (RoomPoint room in rooms)
        {
            if (room.PosH > 0 && room.PosH < _height && room.PosW > 0 && room.PosW < _width && room.PosH % 2 == 1 && room.PosW % 2 == 1)
            {
                if (room.SizeH % 2 == 0 && room.SizeW % 2 == 0)
                {
                    //if ()
                    //{

                    //}
                }
            }
        }
    }

    public void CreateMap()
    {
        List<CellPoint> points = new List<CellPoint>();

        _map[_startPoint.PosH, _startPoint.PosW] = (int)CellType.Marked;
        points.Add(_startPoint);
        var newPoint = new CellPoint();
        if (_startPointDir.Direction == CellDirection.Left)
        {
            newPoint.PosH = _startPoint.PosH;
            newPoint.PosW = _startPoint.PosW - 2;
        }
        if (_startPointDir.Direction == CellDirection.Top)
        {
            newPoint.PosH = _startPoint.PosH + 2;
            newPoint.PosW = _startPoint.PosW;
        }
        if (_startPointDir.Direction == CellDirection.Right)
        {
            newPoint.PosH = _startPoint.PosH;
            newPoint.PosW = _startPoint.PosW + 2;
        }
        if (_startPointDir.Direction == CellDirection.Bottom)
        {
            newPoint.PosH = _startPoint.PosH - 2;
            newPoint.PosW = _startPoint.PosW;
        }
        CreationPath(points[^1], newPoint);
        points.Add(newPoint);

        newPoint = new CellPoint();
        if (_endPointDir.Direction == CellDirection.Left)
        {
            newPoint.PosH = _endPoint.PosH;
            newPoint.PosW = _endPoint.PosW - 2;
        }
        if (_endPointDir.Direction == CellDirection.Top)
        {
            newPoint.PosH = _endPoint.PosH + 2;
            newPoint.PosW = _endPoint.PosW;
        }
        if (_endPointDir.Direction == CellDirection.Right)
        {
            newPoint.PosH = _endPoint.PosH;
            newPoint.PosW = _endPoint.PosW + 2;
        }
        if (_endPointDir.Direction == CellDirection.Bottom)
        {
            newPoint.PosH = _endPoint.PosH - 2;
            newPoint.PosW = _endPoint.PosW;
        }
        CreationPath(newPoint, _endPoint);

        while (points.Any())
        {
            var availableNeighbors = FindAvailableNeighbors(points[^1]);
            var selectedNeighbors = SelectAvailableNeighbors(availableNeighbors);
            if (selectedNeighbors != null)
            {
                CreationPath(points[^1], selectedNeighbors);
                points.Add(selectedNeighbors);
            }
            else
            {
                points.RemoveAt(points.Count - 1);
            }
        }

        //Info();

        CellTypeDefinition();

        DrawMapCells();
    }

    private List<CellPoint> FindAvailableNeighbors(CellPoint points)
    {
        var availableNeighbors = new List<CellPoint>();

        if (points.PosW > 2 && _map[points.PosH, points.PosW - 2] == (int)CellType.Empty)
        {
            availableNeighbors.Add(new CellPoint() { PosH = points.PosH, PosW = points.PosW - 2 });
        }
        if (points.PosH < _map.GetLength(0) - 2 && _map[points.PosH + 2, points.PosW] == (int)CellType.Empty)
        {
            availableNeighbors.Add(new CellPoint() { PosH = points.PosH + 2, PosW = points.PosW });
        }
        if (points.PosW < _map.GetLength(1) - 2 && _map[points.PosH, points.PosW + 2] == (int)CellType.Empty)
        {
            availableNeighbors.Add(new CellPoint() { PosH = points.PosH, PosW = points.PosW + 2 });
        }
        if (points.PosH > 2 && _map[points.PosH - 2, points.PosW] == (int)CellType.Empty)
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

    private void CreationPath(CellPoint latestNeighbor, CellPoint newNeighbor)
    {
        _map[(latestNeighbor.PosH + newNeighbor.PosH) / 2, (latestNeighbor.PosW + newNeighbor.PosW) / 2] = (int)CellType.Marked;
        _map[newNeighbor.PosH, newNeighbor.PosW] = (int)CellType.Marked;
    }

    private void CellTypeDefinition()
    {
        int numberOfNeighbors;
        bool[] directionOfNeighbors = new bool[4]; // Left - Up - Right - Down

        for (int h = 1; h <= _map.GetLength(0) - 2; h += 2)
        {
            for (int w = 1; w <= _map.GetLength(1) - 2; w += 2)
            {
                if (_map[h, w] == (int)CellType.Marked)
                {
                    numberOfNeighbors = 0;
                    for (int i = 0; i < directionOfNeighbors.Length; i++)
                    {
                        directionOfNeighbors[i] = false;
                    }

                    if (_map[h, w - 1] == (int)CellType.Marked)
                    {
                        directionOfNeighbors[0] = true;
                        numberOfNeighbors++;
                    }
                    if (_map[h + 1, w] == (int)CellType.Marked)
                    {
                        directionOfNeighbors[1] = true;
                        numberOfNeighbors++;
                    }
                    if (_map[h, w + 1] == (int)CellType.Marked)
                    {
                        directionOfNeighbors[2] = true;
                        numberOfNeighbors++;
                    }
                    if (_map[h - 1, w] == (int)CellType.Marked)
                    {
                        directionOfNeighbors[3] = true;
                        numberOfNeighbors++;
                    }

                    if (numberOfNeighbors == 1)
                    {
                        _map[h, w] = (int)CellType.Dead * 1000;
                        if (directionOfNeighbors[0])
                        {
                            _map[h, w] += (int)CellDirection.Left;
                        }
                        if (directionOfNeighbors[1])
                        {
                            _map[h, w] += (int)CellDirection.Top;
                        }
                        if (directionOfNeighbors[2])
                        {
                            _map[h, w] += (int)CellDirection.Right;
                        }
                        if (directionOfNeighbors[3])
                        {
                            _map[h, w] += (int)CellDirection.Bottom;
                        }
                    }

                    if (numberOfNeighbors == 2)
                    {
                        if (directionOfNeighbors[0] && directionOfNeighbors[2])
                        {
                            _map[h, w] = (int)CellType.Straight * 1000;
                            _map[h, w] += (int)CellDirection.Left;
                        }
                        if (directionOfNeighbors[1] && directionOfNeighbors[3])
                        {
                            _map[h, w] = (int)CellType.Straight * 1000;
                            _map[h, w] += (int)CellDirection.Top;
                        }

                        if (directionOfNeighbors[0] && directionOfNeighbors[1])
                        {
                            _map[h, w] = (int)CellType.Corner * 1000;
                            _map[h, w] += (int)CellDirection.Left;
                        }
                        if (directionOfNeighbors[1] && directionOfNeighbors[2])
                        {
                            _map[h, w] = (int)CellType.Corner * 1000;
                            _map[h, w] += (int)CellDirection.Top;
                        }
                        if (directionOfNeighbors[2] && directionOfNeighbors[3])
                        {
                            _map[h, w] = (int)CellType.Corner * 1000;
                            _map[h, w] += (int)CellDirection.Right;
                        }
                        if (directionOfNeighbors[3] && directionOfNeighbors[0])
                        {
                            _map[h, w] = (int)CellType.Corner * 1000;
                            _map[h, w] += (int)CellDirection.Bottom;
                        }
                    }

                    if (numberOfNeighbors == 3)
                    {
                        _map[h, w] = (int)CellType.Branch * 1000;
                        if (directionOfNeighbors[0] && directionOfNeighbors[1] && directionOfNeighbors[2])
                        {
                            _map[h, w] += (int)CellDirection.Left;
                        }
                        if (directionOfNeighbors[1] && directionOfNeighbors[2] && directionOfNeighbors[3])
                        {
                            _map[h, w] += (int)CellDirection.Top;
                        }
                        if (directionOfNeighbors[2] && directionOfNeighbors[3] && directionOfNeighbors[0])
                        {
                            _map[h, w] += (int)CellDirection.Right;
                        }
                        if (directionOfNeighbors[3] && directionOfNeighbors[0] && directionOfNeighbors[1])
                        {
                            _map[h, w] += (int)CellDirection.Bottom;
                        }
                    }
                    if (numberOfNeighbors == 4)
                    {
                        _map[h, w] = (int)CellType.Cross * 1000;
                    }
                }
            }
        }
    }

    private void DrawMapCells()
    {
        float height = 0F;
        float size = _enviromentPrefabsData.CellSize / 2;

        for (int h = 1; h <= _map.GetLength(0) - 2; h += 2)
        {
            for (int w = 1; w <= _map.GetLength(1) - 2; w += 2)
            {
                var coordinates = new Vector3(w * size, height, h * size);
                var cellType = (CellType)(_map[h, w] / 1000);
                var cellDirection = (DirectionForInstance)(_map[h, w] % 1000) - 90; //delete - 90 and rotate models

                //добавить метод который вернЄт нужный объект, будет смотреть на свободные клетки в нужном направлении
                //тогда получитс€ метод, в который € передаю вид €чейки, направление и координаты на карте, она возвращает объект
                //add a method that will return the desired object, will look at free cells in the desired direction
                //someday we will get a method in which I pass the cell type, directions and coordinates on the map, it returns an object
                var go = new GameObject();

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
                    cellDirection = (DirectionForInstance)(_map[h, w] % 1000);
                    go = _enviromentPrefabsData.BranchCell;
                }

                if (cellType == CellType.Cross)
                {
                    go = _enviromentPrefabsData.CrossCell;
                }

                CreateEnvironmentInstance(go, coordinates, cellDirection);
                //return for start and end position
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
        Debug.Log($"Map size: H {_height}, W {_width}");
        Debug.Log($"Start point: H {_startPoint.PosH}, W {_startPoint.PosW}");
        Debug.Log($"End point: H {_endPoint.PosH}, W {_endPoint.PosW}");

        string map = "";
        for (int h = 0; h < _map.GetLength(0); h++)
        {
            map = "";
            for (int w = 0; w < _map.GetLength(1); w++)
            {
                map += _map[h, w];
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
