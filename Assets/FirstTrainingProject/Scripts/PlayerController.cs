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
        private float _rotateSpeed; // 400

        [SerializeField]
        private GameObject _head;

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
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * _movementSpeed * Time.deltaTime);
            transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * _rotateSpeed * Time.deltaTime);
            _head.transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y") * _rotateSpeed * Time.deltaTime);
        }
    }
}
