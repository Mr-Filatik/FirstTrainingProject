//using System.Collections;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Threading.Tasks;
//using Unity.AI.Navigation;
//using UnityEngine;

//namespace FirstTrainingProject
//{
//    public class OldMapController : MonoBehaviour
//    {
//        #region Public Fields

//        [SerializeField]
//        private ApplicationManager _applicationManager;

//        [Header("Enviroment parent")]

//        [SerializeField]
//        private GameObject _parentForEviroment;

//        [Header("Enviroment prefabs")]

//        [SerializeField]
//        private EnviromentPrefabsData _enviromentPrefabsData;
//        ////public GameObject WallBlank;
//        ////public GameObject FloorBlank;
//        ////public GameObject DoorLattice;
//        ////public GameObject Room;

//        #endregion

//        [Header("Other")]

//        [SerializeField]
//        private NavMeshSurface _navMeshSurface;

//        //Variables
//        static int height;
//        static int width;

//        #region Private Fields

//        private int[,] map_main;
//        private int[,] map_additional;

//        private int points_list_length = 0;
//        private int[,] points_list;
//        private int[,] neighbors_list;
//        //private int neighbors_count = 0;
//        private int random_neighbor;

//        private Vector3 coordinates;
//        private GameObject object_being_created;

//        #endregion

//        private void Awake()
//        {
//            if (_parentForEviroment == null) Debug.LogError($"ParentForEnviroment not set!");
//            if (_navMeshSurface == null) Debug.LogError($"NavMeshSurface not set!");
//            if (_applicationManager == null) Debug.LogError($"ApplicationManager not set!");

//            //_applicationManager.MapController = this;

//            _applicationManager.ApplicationGameInited += MapCreate;
//            //_applicationManager.ApplicationGameEnded += MapCreate;
//        }

//        private void OnDestroy()
//        {
//            _applicationManager.ApplicationGameInited -= MapCreate;
//            //_applicationManager.ApplicationGameEnded -= MapCreate;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="height"> Height (only !/2 (3, 5, 7, ...)) </param>
//        /// <param name="width"> Width (only !/2 (3, 5, 7, ...)) </param>
//        /// <returns></returns>
//        public void MapCreate()
//        {
//            CreatingMap(15, 21);
//            ClearInvironments();
//            DrawMap(map_main);

//            _navMeshSurface.BuildNavMesh();

//            //_applicationManager.PlayerController.SetPosition();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="height"> Height (only !/2 (3, 5, 7, ...)) </param>
//        /// <param name="width"> Width (only !/2 (3, 5, 7, ...)) </param>
//        private void CreatingMap(int height, int width)
//        {
//            map_main = new int[height, width];
//            map_additional = new int[height, width]; //дополнительная карта
//            points_list = new int[((height - 1) / 2) * ((width - 1) / 2), 2]; //массив в котором мы сохраняем все координаты точек, которые прошли?
            

//            // 2 - пока не знаю что именно
//            // 1 - задаётся для точек, которые являются ячейками
//            // 0 - задаётся для всех других ячеек
//            for (int i = 0; i < height; i++)
//            {
//                for (int j = 0; j < width; j++)
//                {
//                    if (i % 2 == 1 && j % 2 == 1)
//                    {
//                        map_main[i, j] = (int)PointForCreateMaze.Free;
//                        map_additional[i, j] = (int)PointForCreateMaze.Free;
//                    }
//                    else
//                    {
//                        map_main[i, j] = 0;
//                        map_additional[i, j] = 0;
//                    }
//                }
//            }

//            //room in map create
//            for (int i = 1; i <= 6; i++)
//            {
//                for (int j = 1; j <= 4; j++)
//                {
//                    if (i == 6 || j == 4)
//                    {
//                        map_main[i, j] = 0;
//                        map_additional[i, j] = 0;
//                    }
//                    else
//                    {
//                        map_main[i, j] = 2;
//                        map_additional[i, j] = 2;
//                    }
//                }
//            }
//            map_main[3, 4] = 2;
//            map_additional[3, 4] = 2;

//            //room in map create ?
//            map_main[height - 2, 1] = 2;
//            map_main[height - 2, 2] = 2;
//            map_main[height - 2, 3] = 2;

//            map_additional[height - 2, 1] = 2;
//            map_additional[height - 2, 2] = 2;
//            map_additional[height - 2, 3] = 2;

//            map_main[1, width - 2] = 2;
//            map_main[1, width - 3] = 2;
//            map_main[1, width - 4] = (int)PointForCreateMaze.Free;

//            map_additional[1, width - 2] = 2;
//            map_additional[1, width - 3] = 2;
//            map_additional[1, width - 4] = (int)PointForCreateMaze.Free;

//            points_list[0, 0] = height - 2; // координата стартовой точки по высоте
//            points_list[0, 1] = 3; // координата старотовой точки по ширинне
//            map_main[points_list[0, 0], points_list[0, 1]] = 2;
//            map_additional[points_list[0, 0], points_list[0, 1]] = 2;

//            //

//            neighbors_list = new int[4, 2];

//            // Finding the base path
//            // Поиск базового пути
//            points_list_length = 0;
//            while (points_list_length >= 0)
//            {
//                var availableNeighbors = FindAvailableNeighbors(map_main, points_list_length, points_list);
//                CreationPath(availableNeighbors, ref points_list_length, ref points_list, map_main);
//            }

