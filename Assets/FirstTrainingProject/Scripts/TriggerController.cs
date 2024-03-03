using System;
using UnityEngine;

namespace FirstTrainingProject
{
    public class TriggerController : MonoBehaviour
    {
        //m.b. add serialize field for size and type collider and awake create

        #region Private Fields

        private Action<GameObject> _action; //m.b. change on UnityAction

        #endregion

        #region Public Medhods

        public void SetAction(Action<GameObject> action)
        {
            _action = action;
        }

        #endregion

        #region Special Medhods

        private void OnTriggerEnter(Collider other)
        {
            _action?.Invoke(other.gameObject);
        }

        #endregion
    }
}
