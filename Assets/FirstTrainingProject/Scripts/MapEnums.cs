using System;

namespace FirstTrainingProject
{
    public class MapEnums
    {

    }

    public class CellPoint
    {
        public int PosH { get; set; }
        public int PosW { get; set; }
    }

    public class CellPointWithDirection : CellPoint
    {
        public CellDirection Direction { get; set; }
    }

    public class RoomPoint : CellPoint
    {
        private int _sizeH;
        private int _sizeW;
        /// <summary>
        /// 1, 2, 3... cells
        /// </summary>
        public int SizeH { get { return (_sizeH - 1) * 2; } set { _sizeH = value; } }
        /// <summary>
        /// 1, 2, 3... cells
        /// </summary>
        public int SizeW { get { return (_sizeW - 1) * 2; } set { _sizeW = value; } }

        public CellDirection DoorDirection { get; set; }
        /// <summary>
        /// Clockwise distance
        /// </summary>
        public int DoorDistance { get; set; }
    }
}