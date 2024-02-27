using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

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
        private float _rotateSpeed = 400;

        [SerializeField]
        private float _limitAngleTop = 30;

        [SerializeField]
        private float _limitAngleBottom = 60;

        [Header("Parts")]

        [SerializeField]
        private GameObject _head;

        #endregion

        #region Public Fields

        public event Action<float> EnduranceChanged;

        #endregion

        #region Private Fields

        private bool _isRun = false;
        private float _runAcceleration = 2F;

        private float _endurance = 0F;
        private float _enduranceMin = 0F;
        private float _enduranceMax = 1F;
        private float _enduranceFlowRate = 0.5F;
        private float _enduranceRecoveryRate = 0.15F;

        #endregion

        #region Public Properties



        #endregion

        #region Public Methods

        public void SetPosition(Vector3 position, Vector3 rotation)
        {
            gameObject.transform.position = position;
            gameObject.transform.eulerAngles = rotation;
        }

        public void GetCurrentPosition()
        {
            _applicationManager.GameController.SetNewSpawnPlaceInLevel(gameObject.transform.position, gameObject.transform.eulerAngles);
        }

        public void PlayerInitApplication()
        {
            gameObject.transform.eulerAngles = Vector3.zero;
            _head.transform.eulerAngles = Vector3.zero;
        }

        #endregion

        #region Unity Medhods

        private void Awake()
        {
            if (_head == null) throw new System.Exception($"Head not set!");
            if (_applicationManager == null) throw new System.Exception($"ApplicationManager not set!");

            _applicationManager.PlayerController = this;

            _applicationManager.ApplicationGameInited += PlayerInitApplication; // m.b remove
            _applicationManager.ApplicationGamePaused += GetCurrentPosition;
            //_applicationManager.ApplicationGameEnded += GetCurrentPosition;
        }

        private void OnDestroy()
        {
            _applicationManager.ApplicationGameInited -= PlayerInitApplication; // m.b remove
            _applicationManager.ApplicationGamePaused -= GetCurrentPosition;
            //_applicationManager.ApplicationGameEnded -= GetCurrentPosition;
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
            var acceleration = _isRun ? _runAcceleration : 1F;

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
        }

        #endregion
    }
}
