using System.Collections.Generic;

namespace FirstTrainingProject
{
    /// <summary>
    /// Map class for labyrinths
    /// </summary>
    public class MapType
    {
        /// <summary>
        /// Map
        /// </summary>
        /// <remarks>
        /// Example data (1 - cell, 0 - wall, _ - not used):
        /// _ 0 _ 0 _ 0 _ 0 _ 0 _
        /// 0 1 0 1 0 1 0 1 0 1 0
        /// _ 0 _ 0 _ 0 _ 0 _ 0 _
        /// 0 1 0 1 0 1 0 1 0 1 0
        /// _ 0 _ 0 _ 0 _ 0 _ 0 _
        /// 0 1 0 1 0 1 0 1 0 1 0
        /// _ 0 _ 0 _ 0 _ 0 _ 0 _
        /// 0 1 0 1 0 1 0 1 0 1 0
        /// _ 0 _ 0 _ 0 _ 0 _ 0 _
        /// 0 1 0 1 0 1 0 1 0 1 0
        /// _ 0 _ 0 _ 0 _ 0 _ 0 _
        /// </remarks>
        public int[,] Map { get; set; }

        /// <summary>
        /// Map height
        /// </summary>
        public int Height => Map.GetLength(0);

        /// <summary>
        /// Map width
        /// </summary>
        public int Width => Map.GetLength(1);

        /// <summary>
        /// Start point
        /// </summary>
        public CellPointWithDirection StartPoint { get; set; }

        /// <summary>
        /// End point
        /// </summary>
        public CellPointWithDirection EndPoint { get; set; }

        /// <summary>
        /// List of interior rooms
        /// </summary>
        public List<RoomPoint> InternalRooms { get; set; }

        /// <summary>
        /// Minimum permissible map height
        /// </summary>
        public static int MinHeight => 11;

        /// <summary>
        /// Minimum permissible map width
        /// </summary>
        public static int MinWidth => 11;

        /// <summary>
        /// Constructor for creating a map of a given size with default parameters
        /// </summary>
        /// <param name="height"> Height (only odd numbers: 11, 13, 15...) </param>
        /// <param name="width"> Width (only odd numbers: 11, 13, 15...) </param>
        public MapType(int height, int width)
        {
            if (height < MinHeight) height = MinHeight;
            if (width < MinWidth) width = MinWidth;

            Map = new int[height, width];

            for (int h = 0; h < Height; h++)
            {
                for (int w = 0; w < Width; w++)
                {
                    Map[h, w] = (int)CellType.Empty;
                }
            }

            StartPoint = new CellPointWithDirection() { PosH = 1, PosW = 1, Direction = CellDirection.Right };
            Map[StartPoint.PosH, StartPoint.PosW] = (int)CellType.Start;

            EndPoint = new CellPointWithDirection() { PosH = Height - 2, PosW = Width - 2, Direction = CellDirection.Left };
            Map[EndPoint.PosH, EndPoint.PosW] = (int)CellType.End;

            InternalRooms = new List<RoomPoint>();
        }

        /// <summary>
        /// Indexer for getting and changing cell type
        /// </summary>
        /// <param name="heightIndex"> Height index </param>
        /// <param name="widthIndex"> Width index </param>
        /// <returns> Cell type as <see cref="CellType"/> </returns>
        public int this[int heightIndex, int widthIndex]
        {
            get => Map[heightIndex, widthIndex];
            set => Map[heightIndex, widthIndex] = value;
        }

        /// <summary>
        /// Checking if a point can be inside the map
        /// </summary>
        /// <param name="cell"> Cell with direction <see cref="CellPointWithDirection"> </param>
        /// <returns> <see langword="true"/> if the cell is inside the map, otherwise <see langword="false"/> </returns>
        public bool CellInMap(CellPointWithDirection cell)
        {
            if (cell.Direction == CellDirection.Left)
            {
                return CellInMap((CellPoint)cell) && CellInMap(cell.PosH, cell.PosW - 2);
            }
            if (cell.Direction == CellDirection.Top)
            {
                return CellInMap((CellPoint)cell) && CellInMap(cell.PosH + 2, cell.PosW);
            }
            if (cell.Direction == CellDirection.Right)
            {
                return CellInMap((CellPoint)cell) && CellInMap(cell.PosH, cell.PosW + 2);
            }
            if (cell.Direction == CellDirection.Bottom)
            {
                return CellInMap((CellPoint)cell) && CellInMap(cell.PosH - 2, cell.PosW);
            }
            return false;
        }

        /// <summary>
        /// Checking if a point can be inside the map
        /// </summary>
        /// <param name="cell"> Cell as <see cref="CellPoint"> </param>
        /// <returns> <see langword="true"/> if the cell is inside the map, otherwise <see langword="false"/> </returns>
        public bool CellInMap(CellPoint cell)
        {
            return CellInMap(cell.PosH, cell.PosW);
        }

        /// <summary>
        /// Checking if a point can be inside the map
        /// </summary>
        /// <param name="heightCoordinate"> Height coordinate </param>
        /// <param name="widthCoordinate"> Width coordinate </param>
        /// <returns> <see langword="true"/> if the cell is inside the map, otherwise <see langword="false"/> </returns>
        public bool CellInMap(int heightCoordinate, int widthCoordinate)
        {
            return heightCoordinate % 2 == 1 && widthCoordinate % 2 == 1 && heightCoordinate > 0 && heightCoordinate < Height && widthCoordinate > 0 && widthCoordinate < Width;
        }
    }
}
