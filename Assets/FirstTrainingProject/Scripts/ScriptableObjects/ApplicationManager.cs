using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstTrainingProject
{
    [CreateAssetMenu(fileName = "ApplicationManager", menuName = "ScriptableObjects/ApplicationManager", order = 1)]
    public class ApplicationManager : ScriptableObject
    {
        #region Events

        public event Action ApplicationStarted;
        public event Action ApplicationGameInited;
        public event Action ApplicationGameStarted;
        public event Action ApplicationGamePaused;
        public event Action ApplicationGameContinued;
        public event Action<bool> ApplicationGameEnded;
        public event Action ApplicationQuited;

        #region EventCalls

        public void ApplicationStart() => ApplicationStarted?.Invoke();

        public void ApplicationGameInit() => ApplicationGameInited?.Invoke();

        public void ApplicationGameStart() => ApplicationGameStarted?.Invoke();

        public void ApplicationGamePause() => ApplicationGamePaused?.Invoke();

        public void ApplicationGameContinue() => ApplicationGameContinued?.Invoke();

        public void ApplicationGameEnd(bool isWin) => ApplicationGameEnded?.Invoke(isWin);

        public void ApplicationQuit() => ApplicationQuited?.Invoke();

        #endregion

        #endregion

        #region Controllers

        public GameController GameController { get; set; } = null;

        public MapController MapController { get; set; } = null;

        public PlayerController PlayerController { get; set; } = null;

        public EnemyController EnemyController { get; set; } = null;

        #endregion

        #region Event Logging

        private ApplicationManager()
        {
            ApplicationStarted += ApplicationStartLog;
            ApplicationGameInited += ApplicationGameInitLog;
            ApplicationGameStarted += ApplicationGameStartLog;
            ApplicationGamePaused += ApplicationGamePauseLog;
            ApplicationGameContinued += ApplicationGameContinueLog;
            ApplicationGameEnded += ApplicationGameEndLog;
            ApplicationQuited += ApplicationQuitLog;

            // Where to unsubscribe to ScriptableObject?
        }

        public void ApplicationStartLog() => Debug.Log("Application Start");
        public void ApplicationGameInitLog() => Debug.Log("Aplication Game Init");
        public void ApplicationGameStartLog() => Debug.Log("Application Game Start");
        public void ApplicationGamePauseLog() => Debug.Log("Application Game Pause");
        public void ApplicationGameContinueLog() => Debug.Log("Application Game Continue");
        public void ApplicationGameEndLog(bool isWin) => Debug.Log($"Application Game End (Win = {isWin})");
        public void ApplicationQuitLog() => Debug.Log("Application Quit");

        #endregion
    }
}
