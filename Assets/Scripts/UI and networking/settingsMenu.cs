using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using FishNet.Managing;
using TMPro;

public class settingsMenu : MonoBehaviour
{
    [Header("Menues")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject CosmonautsWinMenu;
    [SerializeField] private GameObject MonsterWinMenu;
    [SerializeField] private GameObject HostButton;

    [Header("Audio")]
    [SerializeField] private AudioMixer playerMixer;
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioSource playa;
    [SerializeField] private AudioClip pop;

    [Header("net")]
    [SerializeField] private NetworkManager networkManager;


    /*
    public void enableSettingMenu()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void enableMainMenu()
    {
        if (networkManager.ClientManager.Started)
        {
            pauseMenu.SetActive(true);
            SettingsMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(true);
            SettingsMenu.SetActive(false);
        }
    }
    */

    void Update()
    {

    }

    public void audioVolume(float _volume)
    {
        if (_volume < 0.0001f)
            _volume = 0.0001f;

        float dB = Mathf.Log10(GameManager.SFX) * 20f;
        masterMixer.SetFloat("masterVolume", dB);
        playSound(pop, sfx);
    }

    public void playerVolume(float _volume)
    {
        if (_volume < 0.0001f)
            _volume = 0.0001f;

        float dB = Mathf.Log10(GameManager.playerVoice) * 20f;
        playerMixer.SetFloat("masterVolume", dB);
        playSound(pop, playa);
    }


    public void backToMainMenuFromGame()
    {
        mainMenu.SetActive(true);
        CosmonautsWinMenu.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        MonsterWinMenu.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        HostButton.SetActive(true);

        if (networkManager.ClientManager.Started)
        {
            networkManager.ClientManager.StopConnection();
        }

        if (networkManager.ServerManager.Started)
        {
            networkManager.ServerManager.StopConnection(true);
        }
    }

    void playSound(AudioClip _sound, AudioSource _source)
    {
        if (_source.isPlaying == false)
        {
            _source.PlayOneShot(_sound);
        }
    }


}