//            // Finding an extra path (If you need only one way - remove)
//            // Поиск дополнительного пути (Если нужен только один путь - убрать)
//            points_list_length = 0;
//            while (points_list_length >= 0)
//            {
//                var availableNeighbors = FindAvailableNeighbors(map_additional, points_list_length, points_list);
//                CreationPath(availableNeighbors, ref points_list_length, ref points_list, map_additional);
//            }

//            // With a 33 percent chance we combine the main and additional routes
//            // С шансом 33 процентов объединяем основной и дополнительные маршруты
//            for (int i = 0; i < height; i++)
//            {
//                for (int j = 0; j < width; j++)
//                {
//                    if (map_additional[i, j] == 2 && map_main[i, j] != 2 && Random.Range(0, 3) == 0)
//                    {
//                        map_main[i, j] = map_additional[i, j];
//                    }
//                }
//            }

//            CellTypeDefinition(map_main);

//            VoidsForPremisesDefinition(map_main);

//            map_main[height - 2, 1] = 5; //set start point
//            map_main[1, width - 2] = 6; //set end point

//            map_main[height - 2, 2] = 11; //path with start point for algoritm
//            map_main[3, 4] = 11; //second path with start point for algoritm (mb room)
//            map_main[1, width - 3] = 11; //ent path for algoritm

//            //map_main[1, 1] = 7; //room point
//            //for (int i = 1; i <= 5; i++)
//            //{
//            //    for (int j = 1; j <= 3; j++)
//            //    {
//            //        map_main[i, j] = 0;
//            //    }
//            //}
//            //map_main[1, 1] = 7;
//        }

//        /// <summary>
//        /// Search for all free neighbor cells (cells that are not needed for other purposes)
//        /// </summary>
//        /// <param name="map"> Map (array of integers) </param>
//        /// <param name="point_list_size"></param>
//        /// <param name="point_list"></param>
//        /// <returns> List free neighbor cells (indexes) </returns>
//        /// <remarks> Поиск всех свободных соседних клеток (клеток, которые не нужны для других целей) </remarks>
//        private List<(int hIndex, int wIndex)> FindAvailableNeighbors(int[,] map, int point_list_size, int[,] point_list)
//        {
//            var availableNeighborsList = new List<(int, int)>();

//            if (point_list[point_list_size, 0] > 2 && map[point_list[point_list_size, 0] - 2, point_list[point_list_size, 1]] == (int)PointForCreateMaze.Free)
//            {
//                availableNeighborsList.Add((point_list[point_list_size, 0] - 2, point_list[point_list_size, 1]));
//            }
//            if (point_list[point_list_size, 1] < map.GetUpperBound(1) - 2 && map[point_list[point_list_size, 0], point_list[point_list_size, 1] + 2] == (int)PointForCreateMaze.Free)
//            {
//                availableNeighborsList.Add((point_list[point_list_size, 0], point_list[point_list_size, 1] + 2));
//            }
//            if (point_list[point_list_size, 0] < map.GetUpperBound(0) - 2 && map[point_list[point_list_size, 0] + 2, point_list[point_list_size, 1]] == (int)PointForCreateMaze.Free)
//            {
//                availableNeighborsList.Add((point_list[point_list_size, 0] + 2, point_list[point_list_size, 1]));
//            }
//            if (point_list[point_list_size, 1] > 2 && map[point_list[point_list_size, 0], point_list[point_list_size, 1] - 2] == (int)PointForCreateMaze.Free)
//            {
//                availableNeighborsList.Add((point_list[point_list_size, 0], point_list[point_list_size, 1] - 2));
//            }

//            return availableNeighborsList;
//        }

//        // метод создания пути из текущей точки в следующую
//        private void CreationPath(List<(int hIndex, int wIndex)> list_of_neighbors, ref int point_list_size, ref int[,] point_list, int[,] map)
//        {
//            if (list_of_neighbors.Count > 0)
//            {
//                // выбор рандомного соседа
//                random_neighbor = Random.Range(0, list_of_neighbors.Count);

//                //добавляем его в массив
//                point_list_size++;
//                point_list[point_list_size, 0] = list_of_neighbors[random_neighbor].hIndex;
//                point_list[point_list_size, 1] = list_of_neighbors[random_neighbor].wIndex;

//                //указываем что точка смежная для выбранной и текущей пройдена (стена) (становится недоступной для прохода)
//                map[Mathf.Abs(point_list[point_list_size, 0] + point_list[point_list_size - 1, 0]) / 2, Mathf.Abs(point_list[point_list_size, 1] + point_list[point_list_size - 1, 1]) / 2] = (int)PointForCreateMaze.Bysy;
//                //указываем что текущая точка уже пройдена (становится недоступной для прохода)
//                map[point_list[point_list_size, 0], point_list[point_list_size, 1]] = (int)PointForCreateMaze.Bysy;
//            }
//            else
//            {
//                point_list_size--;
//            }
//        }

