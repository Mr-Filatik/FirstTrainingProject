using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FirstTrainingProject
{
    public class GameController : MonoBehaviour
    {
        private PlayerController _playerController;

        [SerializeField]
        private ApplicationManager _applicationManager;

        [SerializeField]
        private GameObject _spawnPlaceMenu;

        [SerializeField]
        private TMP_Text _screen;

        private bool _isMenu = false;
        private Vector3 _spawnPositionInLevel;
        private Vector3 _spawnRotateInLevel;
        //private DateTime? _startTime = null;
        private int _count = 0;

        //private static GameController _instance;

        public void SetNewSpawnPlaceInLevel(Vector3 spawnPositionInLevel)
        {
            _spawnPositionInLevel = spawnPositionInLevel;
            _spawnRotateInLevel = Vector3.up * 90;
        }

        public void Init()
        {

        }

        public void GameStart()
        {

        }

        public void GamePause()
        {

        }

        public void GameContinue()
        {

        }

        public void GameEnd()
        {

        }

        public void EndGame()
        {
            _spawnPositionInLevel = _playerController.gameObject.transform.position;
            _spawnRotateInLevel = _playerController.gameObject.transform.eulerAngles;

            _playerController.gameObject.transform.position = _spawnPlaceMenu.transform.position;
            _playerController.gameObject.transform.eulerAngles = Vector3.zero;

            _applicationManager.MapController.MyStart();

            SetDataOnScreen(++_count, DateTime.MinValue);

            _isMenu = true;
        }

        private void SetDataOnScreen(int count, DateTime dateTime)
        {
            _screen.text = $"Это комната - меню игры.\r\nЗдесь время игры останавливается.\r\nНажмите Space скрыть/показать курсор \r\nНажмите Escape для переход на уровень \r\n(и для возвращения обратно).\r\n\r\nКарта уже создана, пройдено {count} раз.\r\n\r\nЗатрачено времени {dateTime.Minute} м. {dateTime.Second} с.";
        }

        #region Unity Medhods

        private void Awake()
        {
            if (_spawnPlaceMenu == null) throw new System.Exception($"SpawnPlaceMenu not set!");
            if (_screen == null) throw new System.Exception($"Screen not set!");

            _applicationManager.GameController = this;
        }

        private void Start()
        {
            _playerController = _applicationManager.PlayerController;

            _playerController.gameObject.transform.position = _spawnPlaceMenu.transform.position;
            _playerController.gameObject.transform.eulerAngles = Vector3.zero;

            _isMenu = true;
        }

        private bool _cursorVisible = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isMenu)
                {
                    _playerController.gameObject.transform.position = _spawnPositionInLevel;
                    _playerController.gameObject.transform.eulerAngles = _spawnRotateInLevel;
                    _isMenu = false;
                }
                else
                {
                    _spawnPositionInLevel = _playerController.gameObject.transform.position;
                    _spawnRotateInLevel = _playerController.gameObject.transform.eulerAngles;

                    _playerController.gameObject.transform.position = _spawnPlaceMenu.transform.position;
                    _playerController.gameObject.transform.eulerAngles = Vector3.zero;
                    _isMenu = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_cursorVisible)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    Debug.Log($"<color=green>Cursor Show</color>");
                }
                else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Debug.Log($"<color=red>Cursor Hide</color>");
                }
                _cursorVisible = !_cursorVisible;
            }
        }

        #endregion
    }
}