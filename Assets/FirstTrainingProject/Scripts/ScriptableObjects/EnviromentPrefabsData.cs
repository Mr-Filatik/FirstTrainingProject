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
        /// <remarks> ������ ������ </remarks>
        public float CellSize => _cellSize;

        /// <summary>
        /// Start cell
        /// </summary>
        /// <remarks> ��������� ������ </remarks>
        public GameObject StartCell => _startCell;

        /// <summary>
        /// End cell
        /// </summary>
        /// <remarks> �������� ������ </remarks>
        public GameObject EndCell => _endCell;

        /// <summary>
        /// Dead end cell
        /// </summary>
        /// <remarks> ��������� ������ </remarks>
        public GameObject DeadEndCell => _deadEndCell;

        /// <summary>
        /// Straight cell
        /// </summary>
        /// <remarks> ������ ������ </remarks>
        public GameObject StraightCell => _straightCell;

        /// <summary>
        /// Corner cell
        /// </summary>
        /// <remarks> ������� ������ </remarks>
        public GameObject CornerCell => _cornerCell;

        /// <summary>
        /// Corner cell without pillar
        /// </summary>
        /// <remarks> ������� ������ ��� ����� </remarks>
        public GameObject CornerCellWithoutPillar => _cornerCellWithoutPillar;

        /// <summary>
        /// Branch cell
        /// </summary>
        /// <remarks> ��������� ������ </remarks>
        public GameObject BranchCell => _branchCell;

        /// <summary>
        /// Branch cell without left pillar
        /// </summary>
        /// <remarks> ��������� ������ ��� ����� ����� </remarks>
        public GameObject BranchCellWithoutLeftPillar => _branchCellWithoutLeftPillar;

        /// <summary>
        /// Branch cell without right pillar
        /// </summary>
        /// <remarks> ��������� ������ ��� ������ ����� </remarks>
        public GameObject BranchCellWithoutRightPillar => _branchCellWithoutRightPillar;

        /// <summary>
        /// Branch cell without both pillars
        /// </summary>
        /// <remarks> ��������� ������ ��� ����� ���� </remarks>
        public GameObject BranchCellWithoutBothPillars => _branchCellWithoutBothPillars;

        /// <summary>
        /// Cross cell
        /// </summary>
        /// <remarks> ����������� ������ </remarks>
        public GameObject CrossCell => _crossCell;

        /// <summary>
        /// Cross cell without one pillars
        /// </summary>
        /// <remarks> ����������� ������ ��� ����� ����� </remarks>
        public GameObject CrossCellWithoutOnePillar => _crossCellWithoutOnePillar;

        /// <summary>
        /// Cross cell without two neighbour pillars
        /// </summary>
        /// <remarks> ����������� ������ ���� �������� ���� </remarks>
        public GameObject CrossCellWithoutTwoNeighbourPillars => _crossCellWithoutTwoNeighbourPillars;

        /// <summary>
        /// Cross cell without two not neighbour pillars
        /// </summary>
        /// <remarks> ����������� ������ ��� ���� ��������������� ���� </remarks>
        public GameObject CrossCellWithoutTwoNotNeighbourPillars => _crossCellWithoutTwoNotNeighbourPillars;

        /// <summary>
        /// Cross cell without three pillars
        /// </summary>
        /// <remarks> ����������� ������ ��� ��� ���� </remarks>
        public GameObject CrossCellWithoutThreePillars => _crossCellWithoutThreePillars;

        /// <summary>
        /// Cross cell without four pillars
        /// </summary>
        /// <remarks> ����������� ������ ��� ������ ���� </remarks>
        public GameObject CrossCellWithoutFourPillars => _crossCellWithoutFourPillars;

        /// <summary>
        /// Froor
        /// </summary>
        /// /// <remarks> ��� </remarks>
        public GameObject Floor => _floor;

        #endregion
    }
}