//        /// <summary>
//        /// Determining the cell type
//        /// </summary>
//        /// <param name="map"> Map (array of integers) </param>
//        /// <remarks> Определение типа ячейки </remarks>
//        private void CellTypeDefinition(int[,] map)
//        {
//            int numberOfNeighbors;
//            bool[] directionOfNeighbors = new bool[4]; // Left - Up - Right - Down

//            for (int h = 1; h < map.GetUpperBound(0); h += 2)
//            {
//                for (int w = 1; w < map.GetUpperBound(1); w += 2)
//                {
//                    if (map[h, w] >= 2)
//                    {
//                        // Determining the number and location of neighbors
//                        // Определение количества и расположения соседей
//                        numberOfNeighbors = 0;
//                        for (int i = 0; i < directionOfNeighbors.Length; i++)
//                        {
//                            directionOfNeighbors[i] = false;
//                        }

//                        if (map[h - 1, w] >= 2)
//                        {
//                            directionOfNeighbors[0] = true;
//                            numberOfNeighbors++;
//                        }
//                        if (map[h, w + 1] >= 2)
//                        {
//                            directionOfNeighbors[1] = true;
//                            numberOfNeighbors++;
//                        }
//                        if (map[h + 1, w] >= 2)
//                        {
//                            directionOfNeighbors[2] = true;
//                            numberOfNeighbors++;
//                        }
//                        if (map[h, w - 1] >= 2)
//                        {
//                            directionOfNeighbors[3] = true;
//                            numberOfNeighbors++;
//                        }

//                        // Determining the direction for a cell with one open wall
//                        // Определение направления для ячейки с одной открытой стенкой
//                        if (numberOfNeighbors == 1)
//                        {
//                            if (directionOfNeighbors[0])
//                            {
//                                map[h, w] = (int)BlindAlleyDirection.Left;
//                            }
//                            if (directionOfNeighbors[1])
//                            {
//                                map[h, w] = (int)BlindAlleyDirection.Up;
//                            }
//                            if (directionOfNeighbors[2])
//                            {
//                                map[h, w] = (int)BlindAlleyDirection.Right;
//                            }
//                            if (directionOfNeighbors[3])
//                            {
//                                map[h, w] = (int)BlindAlleyDirection.Down;
//                            }
//                        }

//                        // Determining the direction for a cell with two open walls
//                        // Определение направления для ячейки с двумя открытыми стенками
//                        if (numberOfNeighbors == 2)
//                        {
//                            if (directionOfNeighbors[0] && directionOfNeighbors[2])
//                            {
//                                map[h, w] = (int)StraightDirection.LeftToRight;
//                            }
//                            if (directionOfNeighbors[1] && directionOfNeighbors[3])
//                            {
//                                map[h, w] = (int)StraightDirection.UpToDown;
//                            }
//                            if (directionOfNeighbors[0] && directionOfNeighbors[1])
//                            {
//                                map[h, w] = (int)AngleDirection.LeftToUp;
//                            }
//                            if (directionOfNeighbors[1] && directionOfNeighbors[2])
//                            {
//                                map[h, w] = (int)AngleDirection.UpToRight;
//                            }
//                            if (directionOfNeighbors[2] && directionOfNeighbors[3])
//                            {
//                                map[h, w] = (int)AngleDirection.RightToDown;
//                            }
//                            if (directionOfNeighbors[3] && directionOfNeighbors[0])
//                            {
//                                map[h, w] = (int)AngleDirection.DownToLeft;
//                            }
//                        }

//                        // Determining the direction for a cell with three open walls
//                        // Определение направления для клетки с тремя открытыми стенками
//                        if (numberOfNeighbors == 3)
//                        {
//                            if (directionOfNeighbors[3] && directionOfNeighbors[0] && directionOfNeighbors[1])
//                            {
//                                map[h, w] = (int)BranchDirection.LeftBetweenUpAndDown;
//                            }
//                            if (directionOfNeighbors[0] && directionOfNeighbors[1] && directionOfNeighbors[2])
//                            {
//                                map[h, w] = (int)BranchDirection.UpBetweenLeftAndRight;
//                            }
//                            if (directionOfNeighbors[1] && directionOfNeighbors[2] && directionOfNeighbors[3])
//                            {
//                                map[h, w] = (int)BranchDirection.RightBetweenDownAndUp;
//                            }
//                            if (directionOfNeighbors[2] && directionOfNeighbors[3] && directionOfNeighbors[0])
//                            {
//                                map[h, w] = (int)BranchDirection.DownBetweenRightAndLeft;
//                            }
//                        }

//                        // Determining the direction of a cross cell
//                        // Определение направления поперечной ячейки
//                        if (numberOfNeighbors == 4)
//                        {
//                            map[h, w] = (int)CrossingDirection.AllSides;
//                        }
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// Determination of voids for premises (removal of pillars)
//        /// </summary>
//        /// <param name="map"> Map (array of integers) </param>
//        /// <remarks> Определение пустот под помещения (удаление столбов) </remarks>
//        private void VoidsForPremisesDefinition(int[,] map)
//        {
//            int[,] mapAdditional = new int[map.GetLength(0), map.GetLength(1)];

