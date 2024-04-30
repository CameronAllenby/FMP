using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Leavegame : MonoBehaviour
{
    public string sceneName;
    
    public void ChangeScene()
    {
        print("loading scene " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void PlayBottonSound2()
    {
        print("ee");
        AudioManager.Instance.PlayBottonSound();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
