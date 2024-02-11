using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstTrainingProject
{
    public class EndPointController : MonoBehaviour
    {
        [SerializeField]
        private ApplicationManager _applicationManager;

        [SerializeField]
        private bool _isWin = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other != null && other.CompareTag("Player"))
            {
                _applicationManager.ApplicationGameEnd(_isWin);
            }
        }
    }
}