//            for (int h = 1; h < map.GetUpperBound(0); h += 2)
//            {
//                for (int w = 1; w < map.GetUpperBound(1); w += 2)
//                {
//                    // For angle
//                    if (map[h, w] == (int)AngleDirection.LeftToUp && ChechFreePlace(map, h, w, DirectionForRemovePillars.LeftUp))
//                    {
//                        mapAdditional[h, w] = (int)AngleDirection.LeftToUpWithoutPillar;
//                    }
//                    if (map[h, w] == (int)AngleDirection.UpToRight && ChechFreePlace(map, h, w, DirectionForRemovePillars.UpRight))
//                    {
//                        mapAdditional[h, w] = (int)AngleDirection.UpToRightWithoutPillar;
//                    }
//                    if (map[h, w] == (int)AngleDirection.RightToDown && ChechFreePlace(map, h, w, DirectionForRemovePillars.RightDown))
//                    {
//                        mapAdditional[h, w] = (int)AngleDirection.RightToDownWithoutPillar;
//                    }
//                    if (map[h, w] == (int)AngleDirection.DownToLeft && ChechFreePlace(map, h, w, DirectionForRemovePillars.DownLeft))
//                    {
//                        mapAdditional[h, w] = (int)AngleDirection.DownToLeftWithoutPillar;
//                    }

//                    // For branch
//                    bool isLeftPillar = true;
//                    bool isRightPillar = true;
//                    if (map[h, w] == (int)BranchDirection.LeftBetweenUpAndDown)
//                    {
//                        if (ChechFreePlace(map, h, w, DirectionForRemovePillars.LeftUp))
//                        {
//                            isRightPillar = false;
//                        }
//                        if (ChechFreePlace(map, h, w, DirectionForRemovePillars.DownLeft))
//                        {
//                            isLeftPillar = false;
//                        }
//                        if (!isLeftPillar && !isRightPillar)
//                        {
//                            mapAdditional[h, w] = (int)BranchDirection.LeftBetweenUpAndDownWithoutBothPillars;
//                        }
//                        else
//                        {
//                            if (!isLeftPillar)
//                            {
//                                mapAdditional[h, w] = (int)BranchDirection.LeftBetweenUpAndDownWithoutDownPillar;
//                            }
//                            if (!isRightPillar)
//                            {
//                                mapAdditional[h, w] = (int)BranchDirection.LeftBetweenUpAndDownWithoutUpPillar;
//                            }
//                        }
//                    }
//                    if (map[h, w] == (int)BranchDirection.UpBetweenLeftAndRight)
//                    {
//                        if (ChechFreePlace(map, h, w, DirectionForRemovePillars.UpRight))
//                        {
//                            isRightPillar = false;
//                        }
//                        if (ChechFreePlace(map, h, w, DirectionForRemovePillars.LeftUp))
//                        {
//                            isLeftPillar = false;
//                        }
//                        if (!isLeftPillar && !isRightPillar)
//                        {
//                            mapAdditional[h, w] = (int)BranchDirection.UpBetweenLeftAndRightWithoutBothPillars;
//                        }
//                        else
//                        {
//                            if (!isLeftPillar)
//                            {
//                                mapAdditional[h, w] = (int)BranchDirection.UpBetweenLeftAndRightWithoutLeftPillar;
//                            }
//                            if (!isRightPillar)
//                            {
//                                mapAdditional[h, w] = (int)BranchDirection.UpBetweenLeftAndRightWithoutRightPillar;
//                            }
//                        }
//                    }
//                    if (map[h, w] == (int)BranchDirection.RightBetweenDownAndUp)
//                    {
//                        if (ChechFreePlace(map, h, w, DirectionForRemovePillars.RightDown))
//                        {
//                            isRightPillar = false;
//                        }
//                        if (ChechFreePlace(map, h, w, DirectionForRemovePillars.UpRight))
//                        {
//                            isLeftPillar = false;
//                        }
//                        if (!isLeftPillar && !isRightPillar)
//                        {
//                            mapAdditional[h, w] = (int)BranchDirection.RightBetweenDownAndUpWithoutBothPillars;
//                        }
//                        else
//                        {
//                            if (!isLeftPillar)
//                            {
//                                mapAdditional[h, w] = (int)BranchDirection.RightBetweenDownAndUpWithoutDownPillar;
//                            }
//                            if (!isRightPillar)
//                            {
//                                mapAdditional[h, w] = (int)BranchDirection.RightBetweenDownAndUpWithoutUpPillar;
//                            }
//                        }
//                    }
//                    if (map[h, w] == (int)BranchDirection.DownBetweenRightAndLeft)
//                    {
//                        if (ChechFreePlace(map, h, w, DirectionForRemovePillars.DownLeft))
//                        {
//                            isRightPillar = false;
//                        }
//                        if (ChechFreePlace(map, h, w, DirectionForRemovePillars.RightDown))
//                        {
//                            isLeftPillar = false;
//                        }
//                        if (!isLeftPillar && !isRightPillar)
//                        {
//                            mapAdditional[h, w] = (int)BranchDirection.DownBetweenRightAndLeftWithoutBothPillars;
//                        }
//                        else
//                        {
//                            if (!isLeftPillar)
//                            {
//                                mapAdditional[h, w] = (int)BranchDirection.DownBetweenRightAndLeftWithoutRightPillar;
//                            }
//                            if (!isRightPillar)
//                            {
//                                mapAdditional[h, w] = (int)BranchDirection.DownBetweenRightAndLeftWithoutLeftPillar;
//                            }
//                        }
//                    }

