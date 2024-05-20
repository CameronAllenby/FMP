using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Experimental.GlobalIllumination;

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
    AimWalk,
}
public class PlayerControles : MonoBehaviour
{
    [SerializeField] private LayerMask aimCollider = new LayerMask();
    [SerializeField] private CinemachineVirtualCamera AimCam;
    [SerializeField] private float speed;
    [SerializeField] private Transform debugtansform;
    public CharacterController controller;
    private Vector2 moveInputValue;
    private Vector2 cameraInputValue;
    private float yRot;
    public float mouseSensitivity = 1f;
    private Animator anim;
    States state;
    Vector3 velocity;
    public float gravity = -25f;
    public float jumpHight = 2f;

    public Transform groundCheck;
    [SerializeField] private Transform pfBullet;
    [SerializeField] private Transform bulletPosition;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    bool isGrounded;

    bool isSprint;
    int Sprint = 0;

    public float turnSmooth = 0.1f;
    float turnVelo;
    public Transform cam;
    public Transform cam2;
    Transform hitTransform = null;
    bool Aiming;
    bool isAiming;
    [SerializeField] private GameObject _Camera;
    void Update()
    {
        Vector3 worldMouse = Vector3.zero;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        DoLogic();

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimCollider))
        {
            debugtansform.position = raycastHit.point; 
            worldMouse = raycastHit.point;
        }
        hitTransform = raycastHit.transform;
        
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

        if (state == States.AimWalk)
        {
            PlayerAimWalking();
        }
    }

    void PlayerIdle()
    {
        if (isGrounded == false)
        {
            
        }
        anim.SetBool("Running", false);
        anim.SetBool("Walk", false);

        if ((moveInputValue.x != 0f || moveInputValue.y != 0f) && Aiming == false)
        {
            state = States.Walk;
        }

        if ((moveInputValue.x != 0f || moveInputValue.y != 0f) && Aiming == true)
        {
            state = States.AimWalk;
        }

        aiming();
    }
    void PlayerAimWalking()
    {
        anim.SetBool("Walk", false);
        anim.SetBool("Running", false);
        speed = 6;
        float targitAngle = Mathf.Atan2(moveInputValue.x, moveInputValue.y) + cam.eulerAngles.y;

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targitAngle, ref turnVelo, turnSmooth);

        Vector3 direction = new Vector3(moveInputValue.x, 0f, moveInputValue.y).normalized;

        Vector3 moveDire = Quaternion.Euler(0f, targitAngle, 0f) * Vector3.forward;

        Vector3 move = new Vector3(moveInputValue.x, 0, moveInputValue.y);

        move = move.x * cam2.right.normalized + move.z * cam2.forward.normalized;
        move.y = 0f;

        controller.Move(move.normalized * speed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, targitAngle, 0);

       

        if (moveInputValue.x == 0 && moveInputValue.y == 0)
        {
            state = States.Idle;
        }
        aiming();
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
        speed = 6;
        float targitAngle = Mathf.Atan2(moveInputValue.x, moveInputValue.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targitAngle, ref turnVelo, turnSmooth);
        Vector3 direction = new Vector3(moveInputValue.x, 0f, moveInputValue.y).normalized;
        Vector3 moveDire = Quaternion.Euler(0f, targitAngle, 0f) * Vector3.forward;
        controller.Move(moveDire.normalized * speed * Time.deltaTime);
        
        transform.rotation = Quaternion.Euler(0, angle, 0);

        aiming();

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

        aiming();

    }
    void PlayerFall()
    {
        anim.SetBool("Fall", true);
        if (isGrounded == true)
        {
            anim.SetBool("Fall", false);
            state = States.Idle;
        }
        speed = 2;
        float targitAngle = Mathf.Atan2(moveInputValue.x, moveInputValue.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targitAngle, ref turnVelo, turnSmooth);
        Vector3 direction = new Vector3(moveInputValue.x, 0f, moveInputValue.y).normalized;
        Vector3 moveDire = Quaternion.Euler(0f, targitAngle, 0f) * Vector3.forward;
        controller.Move(moveDire.normalized * speed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, angle, 0);

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
    void OnAim()
    {
       
        if (Aiming == false)
        {
            Aiming = true;
        }
        else
        {
            Aiming = false;
        }
    }
    void PlayerDeath()
    {

    }
    void OnShoot()
    {
        Vector3 worldMouse = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimCollider))
        {
            debugtansform.position = raycastHit.point;
            worldMouse = raycastHit.point;
        }
        hitTransform = raycastHit.transform;
        Vector3 aimDir = (worldMouse - bulletPosition.position).normalized;
        Instantiate(pfBullet, bulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
    }
    void aiming()
    {
        if (Aiming == true)
        {
            anim.SetBool("Aim", true);
            AimCam.gameObject.SetActive(true);
            float targitAngle = Mathf.Atan2(moveInputValue.x, moveInputValue.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targitAngle, ref turnVelo, turnSmooth);
            Vector3 direction = new Vector3(moveInputValue.x, 0f, moveInputValue.y).normalized;
            Vector3 moveDire = Quaternion.Euler(0f, targitAngle, 0f) * Vector3.forward;


            transform.rotation = Quaternion.Euler(0, angle, 0);

        }
        else
        {
            anim.SetBool("Aim", false);
            AimCam.gameObject.SetActive(false);
        }
    }
    private void OnNewaction(InputValue value)
    {
        moveInputValue = value.Get<Vector2>();
        Debug.Log(moveInputValue);
    }
    private void OnCamera(InputValue value)
    {
        cameraInputValue = value.Get<Vector2>();
        Debug.Log(cameraInputValue + "p");
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
