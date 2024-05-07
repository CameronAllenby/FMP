using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicSource,sfxSource,masterSource;
    public Sound[] musicSounds,sfxSounds,masterSounds;
    
    public float MS = 1;
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
    }
    // Start is called before the first frame update
    // Update is called once per frame


    private void Start()
    {
        PlayMusic("Backround");
    }
     
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void PlayBottonSound()
    {
        print("ee3");
        PlaySFX("SFX");
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void MasterVolume(float volume)
    {
        masterSource.volume = volume;
        sfxSource.volume = volume;
        musicSource.volume = volume;
    }
}