//                    // For crossing
//                    bool isLeftUpPillar = true;
//                    bool isUpRightPillar = true;
//                    bool isRightDownPillar = true;
//                    bool isDownLeftPillar = true;
//                    int count = 0;
//                    if (map[h, w] == (int)CrossingDirection.AllSides)
//                    {
//                        if (ChechFreePlace(map, h, w, DirectionForRemovePillars.LeftUp))
//                        {
//                            isLeftUpPillar = false;
//                            count++;
//                        }
//                        if (ChechFreePlace(map, h, w, DirectionForRemovePillars.UpRight))
//                        {
//                            isUpRightPillar = false;
//                            count++;
//                        }
//                        if (ChechFreePlace(map, h, w, DirectionForRemovePillars.RightDown))
//                        {
//                            isRightDownPillar = false;
//                            count++;
//                        }
//                        if (ChechFreePlace(map, h, w, DirectionForRemovePillars.DownLeft))
//                        {
//                            isDownLeftPillar = false;
//                            count++;
//                        }
//                        if (count == 4)
//                        {
//                            mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutFourPillars;
//                        }
//                        if (count == 3)
//                        {
//                            if (!isDownLeftPillar && !isLeftUpPillar && !isUpRightPillar)
//                            {
//                                mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutThreePillarsLeft;
//                            }
//                            if (!isLeftUpPillar && !isUpRightPillar && !isRightDownPillar)
//                            {
//                                mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutThreePillarsUp;
//                            }
//                            if (!isUpRightPillar && !isRightDownPillar && !isDownLeftPillar)
//                            {
//                                mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutThreePillarsRight;
//                            }
//                            if (!isRightDownPillar && !isDownLeftPillar && !isLeftUpPillar)
//                            {
//                                mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutThreePillarsDown;
//                            }
//                        }
//                        if (count == 2)
//                        {
//                            if (!isLeftUpPillar && !isUpRightPillar)
//                            {
//                                mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutTwoPillarsNeighboursUp;
//                            }
//                            if (!isUpRightPillar && !isRightDownPillar)
//                            {
//                                mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutTwoPillarsNeighboursRight;
//                            }
//                            if (!isRightDownPillar && !isDownLeftPillar)
//                            {
//                                mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutTwoPillarsNeighboursDown;
//                            }
//                            if (!isDownLeftPillar && !isLeftUpPillar)
//                            {
//                                mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutTwoPillarsNeighboursLeft;
//                            }

//                            if (!isLeftUpPillar && !isRightDownPillar)
//                            {
//                                mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutTwoPillarsNotNeighboursLeftUp;
//                            }
//                            if (!isUpRightPillar && !isDownLeftPillar)
//                            {
//                                mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutTwoPillarsNotNeighboursUpRight;
//                            }
//                        }
//                        if (count == 1)
//                        {
//                            if (!isLeftUpPillar)
//                            {
//                                mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutOnePillarLeftUp;
//                            }
//                            if (!isUpRightPillar)
//                            {
//                                mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutOnePillarUpRight;
//                            }
//                            if (!isRightDownPillar)
//                            {
//                                mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutOnePillarRightDown;
//                            }
//                            if (!isDownLeftPillar)
//                            {
//                                mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutOnePillarDownLeft;
//                            }
//                        }
//                        if (count == 0)
//                        {
//                            mapAdditional[h, w] = (int)CrossingDirection.AllSides;
//                        }
//                    }
//                }
//            }

//            for (int h = 1; h < map.GetUpperBound(0); h++)
//            {
//                for (int w = 1; w < map.GetUpperBound(1); w++)
//                {
//                    if (mapAdditional[h, w] != 0)
//                    {
//                        map[h, w] = mapAdditional[h, w];
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// Checking that there are no walls between two cells
//        /// </summary>
//        /// <param name="map"> Map (array of integers) </param>
//        /// <param name="h"> Height index </param>
//        /// <param name="w"> Width index </param>
//        /// <param name="direction"> Direction of inspection </param>
//        /// <returns> <see langword="true"/> - if there are no walls, <see langword="false"/> - if there are walls </returns>
//        /// <remarks> Проверка, что между двумя клетками нет стен </remarks>
//        private bool ChechFreePlace(int[,] map, int h, int w, DirectionForRemovePillars direction)
//        {
//            return direction switch
//            {
//                DirectionForRemovePillars.LeftUp => map[h, w + 1] != 0 && map[h - 1, w] != 0 && map[h - 1, w + 2] != 0 && map[h - 2, w + 1] != 0,//Left Up (Left h - 1) (Up w + 1)
//                DirectionForRemovePillars.UpRight => map[h, w + 1] != 0 && map[h + 1, w] != 0 && map[h + 1, w + 2] != 0 && map[h + 2, w + 1] != 0,//Up Right (Up w + 1) (Right h + 1)
//                DirectionForRemovePillars.RightDown => map[h, w - 1] != 0 && map[h + 1, w] != 0 && map[h + 1, w - 2] != 0 && map[h + 2, w - 1] != 0,//Right Down (Right h + 1) (Down w - 1)
//                DirectionForRemovePillars.DownLeft => map[h, w - 1] != 0 && map[h - 1, w] != 0 && map[h - 1, w - 2] != 0 && map[h - 2, w - 1] != 0,//Down Left (Down w - 1) (Left h - 1)
//                _ => false,
//            };
//        }

