using System;
using UnityEngine;
using UnityEngine.AI;

namespace FirstTrainingProject
{
    public class EnemyController : MonoBehaviour
    {
        #region Serialize Fields

        [Header("Main")]

        [SerializeField]
        private ApplicationManager _applicationManager;

        [Header("Agents")]

        [SerializeField]
        private NavMeshAgent _agent;

        [Header("Values")]

        [SerializeField]
        private float _movementSpeed = 1.5F;

        [SerializeField]
        private float _rotateSpeed = 400F;

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

        #endregion

        #region Unity Medhods

        private void Awake()
        {
            if (_applicationManager == null) throw new MissingFieldException($"ApplicationManager not set!");
            if (_agent == null) throw new MissingFieldException($"Agent not set!");

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
                //!don't take every frame
                _agent.SetDestination(_target.position); // SetDestination in update or not
            }
        }

        #endregion

        #region Private Methods

        private void GamePause()
        {
            _agent.enabled = false;
        }

        private void GameContinue()
        {
            _agent.enabled = true;
        }

        private void GameEnd(bool isWin)
        {
            _agent.enabled = false;
            SetPosition(Vector3.zero, Vector3.zero);
        }

        #endregion
    }
}
