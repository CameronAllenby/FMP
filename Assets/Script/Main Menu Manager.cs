using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _homeMainMenuCanvasGO;
    [SerializeField] private GameObject _homeSettingsMenuCanvasGO;

    [SerializeField] private GameObject _homeMainMenuFirst;
    [SerializeField] private GameObject _homeSettingsMenuFirst;

    private bool isPaused;
    private void Start()
    {
        _homeMainMenuCanvasGO.SetActive(true);
        _homeSettingsMenuCanvasGO.SetActive(false);
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
        _homeMainMenuCanvasGO.SetActive(true);
        _homeSettingsMenuCanvasGO.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_homeMainMenuFirst);
    }
    private void CloseAllMenu()
    {
        _homeMainMenuCanvasGO.SetActive(false);
        _homeSettingsMenuCanvasGO.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_homeSettingsMenuFirst);
    }
}
