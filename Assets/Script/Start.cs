using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Leavegame : MonoBehaviour
{

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

    public void Back2Menu()
    {
        SceneManager.LoadScene(0);
    }
}
