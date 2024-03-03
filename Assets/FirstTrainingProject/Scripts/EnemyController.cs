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

        [SerializeField]
        private TriggerController _enemyDestination;

        [Header("Values")]

        [SerializeField]
        private float _movementSpeed = 1.5F;

        [SerializeField]
        private float _rotateSpeed = 400F;

        [Header("Parts")]

        [SerializeField]
        private GameObject _colliderForTriggers;

        [SerializeField]
        private Transform _rayStartPoint;

        #endregion

        #region Private Fields

        //private Transform _target;
        private Transform _player;
        //private bool _isFirstNoVisionPlayer = false;

        #endregion

        #region Public Methods

        public void SetPosition(Vector3 position, Vector3 rotation)
        {
            gameObject.transform.position = position;
            gameObject.transform.eulerAngles = rotation;
        }

        public void SetTarget(Transform target)
        {
            //_target = target;
        }

        public void SetPlayerPosition()
        {
            if (true)
            {

            }
        }

        #endregion

        #region Unity Medhods

        private void Awake()
        {
            if (_applicationManager == null) throw new MissingFieldException($"ApplicationManager not set!");
            if (_agent == null) throw new MissingFieldException($"Agent not set!");
            if (_enemyDestination == null) throw new MissingFieldException($"EnemyDestination not set!");
            if (_rayStartPoint == null) throw new MissingFieldException($"RayStartPoint not set!");

            _applicationManager.EnemyController = this;

            _applicationManager.ApplicationGameStarted += GameStart;
            _applicationManager.ApplicationGamePaused += GamePause;
            _applicationManager.ApplicationGameContinued += GameContinue;
            _applicationManager.ApplicationGameEnded += GameEnd;

            _enemyDestination.SetAction(ArrivalInDestination);
        }

        private void OnDestroy()
        {
            _applicationManager.ApplicationGameStarted -= GameStart;
            _applicationManager.ApplicationGamePaused -= GamePause;
            _applicationManager.ApplicationGameContinued -= GameContinue;
            _applicationManager.ApplicationGameEnded -= GameEnd;
        }

        private void Start()
        {
            _player = _applicationManager.PlayerController.transform;
        }

        private void Update()
        {
            //!Not call every frame
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(_rayStartPoint.position, _player.position - _rayStartPoint.position, out hit, 10F/*, layerMask*/))
            {
                if (hit.collider.transform == _player)
                {
                    Debug.DrawRay(_rayStartPoint.position, (_player.position - _rayStartPoint.position).normalized * hit.distance, Color.green);
                    ArrivalInDestination(_colliderForTriggers, _player.position);
                    //_isFirstNoVisionPlayer = true;
                }
                else
                {
                    //if (_isFirstNoVisionPlayer)
                    //{
                    //    ArrivalInDestination(_colliderForTriggers);
                    //    _isFirstNoVisionPlayer = false;
                    //    Debug.Log("Player NOT DETECTED");
                    //}
                    //! Fix the moment when the monster loses sight of the player, this point can be closed in the room, then he will never reach it, you need to watch that he does not move for some time and make a new position
                    Debug.DrawRay(transform.position, (_player.position - _rayStartPoint.position).normalized * hit.distance, Color.white);
                }
            }
            //else
            //{
            //    Debug.DrawRay(transform.position, (_player.position - transform.position).normalized * 10F, Color.white);
            //    Debug.Log("Did not Hit");
            //}
        }

        #endregion

        #region Private Methods

        private void GameStart()
        {
            _agent.enabled = true;
            ArrivalInDestination(_colliderForTriggers);
        }

        private void GamePause()
        {
            _agent.enabled = false;
        }

        private void GameContinue()
        {
            _agent.enabled = true;
            ArrivalInDestination(_colliderForTriggers, _enemyDestination.gameObject.transform.position);
        }

        private void GameEnd(bool isWin)
        {
            _agent.enabled = false;
            SetPosition(Vector3.zero, Vector3.zero);
        }

        private void ArrivalInDestination(GameObject obj)
        {
            ArrivalInDestination(obj, _applicationManager.MapController.GetRandomPosition(false));
        }

        private void ArrivalInDestination(GameObject obj, Vector3 position)
        {
            if (obj != null && obj == _colliderForTriggers)
            {
                _enemyDestination.gameObject.transform.position = position;
                bool agentEnabled = _agent.enabled;
                _agent.enabled = true;
                _agent.SetDestination(_enemyDestination.gameObject.transform.position);
                _agent.enabled = agentEnabled;
            }
        }

        #endregion
    }
}
