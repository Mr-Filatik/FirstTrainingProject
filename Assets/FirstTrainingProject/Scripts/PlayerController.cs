using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed; // 2
    [SerializeField]
    private float _rotateSpeed; // 100

    void Start()
    {
        //transform.position = new Vector3(1, 3, 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * _movementSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * _rotateSpeed * Time.deltaTime);
    }
}
