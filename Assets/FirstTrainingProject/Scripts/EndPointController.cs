using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstTrainingProject
{
    public class EndPointController : MonoBehaviour
    {
        [SerializeField]
        private ApplicationManager _applicationManager;

        private void OnTriggerEnter(Collider other)
        {
            if (other != null && other.tag == "Player")
            {
                var gc = _applicationManager.GameController;
                if (gc != null)
                {
                    gc.EndGame();
                }
            }
        }
    }
}
