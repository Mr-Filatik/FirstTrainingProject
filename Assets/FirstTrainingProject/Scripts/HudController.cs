using FirstTrainingProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class HudController : MonoBehaviour
{
    #region Serialize Fields

    [Header("Main")]

    [SerializeField]
    private ApplicationManager _applicationManager;

    [Header("Panels")]

    [SerializeField]
    private GameObject _menuPanel;

    [SerializeField]
    private GameObject _gamePanel;

    [Header("Elements")]

    [SerializeField]
    private GameObject _staminaBar;

    #endregion

    #region Private Fields

    private PlayerController _playerController;
    private RectTransform _rectTransform;

    #endregion

    #region Unity Medhods

    private void Awake()
    {
        if (_menuPanel == null) throw new System.Exception($"MenuPanel not set!");
        if (_gamePanel == null) throw new System.Exception($"GamePanel not set!");
        if (_staminaBar == null) throw new System.Exception($"StaminaBar not set!");
        if (_applicationManager == null) throw new System.Exception($"ApplicationManager not set!");

        _rectTransform = _staminaBar.transform as RectTransform;
        _menuPanel.SetActive(false);
        _gamePanel.SetActive(false);

        _applicationManager.ApplicationGameStarted += GameRun;
        _applicationManager.ApplicationGamePaused += GamePause;
        _applicationManager.ApplicationGameContinued += GameRun;
        _applicationManager.ApplicationGameEnded += GameStop;
    }

    private void Start()
    {
        if (_playerController == null)
        {
            _playerController = _applicationManager.PlayerController;
        }
        _playerController.EnduranceChanged += ChangeStaminaBar;
    }

    private void OnDestroy()
    {
        if (_playerController != null)
        {
            _playerController.EnduranceChanged -= ChangeStaminaBar;
        }
        _applicationManager.ApplicationGameStarted -= GameRun;
        _applicationManager.ApplicationGamePaused -= GamePause;
        _applicationManager.ApplicationGameContinued -= GameRun;
        _applicationManager.ApplicationGameEnded -= GameStop;
    }

    #endregion

    #region Private Methods

    private void ChangeStaminaBar(float value)
    {
        //if (_rectTransform == null)
        //{
        //    _rectTransform = _staminaBar.transform as RectTransform;
        //}
        _rectTransform.localScale = new Vector3(value, 1.0f, 1.0f);
    }

    private void GameRun()
    {
        _menuPanel.SetActive(false);
        _gamePanel.SetActive(true);
    }

    private void GamePause()
    {
        _gamePanel.SetActive(false);
        _menuPanel.SetActive(true);
    }

    private void GameStop(bool isWin)
    {
        _gamePanel.SetActive(false);
        _menuPanel.SetActive(true);
    }

    #endregion
}
