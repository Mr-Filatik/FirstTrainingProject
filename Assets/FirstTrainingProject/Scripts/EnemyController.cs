using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace FirstTrainingProject
{
    public class EnemyController : MonoBehaviour
    {
        #region Serialize Fields

        [Header("Parts")]

        [SerializeField]
        private ApplicationManager _applicationManager;

        [SerializeField]
        private NavMeshAgent _agent;

        [Header("Values")]

        [SerializeField]
        private float _movementSpeed; // 1.5

        [SerializeField]
        private float _rotateSpeed; // 400

        #endregion

        #region Private Fields

        private Transform _target;

        #endregion

        #region Public Methods

        public void SetPosition(Vector3 position, Vector3 rotation)
        {
            gameObject.transform.position = position;
            gameObject.transform.eulerAngles = rotation;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void GamePause()
        {
            _agent.enabled = false;
        }

        public void GameContinue()
        {
            _agent.enabled = true;
        }

        public void GameEnd(bool isWin)
        {
            _agent.enabled = false;
            SetPosition(Vector3.zero, Vector3.zero);
        }

        #endregion

        #region Unity Medhods

        private void Awake()
        {
            if (_applicationManager == null) throw new System.Exception($"ApplicationManager not set!");
            if (_agent == null) throw new System.Exception($"Agent not set!");

            _applicationManager.EnemyController = this;

            _applicationManager.ApplicationGamePaused += GamePause;
            _applicationManager.ApplicationGameContinued += GameContinue;
            _applicationManager.ApplicationGameStarted += GameContinue;
            _applicationManager.ApplicationGameEnded += GameEnd;
        }

        private void OnDestroy()
        {
            _applicationManager.ApplicationGamePaused -= GamePause;
            _applicationManager.ApplicationGameContinued -= GameContinue;
            _applicationManager.ApplicationGameStarted -= GameContinue;
            _applicationManager.ApplicationGameEnded -= GameEnd;
        }

        private void Update()
        {
            if (_agent.enabled)
            {
                _agent?.SetDestination(_target.position); // SetDestination in update or not
            }
        }

        #endregion
    }
}
