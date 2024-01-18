using System;
using UnityEngine;

namespace FirstTrainingProject
{
    [CreateAssetMenu(fileName = "EnviromentPrefabsData", menuName = "ScriptableObjects/EnviromentPrefabsData", order = 2)]
    public class EnviromentPrefabsData : ScriptableObject
    {
        #region Serialize Field

        [Header("Values")]

        [SerializeField]
        private float _cellSize; //2

        [Header("Enviroment prefabs")]

        [Header("Cell prefabs")]

        [SerializeField]
        private GameObject _startCell;
        [SerializeField]
        private GameObject _endCell;
        [SerializeField]
        private GameObject _deadEndCell;
        [SerializeField]
        private GameObject _straightCell;
        [SerializeField]
        private GameObject _cornerCell;
        [SerializeField]
        private GameObject _cornerCellWithoutPillar;
        [SerializeField]
        private GameObject _branchCell;
        [SerializeField]
        private GameObject _branchCellWithoutLeftPillar;
        [SerializeField]
        private GameObject _branchCellWithoutRightPillar;
        [SerializeField]
        private GameObject _branchCellWithoutBothPillars;
        [SerializeField]
        private GameObject _crossCell;
        [SerializeField]
        private GameObject _crossCellWithoutOnePillar;
        [SerializeField]
        private GameObject _crossCellWithoutTwoNeighbourPillars;
        [SerializeField]
        private GameObject _crossCellWithoutTwoNotNeighbourPillars;
        [SerializeField]
        private GameObject _crossCellWithoutThreePillars;
        [SerializeField]
        private GameObject _crossCellWithoutFourPillars;

        [Header("Other prefabs")]
        [SerializeField]
        private GameObject _floor;

        #endregion

        #region Properties

        /// <summary>
        /// Cell size
        /// </summary>
        /// <remarks> Размер ячейки </remarks>
        public float CellSize => _cellSize;

        /// <summary>
        /// Start cell
        /// </summary>
        /// <remarks> Стартовая ячейка </remarks>
        public GameObject StartCell => _startCell;

        /// <summary>
        /// End cell
        /// </summary>
        /// <remarks> Конечная ячейка </remarks>
        public GameObject EndCell => _endCell;

        /// <summary>
        /// Dead end cell
        /// </summary>
        /// <remarks> Тупиковая ячейка </remarks>
        public GameObject DeadEndCell => _deadEndCell;

        /// <summary>
        /// Straight cell
        /// </summary>
        /// <remarks> Прямая ячейка </remarks>
        public GameObject StraightCell => _straightCell;

        /// <summary>
        /// Corner cell
        /// </summary>
        /// <remarks> Угловая ячейка </remarks>
        public GameObject CornerCell => _cornerCell;

        /// <summary>
        /// Corner cell without pillar
        /// </summary>
        /// <remarks> Угловая ячейка без опоры </remarks>
        public GameObject CornerCellWithoutPillar => _cornerCellWithoutPillar;

        /// <summary>
        /// Branch cell
        /// </summary>
        /// <remarks> Ветвистая ячейка </remarks>
        public GameObject BranchCell => _branchCell;

        /// <summary>
        /// Branch cell without left pillar
        /// </summary>
        /// <remarks> Ветвистая ячейка без левой опоры </remarks>
        public GameObject BranchCellWithoutLeftPillar => _branchCellWithoutLeftPillar;

        /// <summary>
        /// Branch cell without right pillar
        /// </summary>
        /// <remarks> Ветвистая ячейка без правой опоры </remarks>
        public GameObject BranchCellWithoutRightPillar => _branchCellWithoutRightPillar;

        /// <summary>
        /// Branch cell without both pillars
        /// </summary>
        /// <remarks> Ветвистая ячейка без обоих опор </remarks>
        public GameObject BranchCellWithoutBothPillars => _branchCellWithoutBothPillars;

        /// <summary>
        /// Cross cell
        /// </summary>
        /// <remarks> Перекрёстная ячейка </remarks>
        public GameObject CrossCell => _crossCell;

        /// <summary>
        /// Cross cell without one pillars
        /// </summary>
        /// <remarks> Перекрёстная ячейка без одной опоры </remarks>
        public GameObject CrossCellWithoutOnePillar => _crossCellWithoutOnePillar;

        /// <summary>
        /// Cross cell without two neighbour pillars
        /// </summary>
        /// <remarks> Перекрёстная ячейка двух соседних опор </remarks>
        public GameObject CrossCellWithoutTwoNeighbourPillars => _crossCellWithoutTwoNeighbourPillars;

        /// <summary>
        /// Cross cell without two not neighbour pillars
        /// </summary>
        /// <remarks> Перекрёстная ячейка без двух противоположных опор </remarks>
        public GameObject CrossCellWithoutTwoNotNeighbourPillars => _crossCellWithoutTwoNotNeighbourPillars;

        /// <summary>
        /// Cross cell without three pillars
        /// </summary>
        /// <remarks> Перекрёстная ячейка без трёх опор </remarks>
        public GameObject CrossCellWithoutThreePillars => _crossCellWithoutThreePillars;

        /// <summary>
        /// Cross cell without four pillars
        /// </summary>
        /// <remarks> Перекрёстная ячейка без четырёх опор </remarks>
        public GameObject CrossCellWithoutFourPillars => _crossCellWithoutFourPillars;

        /// <summary>
        /// Froor
        /// </summary>
        /// /// <remarks> Пол </remarks>
        public GameObject Floor => _floor;

        #endregion
    }
}