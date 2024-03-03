using FirstTrainingProject;
using System;
using UnityEngine;

namespace FirstTrainingProject
{
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

        [SerializeField]
        private GameObject _interactionPanel;

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
            if (_menuPanel == null) throw new MissingFieldException($"MenuPanel not set!");
            if (_gamePanel == null) throw new MissingFieldException($"GamePanel not set!");
            if (_interactionPanel == null) throw new MissingFieldException($"InteractionPanel not set!");
            if (_staminaBar == null) throw new MissingFieldException($"StaminaBar not set!");
            if (_applicationManager == null) throw new MissingFieldException($"ApplicationManager not set!");

            _rectTransform = _staminaBar.transform as RectTransform;
            _menuPanel.SetActive(false);
            _gamePanel.SetActive(false);
            _interactionPanel.SetActive(false);

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
            _playerController.InteractionChanged += InteractionChange;
        }

        private void OnDestroy()
        {
            if (_playerController != null)
            {
                _playerController.EnduranceChanged -= ChangeStaminaBar;
                _playerController.InteractionChanged -= InteractionChange;
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

        private void InteractionChange(bool isInteraction)
        {
            _interactionPanel.SetActive(isInteraction);
        }

        #endregion
    }
}