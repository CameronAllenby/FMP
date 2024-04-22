using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public enum States
{
    None,
    Idle,
    Walk,
    Jump,
    Death,
    Glide,
    Aim,
}
public class PlayerControles : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    private Vector2 moveInputValue;
    private Vector2 cameraInputValue;
    private float yRot;
    public float mouseSensitivity = 1f;
    private Animator anim;

    States state;

    bool grounded;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        state = States.Idle;
    }
    void DoLogic()
    {
        if (state == States.Idle)
        {
            PlayerIdle();
        }

        if (state == States.Jump)
        {
            PlayerJumping();
        }

        if (state == States.Walk)
        {
            PlayerWalk();
        }

        if (state == States.Death)
        {
            PlayerDeath();
        }
    }

    void PlayerIdle()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // simulate jump
            state = States.Jump;
            rb.velocity = new Vector3(0, 100, 0);
        }

        anim.SetBool("walk", false);
        if (Input.GetAxis("Mouse X") != 0f)
        {
            state = States.Walk;
        }
    }

    void PlayerJumping()
    {
        // player is jumping, check for hitting the ground
        if (grounded == true)
        {
            //player has landed on floor
            state = States.Idle;
        }
    }

    void PlayerWalk()
    {
        transform.Translate(moveInputValue.x * speed * Time.fixedDeltaTime, 0, moveInputValue.y * speed * Time.fixedDeltaTime);
        yRot += Input.GetAxis("Mouse X") * mouseSensitivity;
        if (rb.velocity.z != 0)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yRot, transform.localEulerAngles.z);
        }
        if (moveInputValue.x == 0f && moveInputValue.y == 0f)
        {
            state = States.Idle;
        }
    }

    void PlayerDeath()
    {

    }

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
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Floor")
        {
            grounded = true;
            print("landed!");
        }

        if (col.gameObject.tag == "EvilCubeTheDestroyerOfWorlds")
        {
            state = States.Death;
        }
    }
    private void OnGUI()
    {
        //debug text
        string text = "Left/Right arrows = Rotate\nSpace = Jump\nUp Arrow = Forward\nCurrent state=" + state;

        // define debug text area
        GUILayout.BeginArea(new Rect(10f, 450f, 1600f, 1600f));
        GUILayout.Label($"<size=16>{text}</size>");
        GUILayout.EndArea();
    }
}
