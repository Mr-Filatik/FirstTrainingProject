using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private GameObject _spawnPlaceMenu;

    private bool _isMenu = false;
    private static Vector3 _spawnPositionInLevel;
    private static Vector3 _spawnRotateInLevel;
    private DateTime? _startTime = null;

    private static GameController _instance;

    public static void SetNewSpawnPlaceInLevel(Vector3 spawnPositionInLevel)
    {
        _spawnPositionInLevel = spawnPositionInLevel;
        _spawnRotateInLevel = Vector3.up * 90;
    }

    public static void EndGame()
    {
        _spawnPositionInLevel = _instance._playerController.gameObject.transform.position;
        _spawnRotateInLevel = _instance._playerController.gameObject.transform.eulerAngles;

        _instance._playerController.gameObject.transform.position = _instance._spawnPlaceMenu.transform.position;
        _instance._playerController.gameObject.transform.eulerAngles = Vector3.zero;

        _instance._isMenu = true;
    }

    private void Awake()
    {
        if (_playerController == null) throw new System.Exception($"PlayerController not set!");
        if (_spawnPlaceMenu == null) throw new System.Exception($"SpawnPlaceMenu not set!");

        _instance = this;
    }

    void Start()
    {
        _playerController.gameObject.transform.position = _spawnPlaceMenu.transform.position;
        _isMenu = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isMenu)
            {
                _playerController.gameObject.transform.position = _spawnPositionInLevel;
                _playerController.gameObject.transform.eulerAngles = _spawnRotateInLevel;
            }
            else
            {
                _spawnPositionInLevel = _playerController.gameObject.transform.position;
                _spawnRotateInLevel = _playerController.gameObject.transform.eulerAngles;

                _playerController.gameObject.transform.position = _spawnPlaceMenu.transform.position;
                _playerController.gameObject.transform.eulerAngles = Vector3.zero;
            }
            _isMenu = !_isMenu;
        }
    }
}