//        private void DrawMap(int[,] map)
//        {
//            //create floor
//            float height = 0F;
//            float size = _enviromentPrefabsData.CellSize / 2;
//            //object_being_created = Instantiate(_enviromentPrefabsData.Floor, Parent.transform);
//            //object_being_created.transform.localScale = new Vector3((map.GetUpperBound(1) / 2) * size, 1, (map.GetUpperBound(0) / 2) * size);
//            //object_being_created.transform.localPosition = new Vector3((map.GetUpperBound(1) / 2) * size, height, (map.GetUpperBound(0) / 2) * size);

//            //create cells
//            for (int i = 1; i <= map.GetUpperBound(0); i += 2)
//            {
//                for (int j = 1; j <= map.GetUpperBound(1); j += 2)
//                {
//                    //Debug.Log($"Draw {map[i, j]} ({i}, {j})");

                    
//                    coordinates = new Vector3(j * size, height, (map.GetUpperBound(0) - i) * size);
//                    if (map[i, j] == 5)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.StartCell, DirectionForInstance.Up);

//                        coordinates += new Vector3(0F, 0.001F, 0F);
//                        var angle = new Vector3(0F, (float)DirectionForInstance.Up, 0F); //need change

//                        //_applicationManager.GameController?.SetNewSpawnPlaceInLevel(coordinates, angle);

//                        //if (am != null)
//                        //{
//                        //    Debug.Log("!= null");
//                        //    am.SetNewSpawnPlaceInLevel(coordinates);
//                        //}
//                    }
//                    if (map[i, j] == 6)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.EndCell, DirectionForInstance.Down);

//                        coordinates += new Vector3(0F, 0.001F, 0F);
//                        var angle = new Vector3(0F, (float)DirectionForInstance.Down, 0F); //need change

//                        //_applicationManager.GameController?.SetNewSpawnPlaceInLevelEnemy(coordinates, angle);
//                        //_applicationManager.EnemyController?.SetPosition(coordinates, angle);
//                        //_applicationManager.EnemyController?.SetTarget(_applicationManager.PlayerController.transform);
//                    }
//                    //if (map[i, j] == 7)
//                    //{
//                    //    object_being_created = Instantiate(Room);
//                    //    object_being_created.transform.position = coordinates;
//                    //    object_being_created.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
//                    //}
//                    if (map[i, j] == (int)BlindAlleyDirection.Left)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.DeadEndCell, DirectionForInstance.Left);
//                    }
//                    if (map[i, j] == (int)BlindAlleyDirection.Up)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.DeadEndCell, DirectionForInstance.Up);
//                    }
//                    if (map[i, j] == (int)BlindAlleyDirection.Right)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.DeadEndCell, DirectionForInstance.Right);
//                    }
//                    if (map[i, j] == (int)BlindAlleyDirection.Down)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.DeadEndCell, DirectionForInstance.Down);
//                    }

//                    // CeilingStraight
//                    if (map[i, j] == (int)StraightDirection.LeftToRight)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.StraightCell, DirectionForInstance.Left);
//                    }
//                    if (map[i, j] == (int)StraightDirection.UpToDown)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.StraightCell, DirectionForInstance.Up);
//                    }

//                    // CeilingAngle
//                    if (map[i, j] == (int)AngleDirection.LeftToUp)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CornerCell, DirectionForInstance.Left);
//                    }
//                    if (map[i, j] == (int)AngleDirection.LeftToUpWithoutPillar)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CornerCellWithoutPillar, DirectionForInstance.Left);
//                    }
//                    if (map[i, j] == (int)AngleDirection.UpToRight)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CornerCell, DirectionForInstance.Up);
//                    }
//                    if (map[i, j] == (int)AngleDirection.UpToRightWithoutPillar)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CornerCellWithoutPillar, DirectionForInstance.Up);
//                    }
//                    if (map[i, j] == (int)AngleDirection.RightToDown)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CornerCell, DirectionForInstance.Right);
//                    }
//                    if (map[i, j] == (int)AngleDirection.RightToDownWithoutPillar)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CornerCellWithoutPillar, DirectionForInstance.Right);
//                    }
//                    if (map[i, j] == (int)AngleDirection.DownToLeft)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CornerCell, DirectionForInstance.Down);
//                    }
//                    if (map[i, j] == (int)AngleDirection.DownToLeftWithoutPillar)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CornerCellWithoutPillar, DirectionForInstance.Down);
//                    }

