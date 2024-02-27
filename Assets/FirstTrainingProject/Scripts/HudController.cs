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

    [Header("Elements")]

    [SerializeField]
    private GameObject _staminaBar;

    #endregion

    #region Private Fields

    private PlayerController _playerController;
    private RectTransform _rectTransform;

    #endregion

    private void ChangeStaminaBar(float value)
    {
        if (_rectTransform == null)
        {
            _rectTransform = _staminaBar.transform as RectTransform;
        }
        _rectTransform.localScale = new Vector3(value, 1.0f, 1.0f);
    }

    #region Unity Medhods

    private void Awake()
    {
        if (_staminaBar == null) throw new System.Exception($"StaminaBar not set!");
        if (_applicationManager == null) throw new System.Exception($"ApplicationManager not set!");

        _rectTransform = _staminaBar.transform as RectTransform;
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
    }

    #endregion
}
