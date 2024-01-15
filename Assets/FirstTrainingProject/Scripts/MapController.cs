using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    #region Public Fields

    [SerializeField]
    private ApplicationManager _applicationManager;

    [Header("Enviroment parent")]
    public GameObject Parent;

    [Header("Enviroment prefabs")]
    //public GameObject WallBlank;
    //public GameObject FloorBlank;
    [SerializeField]
    private GameObject CeilingStraight;
    [SerializeField]
    private GameObject CeilingBlindAlley;
    [SerializeField]
    private GameObject CeilingAngle;
    [SerializeField]
    private GameObject CeilingAngleWithoutPillar;
    [SerializeField]
    private GameObject CeilingBranch;
    [SerializeField]
    private GameObject CeilingBranchWithoutLeftPillar;
    [SerializeField]
    private GameObject CeilingBranchWithoutRightPillar;
    [SerializeField]
    private GameObject CeilingBranchWithoutBothPillars;
    [SerializeField]
    private GameObject CeilingCrossing;
    [SerializeField]
    private GameObject CeilingCrossingWithoutOnePillar;
    [SerializeField]
    private GameObject CeilingCrossingWithoutTwoNeighbourPillar;
    [SerializeField]
    private GameObject CeilingCrossingWithoutTwoNotNeighbourPillar;
    [SerializeField]
    private GameObject CeilingCrossingWithoutThreePillar;
    [SerializeField]
    private GameObject CeilingCrossingWithoutFourPillars;
    //public GameObject DoorLattice;
    [SerializeField]
    private GameObject StartPoint;
    [SerializeField]
    private GameObject EndPoint;
    //public GameObject Room;

    #endregion

    [Header("Other")]

    //Variables
    static int height;
    static int width;

    #region Private Fields

    private int[,] map_main;
    private int[,] map_additional;

    private int points_list_length = 0;
    private int[,] points_list;
    private int[,] neighbors_list;
    private int neighbors_count = 0;
    private int random_neighbor;

    private Vector3 coordinates;
    private GameObject object_being_created;

    #endregion

    private void Awake()
    {
        //if (WallBlank == null) throw new System.Exception($"WallBlank not set!");
        //if (FloorBlank == null) throw new System.Exception($"FloorBlank not set!");
        if (CeilingStraight == null) throw new System.Exception($"CeilingStraight not set!");
        if (CeilingBlindAlley == null) throw new System.Exception($"CeilingBlindAlley not set!");
        if (CeilingAngle == null) throw new System.Exception($"CeilingAngle not set!");
        if (CeilingAngleWithoutPillar == null) throw new System.Exception($"CeilingAngleWithoutPillar not set!");
        if (CeilingBranch == null) throw new System.Exception($"CeilingBranch not set!");
        if (CeilingBranchWithoutLeftPillar == null) throw new System.Exception($"CeilingBranchWithoutLeftPillar not set!");
        if (CeilingBranchWithoutRightPillar == null) throw new System.Exception($"CeilingBranchWithoutRightPillar not set!");
        if (CeilingBranchWithoutBothPillars == null) throw new System.Exception($"CeilingBranchWithoutBothPillars not set!");
        if (CeilingCrossing == null) throw new System.Exception($"CeilingCrossing not set!");
        if (CeilingCrossingWithoutOnePillar == null) throw new System.Exception($"CeilingCrossingWithoutOnePillar not set!");
        if (CeilingCrossingWithoutTwoNeighbourPillar == null) throw new System.Exception($"CeilingCrossingWithoutTwoNeighbourPillar not set!");
        if (CeilingCrossingWithoutTwoNotNeighbourPillar == null) throw new System.Exception($"CeilingCrossingWithoutTwoNotNeighbourPillar not set!");
        if (CeilingCrossingWithoutThreePillar == null) throw new System.Exception($"CeilingCrossingWithoutThreePillar not set!");
        if (CeilingCrossingWithoutFourPillars == null) throw new System.Exception($"CeilingCrossingWithoutFourPillars not set!");
        if (StartPoint == null) throw new System.Exception($"StartPoint not set!");
        if (EndPoint == null) throw new System.Exception($"EndPoint not set!");

        _applicationManager.MapController = this;

        height = 15;//PlayerPrefs.GetInt("Size_h"); //only !/2 (3, 5, 7)
        width = 21;//PlayerPrefs.GetInt("Size_w"); //only !/2 (3, 5, 7)
        map_main = new int[height, width];
        //if (PlayerPrefs.GetString("Map") == "")
        //{
        //CreatingMap();
        //Map_Saver();
        //GameObject.Find("Enemy").GetComponent<NewBehaviourScript>().Map_Load();
        //}
        //else
        //{
        //    Map_Load();
        //}
        //Debug.Log(map_main.Length);
        //Drawing_Map(map_main);
    }

    void Start()
    {
        MyStart();
        /*height = PlayerPrefs.GetInt("Size_h");
        width = PlayerPrefs.GetInt("Size_w");
        map_main = new int[height, width];
        if (PlayerPrefs.GetString("Map") == "")
        {
            Creation_Map();
            Map_Saver();
            //GameObject.Find("Enemy").GetComponent<NewBehaviourScript>().Map_Load();
        }
        else
        {
            Map_Load();
        }
        Drawing_Map(map_main);*/
    }

    void Update()
    {

    }

    public void MyStart()
    {
        CreatingMap();
        Drawing_Map(map_main);
    }

    private void CreatingMap()
    {
        map_additional = new int[height, width];
        points_list = new int[((height - 1) / 2) * ((width - 1) / 2), 2];
        neighbors_list = new int[4, 2];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (i % 2 == 1 && j % 2 == 1)
                {
                    map_main[i, j] = 1;
                    map_additional[i, j] = 1;
                }
                else
                {
                    map_main[i, j] = 0;
                    map_additional[i, j] = 0;
                }
            }
        }

        //room in map create
        for (int i = 1; i <= 6; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                if (i == 6 || j == 4)
                {
                    map_main[i, j] = 0;
                    map_additional[i, j] = 0;
                }
                else
                {
                    map_main[i, j] = 2;
                    map_additional[i, j] = 2;
                }
            }
        }
        map_main[3, 4] = 2;
        map_additional[3, 4] = 2;

        //room in map create ?
        map_main[height - 2, 1] = 2;
        map_main[height - 2, 2] = 2;
        map_main[height - 2, 3] = 2;

        map_additional[height - 2, 1] = 2;
        map_additional[height - 2, 2] = 2;
        map_additional[height - 2, 3] = 2;

        map_main[1, width - 2] = 2;
        map_main[1, width - 3] = 2;
        map_main[1, width - 4] = 1;

        map_additional[1, width - 2] = 2;
        map_additional[1, width - 3] = 2;
        map_additional[1, width - 4] = 1;

        points_list[0, 0] = height - 2;
        points_list[0, 1] = 3;
        map_main[points_list[0, 0], points_list[0, 1]] = 2;
        map_additional[points_list[0, 0], points_list[0, 1]] = 2;


        // Finding the base path
        // Поиск базового пути
        points_list_length = 0;
        while (points_list_length >= 0)
        {
            Search_For_Seighbors(points_list_length, points_list, map_main, ref neighbors_count, ref neighbors_list);
            Creation_Path(neighbors_count, neighbors_list, ref points_list_length, ref points_list, ref map_main);
        }

        // Finding an extra path (If you need only one way - remove)
        // Поиск дополнительного пути (Если нужен только один путь - убрать)
        points_list_length = 0;
        while (points_list_length >= 0)
        {
            Search_For_Seighbors(points_list_length, points_list, map_additional, ref neighbors_count, ref neighbors_list);
            Creation_Path(neighbors_count, neighbors_list, ref points_list_length, ref points_list, ref map_additional);
        }

        // With a 30 percent chance we combine the main and additional routes
        // С шансом 30 процентов объединяем основной и дополнительные маршруты
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (map_additional[i, j] == 2 && map_main[i, j] != 2 && Random.Range(0, 3) == 0)
                {
                    map_main[i, j] = map_additional[i, j];
                }
            }
        }

        CellTypeDefinition(map_main);
        VoidsForPremisesDefinition(map_main);

        map_main[height - 2, 1] = 5; //set start point
        map_main[1, width - 2] = 6; //set end point

        map_main[height - 2, 2] = 11; //path with start point for algoritm
        map_main[3, 4] = 11; //second path with start point for algoritm
        map_main[1, width - 3] = 11; //ent path for algoritm

        //map_main[1, 1] = 7; //room point
        //for (int i = 1; i <= 5; i++)
        //{
        //    for (int j = 1; j <= 3; j++)
        //    {
        //        map_main[i, j] = 0;
        //    }
        //}
        //map_main[1, 1] = 7;
    }

    private void Search_For_Seighbors(int point_list_size, int[,] point_list, int[,] map, ref int number_of_neighbors, ref int[,] list_of_neighbors)
    {
        number_of_neighbors = 0;
        if (point_list[point_list_size, 0] > 2)
        {
            if (map[point_list[point_list_size, 0] - 2, point_list[point_list_size, 1]] == 1)
            {
                list_of_neighbors[number_of_neighbors, 0] = point_list[point_list_size, 0] - 2;
                list_of_neighbors[number_of_neighbors, 1] = point_list[point_list_size, 1];
                number_of_neighbors++;
            }
        }
        if (point_list[point_list_size, 1] < map.GetUpperBound(1) - 2)
        {
            if (map[point_list[point_list_size, 0], point_list[point_list_size, 1] + 2] == 1)
            {
                list_of_neighbors[number_of_neighbors, 0] = point_list[point_list_size, 0];
                list_of_neighbors[number_of_neighbors, 1] = point_list[point_list_size, 1] + 2;
                number_of_neighbors++;
            }
        }
        if (point_list[point_list_size, 0] < map.GetUpperBound(0) - 2)
        {
            if (map[point_list[point_list_size, 0] + 2, point_list[point_list_size, 1]] == 1)
            {
                list_of_neighbors[number_of_neighbors, 0] = point_list[point_list_size, 0] + 2;
                list_of_neighbors[number_of_neighbors, 1] = point_list[point_list_size, 1];
                number_of_neighbors++;
            }
        }
        if (point_list[point_list_size, 1] > 2)
        {
            if (map[point_list[point_list_size, 0], point_list[point_list_size, 1] - 2] == 1)
            {
                list_of_neighbors[number_of_neighbors, 0] = point_list[point_list_size, 0];
                list_of_neighbors[number_of_neighbors, 1] = point_list[point_list_size, 1] - 2;
                number_of_neighbors++;
            }
        }
    }

    private void Creation_Path(int number_of_neighbors, int[,] list_of_neighbors, ref int point_list_size, ref int[,] point_list, ref int[,] map)
    {
        if (number_of_neighbors > 0)
        {
            random_neighbor = Random.Range(0, number_of_neighbors);
            point_list_size++;
            point_list[point_list_size, 0] = list_of_neighbors[random_neighbor, 0];
            point_list[point_list_size, 1] = list_of_neighbors[random_neighbor, 1];
            map[Mathf.Abs(point_list[point_list_size, 0] + point_list[point_list_size - 1, 0]) / 2, Mathf.Abs(point_list[point_list_size, 1] + point_list[point_list_size - 1, 1]) / 2] = 2;
            map[point_list[point_list_size, 0], point_list[point_list_size, 1]] = 2; //2 - ��������
        }
        else
        {
            point_list_size--;
        }
    }

    /// <summary>
    /// Determining the cell type
    /// </summary>
    /// <param name="map"> Map (array of integers) </param>
    /// <remarks> Определение типа ячейки </remarks>
    private void CellTypeDefinition(int[,] map)
    {
        int numberOfNeighbors;
        bool[] directionOfNeighbors = new bool[4]; // Left - Up - Right - Down

        for (int h = 1; h < map.GetUpperBound(0); h += 2)
        {
            for (int w = 1; w < map.GetUpperBound(1); w += 2)
            {
                if (map[h, w] >= 2)
                {
                    // Determining the number and location of neighbors
                    // Определение количества и расположения соседей
                    numberOfNeighbors = 0;
                    for (int i = 0; i < directionOfNeighbors.Length; i++)
                    {
                        directionOfNeighbors[i] = false;
                    }

                    if (map[h - 1, w] >= 2)
                    {
                        directionOfNeighbors[0] = true;
                        numberOfNeighbors++;
                    }
                    if (map[h, w + 1] >= 2)
                    {
                        directionOfNeighbors[1] = true;
                        numberOfNeighbors++;
                    }
                    if (map[h + 1, w] >= 2)
                    {
                        directionOfNeighbors[2] = true;
                        numberOfNeighbors++;
                    }
                    if (map[h, w - 1] >= 2)
                    {
                        directionOfNeighbors[3] = true;
                        numberOfNeighbors++;
                    }

                    // Determining the direction for a cell with one open wall
                    // Определение направления для ячейки с одной открытой стенкой
                    if (numberOfNeighbors == 1)
                    {
                        if (directionOfNeighbors[0])
                        {
                            map[h, w] = (int)BlindAlleyDirection.Left;
                        }
                        if (directionOfNeighbors[1])
                        {
                            map[h, w] = (int)BlindAlleyDirection.Up;
                        }
                        if (directionOfNeighbors[2])
                        {
                            map[h, w] = (int)BlindAlleyDirection.Right;
                        }
                        if (directionOfNeighbors[3])
                        {
                            map[h, w] = (int)BlindAlleyDirection.Down;
                        }
                    }

                    // Determining the direction for a cell with two open walls
                    // Определение направления для ячейки с двумя открытыми стенками
                    if (numberOfNeighbors == 2)
                    {
                        if (directionOfNeighbors[0] && directionOfNeighbors[2])
                        {
                            map[h, w] = (int)StraightDirection.LeftToRight;
                        }
                        if (directionOfNeighbors[1] && directionOfNeighbors[3])
                        {
                            map[h, w] = (int)StraightDirection.UpToDown;
                        }
                        if (directionOfNeighbors[0] && directionOfNeighbors[1])
                        {
                            map[h, w] = (int)AngleDirection.LeftToUp;
                        }
                        if (directionOfNeighbors[1] && directionOfNeighbors[2])
                        {
                            map[h, w] = (int)AngleDirection.UpToRight;
                        }
                        if (directionOfNeighbors[2] && directionOfNeighbors[3])
                        {
                            map[h, w] = (int)AngleDirection.RightToDown;
                        }
                        if (directionOfNeighbors[3] && directionOfNeighbors[0])
                        {
                            map[h, w] = (int)AngleDirection.DownToLeft;
                        }
                    }

                    // Determining the direction for a cell with three open walls
                    // Определение направления для клетки с тремя открытыми стенками
                    if (numberOfNeighbors == 3)
                    {
                        if (directionOfNeighbors[3] && directionOfNeighbors[0] && directionOfNeighbors[1])
                        {
                            map[h, w] = (int)BranchDirection.LeftBetweenUpAndDown;
                        }
                        if (directionOfNeighbors[0] && directionOfNeighbors[1] && directionOfNeighbors[2])
                        {
                            map[h, w] = (int)BranchDirection.UpBetweenLeftAndRight;
                        }
                        if (directionOfNeighbors[1] && directionOfNeighbors[2] && directionOfNeighbors[3])
                        {
                            map[h, w] = (int)BranchDirection.RightBetweenDownAndUp;
                        }
                        if (directionOfNeighbors[2] && directionOfNeighbors[3] && directionOfNeighbors[0])
                        {
                            map[h, w] = (int)BranchDirection.DownBetweenRightAndLeft;
                        }
                    }

                    // Determining the direction of a cross cell
                    // Определение направления поперечной ячейки
                    if (numberOfNeighbors == 4)
                    {
                        map[h, w] = (int)CrossingDirection.AllSides;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Determination of voids for premises (removal of pillars)
    /// </summary>
    /// <param name="map"> Map (array of integers) </param>
    /// <remarks> Определение пустот под помещения (удаление столбов) </remarks>
    private void VoidsForPremisesDefinition(int[,] map)
    {
        int[,] mapAdditional = new int[map.GetLength(0), map.GetLength(1)];

        for (int h = 1; h < map.GetUpperBound(0); h += 2)
        {
            for (int w = 1; w < map.GetUpperBound(1); w += 2)
            {
                // For angle
                if (map[h, w] == (int)AngleDirection.LeftToUp)
                {
                    if (map[h, w + 1] != 0 && map[h - 1, w] != 0 && map[h - 1, w + 2] != 0 && map[h - 2, w + 1] != 0)
                    {
                        mapAdditional[h, w] = (int)AngleDirection.LeftToUpWithoutPillar;
                    }
                }
                if (map[h, w] == (int)AngleDirection.UpToRight)
                {
                    if (map[h, w + 1] != 0 && map[h + 1, w] != 0 && map[h + 1, w + 2] != 0 && map[h + 2, w + 1] != 0)
                    {
                        mapAdditional[h, w] = (int)AngleDirection.UpToRightWithoutPillar;
                    }
                }
                if (map[h, w] == (int)AngleDirection.RightToDown)
                {
                    if (map[h, w - 1] != 0 && map[h + 1, w] != 0 && map[h + 1, w - 2] != 0 && map[h + 2, w - 1] != 0)
                    {
                        mapAdditional[h, w] = (int)AngleDirection.RightToDownWithoutPillar;
                    }
                }
                if (map[h, w] == (int)AngleDirection.DownToLeft)
                {
                    if (map[h, w - 1] != 0 && map[h - 1, w] != 0 && map[h - 1, w - 2] != 0 && map[h - 2, w - 1] != 0)
                    {
                        mapAdditional[h, w] = (int)AngleDirection.DownToLeftWithoutPillar;
                    }
                }

                // For branch
                bool isLeftPillar = true;
                bool isRightPillar = true;
                if (map[h, w] == (int)BranchDirection.LeftBetweenUpAndDown)
                {
                    if (map[h, w + 1] != 0 && map[h - 1, w] != 0 && map[h - 1, w + 2] != 0 && map[h - 2, w + 1] != 0)
                    {
                        isRightPillar = false;
                    }
                    if (map[h, w - 1] != 0 && map[h - 1, w] != 0 && map[h - 1, w - 2] != 0 && map[h - 2, w - 1] != 0)
                    {
                        isLeftPillar = false;
                    }
                    if (!isLeftPillar && !isRightPillar)
                    {
                        mapAdditional[h, w] = (int)BranchDirection.LeftBetweenUpAndDownWithoutBothPillars;
                    }
                    else
                    {
                        if (!isLeftPillar)
                        {
                            mapAdditional[h, w] = (int)BranchDirection.LeftBetweenUpAndDownWithoutDownPillar;
                        }
                        if (!isRightPillar)
                        {
                            mapAdditional[h, w] = (int)BranchDirection.LeftBetweenUpAndDownWithoutUpPillar;
                        }
                    }
                }
                if (map[h, w] == (int)BranchDirection.UpBetweenLeftAndRight)
                {
                    if (map[h, w + 1] != 0 && map[h + 1, w] != 0 && map[h + 1, w + 2] != 0 && map[h + 2, w + 1] != 0)
                    {
                        isRightPillar = false;
                    }
                    if (map[h, w + 1] != 0 && map[h - 1, w] != 0 && map[h - 1, w + 2] != 0 && map[h - 2, w + 1] != 0)
                    {
                        isLeftPillar = false;
                    }
                    if (!isLeftPillar && !isRightPillar)
                    {
                        mapAdditional[h, w] = (int)BranchDirection.UpBetweenLeftAndRightWithoutBothPillars;
                    }
                    else
                    {
                        if (!isLeftPillar)
                        {
                            mapAdditional[h, w] = (int)BranchDirection.UpBetweenLeftAndRightWithoutLeftPillar;
                        }
                        if (!isRightPillar)
                        {
                            mapAdditional[h, w] = (int)BranchDirection.UpBetweenLeftAndRightWithoutRightPillar;
                        }
                    }
                }
                if (map[h, w] == (int)BranchDirection.RightBetweenDownAndUp)
                {
                    if (map[h, w - 1] != 0 && map[h + 1, w] != 0 && map[h + 1, w - 2] != 0 && map[h + 2, w - 1] != 0)
                    {
                        isRightPillar = false;
                    }
                    if (map[h, w + 1] != 0 && map[h + 1, w] != 0 && map[h + 1, w + 2] != 0 && map[h + 2, w + 1] != 0)
                    {
                        isLeftPillar = false;
                    }
                    if (!isLeftPillar && !isRightPillar)
                    {
                        mapAdditional[h, w] = (int)BranchDirection.RightBetweenDownAndUpWithoutBothPillars;
                    }
                    else
                    {
                        if (!isLeftPillar)
                        {
                            mapAdditional[h, w] = (int)BranchDirection.RightBetweenDownAndUpWithoutDownPillar;
                        }
                        if (!isRightPillar)
                        {
                            mapAdditional[h, w] = (int)BranchDirection.RightBetweenDownAndUpWithoutUpPillar;
                        }
                    }
                }
                if (map[h, w] == (int)BranchDirection.DownBetweenRightAndLeft)
                {
                    if (map[h, w - 1] != 0 && map[h - 1, w] != 0 && map[h - 1, w - 2] != 0 && map[h - 2, w - 1] != 0)
                    {
                        isRightPillar = false;
                    }
                    if (map[h, w - 1] != 0 && map[h + 1, w] != 0 && map[h + 1, w - 2] != 0 && map[h + 2, w - 1] != 0)
                    {
                        isLeftPillar = false;
                    }
                    if (!isLeftPillar && !isRightPillar)
                    {
                        mapAdditional[h, w] = (int)BranchDirection.DownBetweenRightAndLeftWithoutBothPillars;
                    }
                    else
                    {
                        if (!isLeftPillar)
                        {
                            mapAdditional[h, w] = (int)BranchDirection.DownBetweenRightAndLeftWithoutRightPillar;
                        }
                        if (!isRightPillar)
                        {
                            mapAdditional[h, w] = (int)BranchDirection.DownBetweenRightAndLeftWithoutLeftPillar;
                        }
                    }
                }

                // For crossing
                bool isLeftUpPillar = true;
                bool isUpRightPillar = true;
                bool isRightDownPillar = true;
                bool isDownLeftPillar = true;
                int count = 0;
                if (map[h, w] == (int)CrossingDirection.AllSides)
                {
                    if (map[h, w + 1] != 0 && map[h - 1, w] != 0 && map[h - 1, w + 2] != 0 && map[h - 2, w + 1] != 0)
                    {
                        isLeftUpPillar = false;
                        count++;
                    }
                    if (map[h, w + 1] != 0 && map[h + 1, w] != 0 && map[h + 1, w + 2] != 0 && map[h + 2, w + 1] != 0)
                    {
                        isUpRightPillar = false;
                        count++;
                    }
                    if (map[h, w - 1] != 0 && map[h + 1, w] != 0 && map[h + 1, w - 2] != 0 && map[h + 2, w - 1] != 0)
                    {
                        isRightDownPillar = false;
                        count++;
                    }
                    if (map[h, w - 1] != 0 && map[h - 1, w] != 0 && map[h - 1, w - 2] != 0 && map[h - 2, w - 1] != 0)
                    {
                        isDownLeftPillar = false;
                        count++;
                    }
                    if (count == 4)
                    {
                        mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutFourPillars;
                    }
                    if (count == 3)
                    {
                        if (!isDownLeftPillar && !isLeftUpPillar && !isUpRightPillar)
                        {
                            mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutThreePillarsLeft;
                        }
                        if (!isLeftUpPillar && !isUpRightPillar && !isRightDownPillar)
                        {
                            mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutThreePillarsUp;
                        }
                        if (!isUpRightPillar && !isRightDownPillar && !isDownLeftPillar)
                        {
                            mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutThreePillarsRight;
                        }
                        if (!isRightDownPillar && !isDownLeftPillar && !isLeftUpPillar)
                        {
                            mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutThreePillarsDown;
                        }
                    }
                    if (count == 2)
                    {
                        if (!isLeftUpPillar && !isUpRightPillar)
                        {
                            mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutTwoPillarsNeighboursUp;
                        }
                        if (!isUpRightPillar && !isRightDownPillar)
                        {
                            mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutTwoPillarsNeighboursRight;
                        }
                        if (!isRightDownPillar && !isDownLeftPillar)
                        {
                            mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutTwoPillarsNeighboursDown;
                        }
                        if (!isDownLeftPillar && !isLeftUpPillar)
                        {
                            mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutTwoPillarsNeighboursLeft;
                        }

                        if (!isLeftUpPillar && !isRightDownPillar)
                        {
                            mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutTwoPillarsNotNeighboursLeftUp;
                        }
                        if (!isUpRightPillar && !isDownLeftPillar)
                        {
                            mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutTwoPillarsNotNeighboursUpRight;
                        }
                    }
                    if (count == 1)
                    {
                        if (!isLeftUpPillar)
                        {
                            mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutOnePillarLeftUp;
                        }
                        if (!isUpRightPillar)
                        {
                            mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutOnePillarUpRight;
                        }
                        if (!isRightDownPillar)
                        {
                            mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutOnePillarRightDown;
                        }
                        if (!isDownLeftPillar)
                        {
                            mapAdditional[h, w] = (int)CrossingDirection.AllSidesWithoutOnePillarDownLeft;
                        }
                    }
                    if (count == 0)
                    {
                        mapAdditional[h, w] = (int)CrossingDirection.AllSides;
                    }
                }
            }
        }

        for (int h = 1; h < map.GetUpperBound(0); h++)
        {
            for (int w = 1; w < map.GetUpperBound(1); w++)
            {
                if (mapAdditional[h, w] != 0)
                {
                    map[h, w] = mapAdditional[h, w];
                }
            }
        }
    }

    private bool AAA(int[,] map, DirectionForRemovePillars direction)
    {
        //map[h, w + 1] != 0 && map[h - 1, w] != 0 && map[h - 1, w + 2] != 0 && map[h - 2, w + 1] != 0 //Left Up (Left -1) 
        //map[h, w + 1] != 0 && map[h + 1, w] != 0 && map[h + 1, w + 2] != 0 && map[h + 2, w + 1] != 0 //Up Right
        //map[h, w - 1] != 0 && map[h + 1, w] != 0 && map[h + 1, w - 2] != 0 && map[h + 2, w - 1] != 0 //Right Down
        //map[h, w - 1] != 0 && map[h - 1, w] != 0 && map[h - 1, w - 2] != 0 && map[h - 2, w - 1] != 0 //Down Left (Left -1) 
        return false;
    }

    private void Drawing_Map(int[,] map)
    {
        Removal_Of_Invironment();
        for (int i = 1; i <= map.GetUpperBound(0); i += 2)
        {
            for (int j = 1; j <= map.GetUpperBound(1); j += 2)
            {
                //Debug.Log($"Draw {map[i, j]} ({i}, {j})");
                coordinates = new Vector3(j * 1f, 2f, (map.GetUpperBound(0) - i) * 1f);
                if (map[i, j] == 5) // Start point
                {
                    Createanenvironmentinstance(StartPoint, DirectionForInstance.Up);

                    coordinates += new Vector3(0F, 0.5F, 0F);
                    var am = _applicationManager.GameController;
                    am.SetNewSpawnPlaceInLevel(coordinates);
                    //if (am != null)
                    //{
                    //    Debug.Log("!= null");
                    //    am.SetNewSpawnPlaceInLevel(coordinates);
                    //}
                }
                if (map[i, j] == 6)
                {
                    Createanenvironmentinstance(EndPoint, DirectionForInstance.Down);
                }
                //if (map[i, j] == 7)
                //{
                //    object_being_created = Instantiate(Room);
                //    object_being_created.transform.position = coordinates;
                //    object_being_created.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
                //}
                if (map[i, j] == (int)BlindAlleyDirection.Left)
                {
                    Createanenvironmentinstance(CeilingBlindAlley, DirectionForInstance.Left);
                }
                if (map[i, j] == (int)BlindAlleyDirection.Up)
                {
                    Createanenvironmentinstance(CeilingBlindAlley, DirectionForInstance.Up);
                }
                if (map[i, j] == (int)BlindAlleyDirection.Right)
                {
                    Createanenvironmentinstance(CeilingBlindAlley, DirectionForInstance.Right);
                }
                if (map[i, j] == (int)BlindAlleyDirection.Down)
                {
                    Createanenvironmentinstance(CeilingBlindAlley, DirectionForInstance.Down);
                }

                // CeilingStraight
                if (map[i, j] == (int)StraightDirection.LeftToRight)
                {
                    Createanenvironmentinstance(CeilingStraight, DirectionForInstance.Left);
                }
                if (map[i, j] == (int)StraightDirection.UpToDown)
                {
                    Createanenvironmentinstance(CeilingStraight, DirectionForInstance.Up);
                }

                // CeilingAngle
                if (map[i, j] == (int)AngleDirection.LeftToUp)
                {
                    Createanenvironmentinstance(CeilingAngle, DirectionForInstance.Left);
                }
                if (map[i, j] == (int)AngleDirection.LeftToUpWithoutPillar)
                {
                    Createanenvironmentinstance(CeilingAngleWithoutPillar, DirectionForInstance.Left);
                }
                if (map[i, j] == (int)AngleDirection.UpToRight)
                {
                    Createanenvironmentinstance(CeilingAngle, DirectionForInstance.Up);
                }
                if (map[i, j] == (int)AngleDirection.UpToRightWithoutPillar)
                {
                    Createanenvironmentinstance(CeilingAngleWithoutPillar, DirectionForInstance.Up);
                }
                if (map[i, j] == (int)AngleDirection.RightToDown)
                {
                    Createanenvironmentinstance(CeilingAngle, DirectionForInstance.Right);
                }
                if (map[i, j] == (int)AngleDirection.RightToDownWithoutPillar)
                {
                    Createanenvironmentinstance(CeilingAngleWithoutPillar, DirectionForInstance.Right);
                }
                if (map[i, j] == (int)AngleDirection.DownToLeft)
                {
                    Createanenvironmentinstance(CeilingAngle, DirectionForInstance.Down);
                }
                if (map[i, j] == (int)AngleDirection.DownToLeftWithoutPillar)
                {
                    Createanenvironmentinstance(CeilingAngleWithoutPillar, DirectionForInstance.Down);
                }

                // CeilingBranch
                if (map[i, j] == (int)BranchDirection.LeftBetweenUpAndDown)
                {
                    Createanenvironmentinstance(CeilingBranch, DirectionForInstance.Left);
                }
                if (map[i, j] == (int)BranchDirection.LeftBetweenUpAndDownWithoutDownPillar)
                {
                    Createanenvironmentinstance(CeilingBranchWithoutLeftPillar, DirectionForInstance.Left);
                }
                if (map[i, j] == (int)BranchDirection.LeftBetweenUpAndDownWithoutUpPillar)
                {
                    Createanenvironmentinstance(CeilingBranchWithoutRightPillar, DirectionForInstance.Left);
                }
                if (map[i, j] == (int)BranchDirection.LeftBetweenUpAndDownWithoutBothPillars)
                {
                    Createanenvironmentinstance(CeilingBranchWithoutBothPillars, DirectionForInstance.Left);
                }
                if (map[i, j] == (int)BranchDirection.UpBetweenLeftAndRight)
                {
                    Createanenvironmentinstance(CeilingBranch, DirectionForInstance.Up);
                }
                if (map[i, j] == (int)BranchDirection.UpBetweenLeftAndRightWithoutLeftPillar)
                {
                    Createanenvironmentinstance(CeilingBranchWithoutLeftPillar, DirectionForInstance.Up);
                }
                if (map[i, j] == (int)BranchDirection.UpBetweenLeftAndRightWithoutRightPillar)
                {
                    Createanenvironmentinstance(CeilingBranchWithoutRightPillar, DirectionForInstance.Up);
                }
                if (map[i, j] == (int)BranchDirection.UpBetweenLeftAndRightWithoutBothPillars)
                {
                    Createanenvironmentinstance(CeilingBranchWithoutBothPillars, DirectionForInstance.Up);
                }
                if (map[i, j] == (int)BranchDirection.RightBetweenDownAndUp)
                {
                    Createanenvironmentinstance(CeilingBranch, DirectionForInstance.Right);
                }
                if (map[i, j] == (int)BranchDirection.RightBetweenDownAndUpWithoutDownPillar)
                {
                    Createanenvironmentinstance(CeilingBranchWithoutLeftPillar, DirectionForInstance.Right);
                }
                if (map[i, j] == (int)BranchDirection.RightBetweenDownAndUpWithoutUpPillar)
                {
                    Createanenvironmentinstance(CeilingBranchWithoutRightPillar, DirectionForInstance.Right);
                }
                if (map[i, j] == (int)BranchDirection.RightBetweenDownAndUpWithoutBothPillars)
                {
                    Createanenvironmentinstance(CeilingBranchWithoutBothPillars, DirectionForInstance.Right);
                }
                if (map[i, j] == (int)BranchDirection.DownBetweenRightAndLeft)
                {
                    Createanenvironmentinstance(CeilingBranch, DirectionForInstance.Down);
                }
                if (map[i, j] == (int)BranchDirection.DownBetweenRightAndLeftWithoutRightPillar)
                {
                    Createanenvironmentinstance(CeilingBranchWithoutLeftPillar, DirectionForInstance.Down);
                }
                if (map[i, j] == (int)BranchDirection.DownBetweenRightAndLeftWithoutLeftPillar)
                {
                    Createanenvironmentinstance(CeilingBranchWithoutRightPillar, DirectionForInstance.Down);
                }
                if (map[i, j] == (int)BranchDirection.DownBetweenRightAndLeftWithoutBothPillars)
                {
                    Createanenvironmentinstance(CeilingBranchWithoutBothPillars, DirectionForInstance.Down);
                }

                if (map[i, j] == (int)CrossingDirection.AllSides)
                {
                    Createanenvironmentinstance(CeilingCrossing);
                }
                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutOnePillarLeftUp)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutOnePillar, DirectionForInstance.Left);
                }
                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutOnePillarUpRight)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutOnePillar, DirectionForInstance.Up);
                }
                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutOnePillarRightDown)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutOnePillar, DirectionForInstance.Right);
                }
                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutOnePillarDownLeft)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutOnePillar, DirectionForInstance.Down);
                }

                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutTwoPillarsNeighboursLeft)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutTwoNeighbourPillar, DirectionForInstance.Left);
                }
                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutTwoPillarsNeighboursUp)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutTwoNeighbourPillar, DirectionForInstance.Up);
                }
                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutTwoPillarsNeighboursRight)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutTwoNeighbourPillar, DirectionForInstance.Right);
                }
                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutTwoPillarsNeighboursDown)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutTwoNeighbourPillar, DirectionForInstance.Down);
                }

                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutTwoPillarsNotNeighboursLeftUp)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutTwoNotNeighbourPillar, DirectionForInstance.Left);
                }
                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutTwoPillarsNotNeighboursUpRight)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutTwoNotNeighbourPillar, DirectionForInstance.Up);
                }
                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutTwoPillarsNotNeighboursRightDown)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutTwoNotNeighbourPillar, DirectionForInstance.Right);
                }
                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutTwoPillarsNotNeighboursDownLeft)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutTwoNotNeighbourPillar, DirectionForInstance.Down);
                }

                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutThreePillarsLeft)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutThreePillar, DirectionForInstance.Left);
                }
                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutThreePillarsUp)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutThreePillar, DirectionForInstance.Up);
                }
                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutThreePillarsRight)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutThreePillar, DirectionForInstance.Right);
                }
                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutThreePillarsDown)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutThreePillar, DirectionForInstance.Down);
                }

                if (map[i, j] == (int)CrossingDirection.AllSidesWithoutFourPillars)
                {
                    Createanenvironmentinstance(CeilingCrossingWithoutFourPillars);
                }
            }
        }
        //for (int i = 1; i < map.GetUpperBound(0); i++)
        //{
        //    for (int j = i % 2 + 1; j < map.GetUpperBound(1); j += 2)
        //    {
        //        if (map[i, j] == 10 || map[i, j] == 11)
        //        {
        //            //Debug.Log(i + " " + j);
        //            if (i % 2 == 0)
        //            {
        //                //Debug.Log(111);
        //                coordinates = new Vector3(0.7f + j * 1f, 2f, (map.GetUpperBound(0) - i) * 1f);
        //                object_being_created = Instantiate(DoorLattice);
        //                if (i < 5)
        //                {
        //                    if (j < 5)
        //                    {
        //                        //object_being_created.GetComponent<Locker_Unlocker>().pasword = "1";
        //                    }
        //                    else
        //                    {
        //                        //object_being_created.GetComponent<Locker_Unlocker>().pasword = "2";
        //                    }
        //                    //object_being_created.GetComponent<Locker_Unlocker>().is_locked = true;
        //                }
        //                else
        //                {
        //                    //object_being_created.GetComponent<Locker_Unlocker>().is_locked = false;
        //                }
        //                object_being_created.transform.position = coordinates;
        //                object_being_created.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        //            }
        //            else
        //            {
        //                coordinates = new Vector3(j * 1f, 2f, (map.GetUpperBound(0) - i) * 1f - 0.65f);
        //                object_being_created = Instantiate(DoorLattice);
        //                if (i < 5)
        //                {
        //                    if (j < 5)
        //                    {
        //                        //object_being_created.GetComponent<Locker_Unlocker>().pasword = "1";
        //                    }
        //                    else
        //                    {
        //                        //object_being_created.GetComponent<Locker_Unlocker>().pasword = "2";
        //                    }
        //                    //object_being_created.GetComponent<Locker_Unlocker>().is_locked = true;
        //                }
        //                else
        //                {
        //                    //object_being_created.GetComponent<Locker_Unlocker>().is_locked = false;
        //                }
        //                object_being_created.transform.position = coordinates;
        //                object_being_created.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);
        //            }
        //        }
        //    }
        //}
        //coordinates = new Vector3(1f, 2f, 1f);
        //coordinates = new Vector3(1.1f, 2f, 1f);
        //object_being_created = GameObject.FindGameObjectWithTag("Player");
        //object_being_created.transform.position = coordinates;
        //object_being_created.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);

        //coordinates = new Vector3(map_main.GetUpperBound(1) * 1f - 3, 3.5f, map_main.GetUpperBound(0) * 1f - 1);
        //object_being_created = GameObject.Find("Enemy");
        //object_being_created.transform.position = coordinates;
        //object_being_created.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    private void Createanenvironmentinstance(GameObject prefab, DirectionForInstance direction = DirectionForInstance.Left)
    {
        object_being_created = Instantiate(prefab, Parent.transform);
        object_being_created.transform.position = coordinates;
        object_being_created.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (float)direction, transform.eulerAngles.z);
    }

    private void Removal_Of_Invironment()
    {
        for (int i = Parent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(Parent.transform.GetChild(i).gameObject);
        }
    }

    private void Map_Saver()
    {
        string map = "";
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                map += map_main[i, j];
                map += ",";
            }
            map += ";";
        }
        PlayerPrefs.SetString("Map", map);
    }

    private void Map_Load()
    {
        string map = PlayerPrefs.GetString("Map");
        int i = 0;
        int j = 0;
        string number = "";
        foreach (char el in map)
        {
            if (el == ',' || el == ';')
            {
                if (el == ',')
                {
                    //map_main[j, i] = int.Parse(number, System.Globalization.NumberStyles.Integer);
                    map_main[j, i] = int.Parse(number);
                    i++;
                }
                if (el == ';')
                {
                    i = 0;
                    j++;
                }
                number = "";
            }
            else
            {
                number += el;
            }
        }
    }
}
