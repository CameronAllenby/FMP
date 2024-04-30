using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

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
    States state;
    Vector3 velocity;
    public float gravity = -9.81f;
    public float jumpHight = 2f;

    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    bool isGrounded;

    bool isSprint;
    int Sprint = 0;

    public float turnSmooth = 0.1f;
    float turnVelo;
    public Transform cam;

    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1;
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
            velocity.y += Mathf.Sqrt(jumpHight);
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
        float targitAngle = Mathf.Atan2(moveInputValue.x, moveInputValue.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targitAngle, ref turnVelo, turnSmooth);
        Vector3 direction = new Vector3(moveInputValue.x, 0f, moveInputValue.y).normalized;
        Vector3 moveDire = Quaternion.Euler(0f, targitAngle, 0f) * Vector3.forward;
        controller.Move(moveDire.normalized * speed * Time.deltaTime);
        
        transform.rotation = Quaternion.Euler(0, angle, 0);


        if (moveInputValue.x == 0 && moveInputValue.y == 0)
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
        float targitAngle = Mathf.Atan2(moveInputValue.x, moveInputValue.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targitAngle, ref turnVelo, turnSmooth);
        Vector3 direction = new Vector3(moveInputValue.x, 0f, moveInputValue.y).normalized;
        Vector3 moveDire = Quaternion.Euler(0f, targitAngle, 0f) * Vector3.forward;
        controller.Move(moveDire.normalized * speed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, angle, 0);
        if (moveInputValue.x == 0 && moveInputValue.y == 0)
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
    void OnJump()
    {
        Debug.Log(isGrounded);
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
