using System;
using UnityEngine;

namespace FirstTrainingProject
{
    public class PlayerController : MonoBehaviour
    {
        #region Serialize Fields

        [Header("Main")]

        [SerializeField]
        private ApplicationManager _applicationManager;

        [Header("Values")]

        [SerializeField]
        private float _movementSpeed = 1.5F;

        [SerializeField]
        private float _rotateSpeed = 400F;

        [SerializeField]
        private float _limitAngleTop = 30F;

        [SerializeField]
        private float _limitAngleBottom = 60F;

        [Header("Parts")]

        [SerializeField]
        private GameObject _head;

        [SerializeField]
        private Camera _camera;

        #endregion

        #region Public Fields

        public event Action<float> EnduranceChanged;
        public event Action<bool> InteractionChanged;

        #endregion

        #region Private Fields

        private bool _isGame = false;

        private bool _isRun = false;
        private readonly float _runAcceleration = 2F;

        private float _endurance = 0F;
        private readonly float _enduranceMin = 0F;
        private readonly float _enduranceMax = 1F;
        private readonly float _enduranceFlowRate = 0.5F;
        private readonly float _enduranceRecoveryRate = 0.1F;

        private Vector3 _startRayPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        private float _actionDistance = 1F;
        private bool _lastInteraction = false;

        #endregion

        #region Public Methods

        public void SetPosition(Vector3 position, Vector3 rotation)
        {
            gameObject.transform.position = position;
            gameObject.transform.eulerAngles = rotation;
        }

        #endregion

        #region Unity Medhods

        private void Awake()
        {
            if (_head == null) throw new MissingFieldException($"Head not set!");
            if (_camera == null) throw new MissingFieldException($"Camera not set!");
            if (_applicationManager == null) throw new MissingFieldException($"ApplicationManager not set!");

            _applicationManager.PlayerController = this;

            _applicationManager.ApplicationGameInited += PlayerInitApplication; // m.b remove
            _applicationManager.ApplicationGamePaused += GetCurrentPosition;

            _applicationManager.ApplicationGameStarted += GameStart;
            _applicationManager.ApplicationGamePaused += GamePause;
            _applicationManager.ApplicationGameContinued += GameContinue;
            _applicationManager.ApplicationGameEnded += GameStop;
        }

        private void OnDestroy()
        {
            _applicationManager.ApplicationGameInited -= PlayerInitApplication; // m.b remove
            _applicationManager.ApplicationGamePaused -= GetCurrentPosition;

            _applicationManager.ApplicationGameStarted -= GameStart;
            _applicationManager.ApplicationGamePaused -= GamePause;
            _applicationManager.ApplicationGameContinued -= GameContinue;
            _applicationManager.ApplicationGameEnded -= GameStop;
        }

        private void Update()
        {
            if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && Input.GetAxis("Vertical") != 0)
            {
                _isRun = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                _isRun = false;
            }
            if (_isGame)
            {
                if (_isRun)
                {
                    _endurance -= _enduranceFlowRate * Time.deltaTime;
                    if (_endurance <= _enduranceMin)
                    {
                        _isRun = false;
                        _endurance = _enduranceMin;
                    }
                }
                else
                {
                    _endurance += _enduranceRecoveryRate * Time.deltaTime;
                    if (_endurance >= _enduranceMax)
                    {
                        _endurance = _enduranceMax;
                    }
                }
                EnduranceChanged?.Invoke(_endurance);
            }
            var acceleration = _isRun && Input.GetAxis("Vertical") > 0 ? _runAcceleration : 1F;

            transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * _movementSpeed * Time.deltaTime * acceleration);
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * _movementSpeed * Time.deltaTime);
            transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * _rotateSpeed * Time.deltaTime);
            _head.transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y") * _rotateSpeed * Time.deltaTime);

            var currentAngles = _head.transform.localEulerAngles;
            if (currentAngles.x > 180F && currentAngles.x < 360F - _limitAngleTop)
            {
                currentAngles.x = -_limitAngleTop;
            }
            if (currentAngles.x < 180F && currentAngles.x > _limitAngleBottom)
            {
                currentAngles.x = _limitAngleBottom;
            }
            _head.transform.localEulerAngles = currentAngles;

            
            bool interaction = false;
            Ray ray = _camera.ScreenPointToRay(_startRayPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var obj = hit.transform.gameObject.GetComponent<KnobController>();
                if (obj != null && hit.distance < _actionDistance)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        obj.KnobTrigger();
                    }
                    else
                    {
                        interaction = true;
                        InteractionChanged?.Invoke(true);
                    }
                }
            }
            if (_lastInteraction != interaction)
            {
                InteractionChanged?.Invoke(interaction);
                _lastInteraction = interaction;
            }
        }

        #endregion

        #region Private Methods

        private void PlayerInitApplication()
        {
            gameObject.transform.eulerAngles = Vector3.zero;
            _head.transform.eulerAngles = Vector3.zero;
        }

        private void GetCurrentPosition()
        {
            _applicationManager.GameController.SetNewSpawnPlaceInLevel(gameObject.transform.position, gameObject.transform.eulerAngles);
        }

        private void GameStart()
        {
            _isGame = true;
            _endurance = _enduranceMin;
        }

        private void GamePause()
        {
            _isGame = false;
        }

        private void GameContinue()
        {
            _isGame = true;
        }

        private void GameStop(bool isWin)
        {
            _isGame = false;
        }

        #endregion
    }
}
