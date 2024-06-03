using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuCanvasGO;
    [SerializeField] private GameObject _SettingsMenuCanvasGO;

    [SerializeField] private GameObject _mainMenuFirst;
    [SerializeField] private GameObject _settingsMenuFirst;

    private bool isPaused;
    private void Start()
    {
        _mainMenuCanvasGO.SetActive(false);
        _SettingsMenuCanvasGO.SetActive(false);

        Time.timeScale = 1;
    }
    private void Update()
    {
        if (InputManager.Instance.MenuOpenCloseInput)
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                UnPause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        OpenMenu();
    }
    public void UnPause()
    {
        isPaused = false;
        Time.timeScale = 1;
        CloseAllMenu();
    }

    private void OpenMenu()
    {
        _mainMenuCanvasGO.SetActive(true);
 

        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }
    private void CloseAllMenu()
    {
        _mainMenuCanvasGO.SetActive(false);

    }
}
