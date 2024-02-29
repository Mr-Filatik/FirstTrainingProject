using System;
using UnityEngine;

namespace FirstTrainingProject
{
    public class EndPointController : MonoBehaviour
    {
        #region Serialize Fields

        [Header("Main")]

        [SerializeField]
        private ApplicationManager _applicationManager;

        [Header("Values")]

        [SerializeField]
        private bool _isWin = false;

        #endregion

        #region Unity Medhods

        private void Awake()
        {
            if (_applicationManager == null) throw new MissingFieldException($"ApplicationManager not set!");
        }

        #endregion

        #region Special Medhods

        private void OnTriggerEnter(Collider other)
        {
            if (other != null && other.CompareTag("Player"))
            {
                _applicationManager.ApplicationGameEnd(_isWin);
            }
        }

        #endregion
    }
}
