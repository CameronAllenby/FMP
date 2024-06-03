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
    }

   

    public void OpenMenu()
    {
        _homeMainMenuCanvasGO.SetActive(false);
        _homeSettingsMenuCanvasGO.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_homeSettingsMenuFirst);
    }
    public void CloseMenu()
    {
        _homeMainMenuCanvasGO.SetActive(true);
        _homeSettingsMenuCanvasGO.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_homeMainMenuFirst);
    }
}
