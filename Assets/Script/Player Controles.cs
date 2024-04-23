using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using Unity.VisualScripting;

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
    [SerializeField] private float speed;
    public CharacterController controller;
    private Vector2 moveInputValue;
    private Vector2 cameraInputValue;
    private float yRot;
    public float mouseSensitivity = 1f;
    private Animator anim;
    private bool jump;
    States state;
    Vector3 velocity;
    public float gravity = -9.81f;
    void Update()
    {
        DoLogic();
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void FixedUpdate()
    {
        grounded = false;
    }

    bool grounded;

    private void Start()
    {
        anim = GetComponent<Animator>();
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
        if (grounded == false)
        {
           
        }

        anim.SetBool("walk", false);

        if (moveInputValue.x != 0f || moveInputValue.y != 0f)
        {
            state = States.Walk;
        }
    }

    void PlayerJumping()
    {
        if (grounded == false)
        {
            anim.SetBool("Fall", true);
            anim.SetBool("jump", false);

        }
        // player is jumping, check for hitting the ground
        if (grounded == true)
        {
            Jcount = 1;
            state = States.Idle;
        }
    }
    void PlayerWalk()
    {
        anim.SetBool("walk", true);
        transform.Translate(moveInputValue.x * speed * Time.fixedDeltaTime, 0, moveInputValue.y * speed * Time.fixedDeltaTime);
        yRot += Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yRot, transform.localEulerAngles.z);
        if (moveInputValue.x <= 0.1f && moveInputValue.y <= 0.1f)
        {
            state = States.Idle;
        }
    }
    int Jcount = 1;
    void OnJump()
    {
        anim.SetBool("walk", false);
        transform.Translate(moveInputValue.x * speed * Time.fixedDeltaTime, 0, moveInputValue.y * speed * Time.fixedDeltaTime);
        // simulate jump
        state = States.Jump;
        Debug.Log("Pressed");
        if (Jcount >= 0)
        {

            Jcount--;

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
        string text = "Current state=" + state;

        // define debug text area
        GUILayout.BeginArea(new Rect(10f, 450f, 1600f, 1600f));
        GUILayout.Label($"<size=16>{text}</size>");
        GUILayout.EndArea();
    }
}
