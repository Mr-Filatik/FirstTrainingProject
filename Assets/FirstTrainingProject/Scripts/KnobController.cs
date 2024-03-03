using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstTrainingProject
{
    public class KnobController : MonoBehaviour
    {
        public event Action<GameObject> KnobTriggered;

        public void KnobTrigger()
        {
            KnobTriggered?.Invoke(gameObject);
        }
    }
}