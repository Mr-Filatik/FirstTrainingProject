using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace FirstTrainingProject
{
    public class GameController : MonoBehaviour
    {
        #region Serialize Fields

        [Header("Main")]

        [SerializeField]
        private ApplicationManager _applicationManager;

        [Header("Elements")]

        [SerializeField]
        private GameObject _spawnPlaceMenu;

        [SerializeField]
        private TMP_Text _screen;

        #endregion

        #region Private Fields

        private bool _isPause = false;
        private Vector3 _spawnPositionInMenu => _spawnPlaceMenu.transform.position;
        private Vector3 _spawnRotateInMenu => _spawnPlaceMenu.transform.eulerAngles;
        private Vector3 _spawnPositionInLevel;
        private Vector3 _spawnRotateInLevel;
        private DateTime _time = DateTime.MinValue;
        private DateTime _timeTmp = DateTime.MinValue;
        private DateTime _bestTime = DateTime.MaxValue;
        private int _countWin = 0;
        private int _countLose = 0;

        #endregion

        #region Public Methods

        public void SetNewSpawnPlaceInLevel(Vector3 position, Vector3 rotation)
        {
            _spawnPositionInLevel = position;
            _spawnRotateInLevel = rotation;
        }

        #endregion

        #region Unity Medhods

        private void Awake()
        {
            if (_spawnPlaceMenu == null) throw new System.Exception($"SpawnPlaceMenu not set!");
            if (_screen == null) throw new System.Exception($"Screen not set!");
            if (_applicationManager == null) throw new System.Exception($"ApplicationManager not set!");

            _applicationManager.GameController = this;

            _applicationManager.ApplicationGameStarted += GameStart;
            _applicationManager.ApplicationGamePaused += GamePause;
            _applicationManager.ApplicationGameContinued += GameContinue;
            _applicationManager.ApplicationGameEnded += GameEnd;
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
            _applicationManager.PlayerController?.SetPosition(_spawnPositionInMenu, _spawnRotateInMenu);

            _isPause = true;

            _screen.text = $"This room is the game's menu.\r\nTo start the game, go to the green zone.\r\nPress Space to hide/show the cursor.\r\nPress Escape to exit here (to the menu).\r\n\r\nMap already created, passed {_countWin + _countLose} times.\r\nTime spent {0} m. {0} s. (Best time {0} m. {0} s.).";

            _applicationManager.ApplicationGameInit(); //запускам настройку и создание карты, внутри метода создания вызываем старт игры
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_isPause)
                {
                    _applicationManager.ApplicationGamePause();
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Cursor.visible = !Cursor.visible;
                Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
                Debug.Log($"Cursor {Cursor.lockState}.");
            }
        }

        #endregion

        #region Private Methods

        //private void GameInit()
        //{
        //    _applicationManager.PlayerController.SetPosition(_spawnPositionInMenu, _spawnRotateInMenu);
        //}

        private void GameStart()
        {
            _isPause = false;

            _applicationManager.PlayerController.SetPosition(_spawnPositionInLevel, _spawnRotateInLevel);

            _time = DateTime.MinValue;
            _timeTmp = DateTime.Now;
        }

        private void GamePause()
        {
            _isPause = true;

            _applicationManager.PlayerController.SetPosition(_spawnPositionInMenu, _spawnRotateInMenu);

            DateTime now = DateTime.Now;
            var delta = now.AddHours(-_timeTmp.Hour).AddMinutes(-_timeTmp.Minute).AddSeconds(-_timeTmp.Second);
            _time = _time.AddHours(delta.Hour).AddMinutes(delta.Minute).AddSeconds(delta.Second);
        }

        private void GameContinue()
        {
            _isPause = false;

            _applicationManager.PlayerController.SetPosition(_spawnPositionInLevel, _spawnRotateInLevel);

            _timeTmp = DateTime.Now;
        }

        private void GameEnd(bool isWin)
        {
            _isPause = true;

            _applicationManager.PlayerController.SetPosition(_spawnPositionInMenu, _spawnRotateInMenu);

            _applicationManager.ApplicationGameInit();

            DateTime now = DateTime.Now;
            var delta = now.AddHours(-_timeTmp.Hour).AddMinutes(-_timeTmp.Minute).AddSeconds(-_timeTmp.Second);
            _time = _time.AddHours(delta.Hour).AddMinutes(delta.Minute).AddSeconds(delta.Second);

            if (isWin)
            {
                if (_bestTime > _time)
                {
                    _bestTime = _time;
                }

                _countWin++;
                SetDataOnScreen(true);
            }
            else
            {
                _countLose++;
                SetDataOnScreen(false);
            }
        }

        private void SetDataOnScreen(bool isWin)
        {
            _screen.text = $"This room is the game's menu.\r\nTo start the game, go to the green zone.\r\nPress Space to hide/show the cursor.\r\nPress Escape to exit here (to the menu).\r\n\r\nMap already created, passed {_countWin + _countLose} times.\r\nTime spent {_time.Minute} m. {_time.Second} s. (Best time {_bestTime.Minute} m. {_bestTime.Second} s.).\r\n{(isWin ? "Yep :) !YOU IS WIN! (: peY" : "Sorry :( !YOU IS DEAD! ): yrroS")}\r\nCount win {_countWin}, count lose {_countLose}.";
        }

        #endregion
    }
}