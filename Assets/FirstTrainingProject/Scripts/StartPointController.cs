using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace FirstTrainingProject
{
    public class StartPointController : MonoBehaviour
    {
        #region Serialize Fields

        [Header("Main")]

        [SerializeField]
        private ApplicationManager _applicationManager;

        #endregion

        #region Private Fields

        private bool _isGame = false;

        #endregion

        #region Unity Medhods

        private void Awake()
        {
            if (_applicationManager == null) throw new System.Exception($"ApplicationManager not set!");

            _applicationManager.ApplicationGameStarted += GameStart;
            _applicationManager.ApplicationGameEnded += GameEnd;
        }

        private void OnDestroy()
        {
            _applicationManager.ApplicationGameStarted -= GameStart;
            _applicationManager.ApplicationGameEnded -= GameEnd;
        }

        #endregion

        #region Special Medhods

        private void OnTriggerEnter(Collider other)
        {
            if (other != null && other.CompareTag("Player"))
            {
                if (_isGame)
                {
                    _applicationManager.ApplicationGameContinue();
                }
                else
                {
                    _applicationManager.ApplicationGameStart();
                }

            }
        }

        #endregion

        #region Private Methods

        private void GameStart()
        {
            _isGame = true;
        }

        private void GameEnd(bool isWin)
        {
            _isGame = false;
        }

        #endregion
    }
}
