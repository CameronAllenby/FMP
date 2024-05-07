using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public bool MenuOpenCloseInput { get; private set; }

    private PlayerInput _playerInput;
    public static InputManager Instance;
    private InputAction _action;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        _playerInput = GetComponent<PlayerInput>();

        _action = _playerInput.actions["StartButton"];
    }
    private void Update()
    {
        MenuOpenCloseInput = _action.WasPressedThisFrame();
    }
}
