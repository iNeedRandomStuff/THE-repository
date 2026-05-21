using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using FishNet.Managing;
using FishNet.Object;

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
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip pop;

    [Header("net")]
    [SerializeField] private NetworkManager networkManager;

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

    public void audioVolume(float _volume)
    {
        if (_volume < 0.0001f)
            _volume = 0.0001f;

        float dB = Mathf.Log10(_volume) * 20f;
        masterMixer.SetFloat("masterVolume", dB);
        playSound(pop);
    }

    public void playerVolume(float _volume)
    {
        if (_volume < 0.0001f)
            _volume = 0.0001f;

        float dB = Mathf.Log10(_volume) * 20f;
        masterMixer.SetFloat("masterVolume", dB);
        playSound(pop);
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

    void playSound(AudioClip _sound)
    {
        if(source.isPlaying == false)
        {
            source.PlayOneShot(_sound);
        }
    }
}