//                    // CeilingBranch
//                    if (map[i, j] == (int)BranchDirection.LeftBetweenUpAndDown)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.BranchCell, DirectionForInstance.Left);
//                    }
//                    if (map[i, j] == (int)BranchDirection.LeftBetweenUpAndDownWithoutDownPillar)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.BranchCellWithoutLeftPillar, DirectionForInstance.Left);
//                    }
//                    if (map[i, j] == (int)BranchDirection.LeftBetweenUpAndDownWithoutUpPillar)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.BranchCellWithoutRightPillar, DirectionForInstance.Left);
//                    }
//                    if (map[i, j] == (int)BranchDirection.LeftBetweenUpAndDownWithoutBothPillars)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.BranchCellWithoutBothPillars, DirectionForInstance.Left);
//                    }
//                    if (map[i, j] == (int)BranchDirection.UpBetweenLeftAndRight)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.BranchCell, DirectionForInstance.Up);
//                    }
//                    if (map[i, j] == (int)BranchDirection.UpBetweenLeftAndRightWithoutLeftPillar)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.BranchCellWithoutLeftPillar, DirectionForInstance.Up);
//                    }
//                    if (map[i, j] == (int)BranchDirection.UpBetweenLeftAndRightWithoutRightPillar)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.BranchCellWithoutRightPillar, DirectionForInstance.Up);
//                    }
//                    if (map[i, j] == (int)BranchDirection.UpBetweenLeftAndRightWithoutBothPillars)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.BranchCellWithoutBothPillars, DirectionForInstance.Up);
//                    }
//                    if (map[i, j] == (int)BranchDirection.RightBetweenDownAndUp)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.BranchCell, DirectionForInstance.Right);
//                    }
//                    if (map[i, j] == (int)BranchDirection.RightBetweenDownAndUpWithoutDownPillar)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.BranchCellWithoutLeftPillar, DirectionForInstance.Right);
//                    }
//                    if (map[i, j] == (int)BranchDirection.RightBetweenDownAndUpWithoutUpPillar)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.BranchCellWithoutRightPillar, DirectionForInstance.Right);
//                    }
//                    if (map[i, j] == (int)BranchDirection.RightBetweenDownAndUpWithoutBothPillars)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.BranchCellWithoutBothPillars, DirectionForInstance.Right);
//                    }
//                    if (map[i, j] == (int)BranchDirection.DownBetweenRightAndLeft)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.BranchCell, DirectionForInstance.Down);
//                    }
//                    if (map[i, j] == (int)BranchDirection.DownBetweenRightAndLeftWithoutRightPillar)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.BranchCellWithoutLeftPillar, DirectionForInstance.Down);
//                    }
//                    if (map[i, j] == (int)BranchDirection.DownBetweenRightAndLeftWithoutLeftPillar)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.BranchCellWithoutRightPillar, DirectionForInstance.Down);
//                    }
//                    if (map[i, j] == (int)BranchDirection.DownBetweenRightAndLeftWithoutBothPillars)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.BranchCellWithoutBothPillars, DirectionForInstance.Down);
//                    }

//                    if (map[i, j] == (int)CrossingDirection.AllSides)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCell);
//                    }
//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutOnePillarLeftUp)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutOnePillar, DirectionForInstance.Left);
//                    }
//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutOnePillarUpRight)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutOnePillar, DirectionForInstance.Up);
//                    }
//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutOnePillarRightDown)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutOnePillar, DirectionForInstance.Right);
//                    }
//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutOnePillarDownLeft)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutOnePillar, DirectionForInstance.Down);
//                    }

//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutTwoPillarsNeighboursLeft)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutTwoNeighbourPillars, DirectionForInstance.Left);
//                    }
//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutTwoPillarsNeighboursUp)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutTwoNeighbourPillars, DirectionForInstance.Up);
//                    }
//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutTwoPillarsNeighboursRight)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutTwoNeighbourPillars, DirectionForInstance.Right);
//                    }
//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutTwoPillarsNeighboursDown)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutTwoNeighbourPillars, DirectionForInstance.Down);
//                    }

//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutTwoPillarsNotNeighboursLeftUp)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutTwoNotNeighbourPillars, DirectionForInstance.Left);
//                    }
//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutTwoPillarsNotNeighboursUpRight)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutTwoNotNeighbourPillars, DirectionForInstance.Up);
//                    }
//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutTwoPillarsNotNeighboursRightDown)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutTwoNotNeighbourPillars, DirectionForInstance.Right);
//                    }
//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutTwoPillarsNotNeighboursDownLeft)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutTwoNotNeighbourPillars, DirectionForInstance.Down);
//                    }

//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutThreePillarsLeft)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutThreePillars, DirectionForInstance.Left);
//                    }
//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutThreePillarsUp)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutThreePillars, DirectionForInstance.Up);
//                    }
//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutThreePillarsRight)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutThreePillars, DirectionForInstance.Right);
//                    }
//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutThreePillarsDown)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutThreePillars, DirectionForInstance.Down);
//                    }

