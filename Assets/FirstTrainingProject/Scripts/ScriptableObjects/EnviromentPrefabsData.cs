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
        private float _cellSize = 2F;

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

        [Header("Room prefabs")]

        [SerializeField]
        private GameObject _room1x1;
        [SerializeField]
        private GameObject _room2x2Left;
        [SerializeField]
        private GameObject _room2x2Right;
        [SerializeField]
        private GameObject _room3x2Left;
        [SerializeField]
        private GameObject _room3x2Center;
        [SerializeField]
        private GameObject _room3x2Right;

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

        public GameObject Room1x1 => _room1x1;

        public GameObject Room2x2Left => _room2x2Left;

        public GameObject Room2x2Right => _room2x2Right;

        public GameObject Room3x2Left => _room3x2Left;

        public GameObject Room3x2Center => _room3x2Center;

        public GameObject Room3x2Right => _room3x2Right;

        #endregion
    }
}