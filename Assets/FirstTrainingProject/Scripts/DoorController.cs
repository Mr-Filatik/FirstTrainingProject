using UnityEngine;

namespace FirstTrainingProject
{
    public class DoorController : MonoBehaviour
    {
        #region Serialize Fields

        [Header("Elements")]

        [SerializeField]
        private GameObject _doorHinge;

        [SerializeField]
        private GameObject _doorKnobIn;

        [SerializeField]
        private GameObject _doorKnobOut;

        [Header("Values")]

        [SerializeField]
        private DoorOpeningDirection _doorOpeningDirection;

        [SerializeField]
        private bool _isOpen;

        #endregion

        private Vector3 _closeRotation = Vector3.zero;
        private Vector3 _openInRotation = new Vector3(0F, 90F, 0F);
        private Vector3 _openOutRotation = new Vector3(0F, -90F, 0F);

        private void Open(GameObject knob)
        {
            _isOpen = !_isOpen;
            if (_isOpen)
            {
                if (_doorOpeningDirection == DoorOpeningDirection.In)
                {
                    _doorHinge.transform.localEulerAngles = _openInRotation;
                }
                if (_doorOpeningDirection == DoorOpeningDirection.Out)
                {
                    _doorHinge.transform.localEulerAngles = _openOutRotation;
                }
            }
            else
            {
                _doorHinge.transform.localEulerAngles = _closeRotation;
            }
        }

        private void Awake()
        {
            var kc1 = _doorKnobIn?.GetComponent<KnobController>();
            if (kc1 != null) 
            {
                kc1.KnobTriggered += Open; 
            }
            var kc2 = _doorKnobOut?.GetComponent<KnobController>();
            if (kc2 != null)
            {
                kc2.KnobTriggered += Open;
            }
        }

        private void OnDestroy()
        {
            var kc1 = _doorKnobIn?.GetComponent<KnobController>();
            if (kc1 != null)
            {
                kc1.KnobTriggered -= Open;
            }
            var kc2 = _doorKnobOut?.GetComponent<KnobController>();
            if (kc2 != null)
            {
                kc2.KnobTriggered -= Open;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum DoorOpeningDirection
    {
        /// <summary>
        /// 
        /// </summary>
        In,

        /// <summary>
        /// 
        /// </summary>
        Out,

        /// <summary>
        /// 
        /// </summary>
        InOut
    }
}
