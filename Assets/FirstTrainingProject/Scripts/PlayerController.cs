using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace FirstTrainingProject
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float _movementSpeed; // 2

        [SerializeField]
        private float _rotateSpeed; // 100

        [SerializeField]
        private ApplicationManager _applicationManager;

        private void Awake()
        {
            _applicationManager.PlayerController = this;
        }

        private void Start()
        {
            //transform.position = new Vector3(1, 3, 1);
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * _movementSpeed * Time.deltaTime);
            transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * _rotateSpeed * Time.deltaTime);
        }
    }
}