//                    if (map[i, j] == (int)CrossingDirection.AllSidesWithoutFourPillars)
//                    {
//                        Createanenvironmentinstance(_enviromentPrefabsData.CrossCellWithoutFourPillars);
//                    }
//                }
//            }
//            //for (int i = 1; i < map.GetUpperBound(0); i++)
//            //{
//            //    for (int j = i % 2 + 1; j < map.GetUpperBound(1); j += 2)
//            //    {
//            //        if (map[i, j] == 10 || map[i, j] == 11)
//            //        {
//            //            //Debug.Log(i + " " + j);
//            //            if (i % 2 == 0)
//            //            {
//            //                //Debug.Log(111);
//            //                coordinates = new Vector3(0.7f + j * 1f, 2f, (map.GetUpperBound(0) - i) * 1f);
//            //                object_being_created = Instantiate(DoorLattice);
//            //                if (i < 5)
//            //                {
//            //                    if (j < 5)
//            //                    {
//            //                        //object_being_created.GetComponent<Locker_Unlocker>().pasword = "1";
//            //                    }
//            //                    else
//            //                    {
//            //                        //object_being_created.GetComponent<Locker_Unlocker>().pasword = "2";
//            //                    }
//            //                    //object_being_created.GetComponent<Locker_Unlocker>().is_locked = true;
//            //                }
//            //                else
//            //                {
//            //                    //object_being_created.GetComponent<Locker_Unlocker>().is_locked = false;
//            //                }
//            //                object_being_created.transform.position = coordinates;
//            //                object_being_created.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
//            //            }
//            //            else
//            //            {
//            //                coordinates = new Vector3(j * 1f, 2f, (map.GetUpperBound(0) - i) * 1f - 0.65f);
//            //                object_being_created = Instantiate(DoorLattice);
//            //                if (i < 5)
//            //                {
//            //                    if (j < 5)
//            //                    {
//            //                        //object_being_created.GetComponent<Locker_Unlocker>().pasword = "1";
//            //                    }
//            //                    else
//            //                    {
//            //                        //object_being_created.GetComponent<Locker_Unlocker>().pasword = "2";
//            //                    }
//            //                    //object_being_created.GetComponent<Locker_Unlocker>().is_locked = true;
//            //                }
//            //                else
//            //                {
//            //                    //object_being_created.GetComponent<Locker_Unlocker>().is_locked = false;
//            //                }
//            //                object_being_created.transform.position = coordinates;
//            //                object_being_created.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);
//            //            }
//            //        }
//            //    }
//            //}
//            //coordinates = new Vector3(1f, 2f, 1f);
//            //coordinates = new Vector3(1.1f, 2f, 1f);
//            //object_being_created = GameObject.FindGameObjectWithTag("Player");
//            //object_being_created.transform.position = coordinates;
//            //object_being_created.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);

//            //coordinates = new Vector3(map_main.GetUpperBound(1) * 1f - 3, 3.5f, map_main.GetUpperBound(0) * 1f - 1);
//            //object_being_created = GameObject.Find("Enemy");
//            //object_being_created.transform.position = coordinates;
//            //object_being_created.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
//        }

//        private void Createanenvironmentinstance(GameObject prefab, DirectionForInstance direction = DirectionForInstance.Left)
//        {
//            object_being_created = Instantiate(prefab, _parentForEviroment.transform);
//            object_being_created.transform.position = coordinates;
//            object_being_created.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (float)direction, transform.eulerAngles.z);
//        }

//        private void ClearInvironments()
//        {
//            Vector3 position = Vector3.zero;
//            for (int i = _parentForEviroment.transform.childCount - 1; i >= 0; i--)
//            {
//                // fix nav mesh bake method
//                position = _parentForEviroment.transform.GetChild(i).gameObject.transform.position;
//                position.y -= 10;
//                _parentForEviroment.transform.GetChild(i).gameObject.transform.position = position;

//                Destroy(_parentForEviroment.transform.GetChild(i).gameObject);
//            }
//        }

//        private void Map_Saver()
//        {
//            //string map = "";
//            //for (int i = 0; i < height; i++)
//            //{
//            //    for (int j = 0; j < width; j++)
//            //    {
//            //        map += map_main[i, j];
//            //        map += ",";
//            //    }
//            //    map += ";";
//            //}
//            //PlayerPrefs.SetString("Map", map);

//            //if (PlayerPrefs.GetString("Map") == "")
//            //{
//            //CreatingMap();
//            //Map_Saver();
//            //GameObject.Find("Enemy").GetComponent<NewBehaviourScript>().Map_Load();
//            //}
//            //else
//            //{
//            //    Map_Load();
//            //}
//            //Debug.Log(map_main.Length);
//            //Drawing_Map(map_main);

//            //MyStart();
//            /*height = PlayerPrefs.GetInt("Size_h");
//            width = PlayerPrefs.GetInt("Size_w");
//            map_main = new int[height, width];
//            if (PlayerPrefs.GetString("Map") == "")
//            {
//                Creation_Map();
//                Map_Saver();
//                //GameObject.Find("Enemy").GetComponent<NewBehaviourScript>().Map_Load();
//            }
//            else
//            {
//                Map_Load();
//            }
//            Drawing_Map(map_main);*/
//        }

//        private void Map_Load()
//        {
//            //string map = PlayerPrefs.GetString("Map");
//            //int i = 0;
//            //int j = 0;
//            //string number = "";
//            //foreach (char el in map)
//            //{
//            //    if (el == ',' || el == ';')
//            //    {
//            //        if (el == ',')
//            //        {
//            //            //map_main[j, i] = int.Parse(number, System.Globalization.NumberStyles.Integer);
//            //            map_main[j, i] = int.Parse(number);
//            //            i++;
//            //        }
//            //        if (el == ';')
//            //        {
//            //            i = 0;
//            //            j++;
//            //        }
//            //        number = "";
//            //    }
//            //    else
//            //    {
//            //        number += el;
//            //    }
//            //}
//        }
//    }
//}