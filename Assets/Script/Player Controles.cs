using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerControles : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    private Vector2 moveInputValue;
    private Vector2 cameraInputValue;
    private float yRot;
    public float mouseSensitivity = 1f;

    private void OnNewaction(InputValue value)
    {
        moveInputValue = value.Get<Vector2>();
        Debug.Log(moveInputValue);
    }
    private void OnCamera(InputValue value)
    {
        cameraInputValue = value.Get<Vector2>();
        Debug.Log(cameraInputValue);
    }
    private void MovementLogic()
    {
        transform.Translate(moveInputValue.x * speed * Time.fixedDeltaTime, 0, moveInputValue.y * speed * Time.fixedDeltaTime);
        yRot += Input.GetAxis("Mouse X") * mouseSensitivity;
        if (rb.velocity.z != 0)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yRot, transform.localEulerAngles.z);
        }
    }

    private void FixedUpdate()
    {
        MovementLogic();
    }
}
