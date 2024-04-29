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
    Sprint,
    Fall,
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
    public float gravity = -0.81f;
    public float jumpHight = 9f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    bool isSprint;
    int Sprint = 0;
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        DoLogic();
    }

    void FixedUpdate()
    {
 
    }

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

        if (state == States.Sprint)
        {
            PlayerSprint();
        }

        if (state == States.Fall)
        {
            PlayerFall();
        }
    }

    void PlayerIdle()
    {
        if (isGrounded == false)
        {
            
        }
        anim.SetBool("Running", false);
        anim.SetBool("Walk", false);

        if (moveInputValue.x != 0f || moveInputValue.y != 0f)
        {
            state = States.Walk;
        }
    }

    void PlayerJumping()
    {
        if (isGrounded == false)
        {
            anim.SetBool("Fall", true);
            anim.SetBool("Jump", false);
            state = States.Fall;
        }
        // player is jumping, check for hitting the ground
        if (isGrounded == true)
        {
            velocity.y += Mathf.Sqrt(jumpHight * -2 * gravity);
        }
    }
    void PlayerWalk()
    {
        if (isSprint == true)
        {
            state = States.Sprint;
        }
        anim.SetBool("Walk", true);
        anim.SetBool("Running", false);
        speed = 4;
        transform.Translate(moveInputValue.x * speed * Time.fixedDeltaTime, 0, moveInputValue.y * speed * Time.fixedDeltaTime);
        yRot += Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yRot, transform.localEulerAngles.z);
        if (moveInputValue.x <= 0.1f && moveInputValue.y <= 0.1f)
        {
            state = States.Idle;
        }
    }
    void PlayerSprint()
    {
        speed = 16;
        if (isSprint == false)
        {
            state = States.Walk;
        }
        anim.SetBool("Running", true);
        transform.Translate(moveInputValue.x * speed * Time.fixedDeltaTime, 0, moveInputValue.y * speed * Time.fixedDeltaTime);
        yRot += Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yRot, transform.localEulerAngles.z);
        if (moveInputValue.x <= 0.1f && moveInputValue.y <= 0.1f)
        {
            state = States.Idle;
        }

    }
    void PlayerFall()
    {
        anim.SetBool("Fall", true);
        if (isGrounded == true)
        {
            anim.SetBool("Fall", false);
            state = States.Idle;
        }
    
    }
    int Jcount = 1;
    void OnJump()
    {
        if (isGrounded == true)
        {
            state = States.Jump;
            anim.SetBool("Jump", true);
        }
    }
    void OnSprint()
    {
        Sprint++;
        if (Sprint == 0)
        {
            isSprint = false;
        }
        if (Sprint == 1)
        {
            isSprint = true;
        }
        else
        {
            Sprint = 0;
            isSprint = false;
        }
        Debug.Log(Sprint);
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
