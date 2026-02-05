using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class WindowSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer = null;

    [SerializeField] private Toggle toggleSFX;
    [SerializeField] private Toggle toggleMusic;
    [SerializeField] private Button buttonExit;

    private const string MUSIC_PARAM = "MusicVolume";
    private const string SFX_PARAM = "SFXVolume";

    private void Start()
    {
        toggleSFX.onValueChanged.AddListener(OnSFXToggle);
        toggleMusic.onValueChanged.AddListener(OnMusicToggle);
        buttonExit.onClick.AddListener(ExitSettings);

        toggleSFX.isOn = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
        toggleMusic.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;

        OnSFXToggle(toggleSFX.isOn);
        OnMusicToggle(toggleMusic.isOn);
    }

    private void OnMusicToggle(bool isOn)
    {
        audioMixer.SetFloat(MUSIC_PARAM, isOn ? 0f : -80f); // 0 dB = норм, -80 dB = тишина
        PlayerPrefs.SetInt("MusicEnabled", isOn ? 1 : 0);
    }

    private void OnSFXToggle(bool isOn)
    {
        audioMixer.SetFloat(SFX_PARAM, isOn ? 0f : -80f);
        PlayerPrefs.SetInt("SFXEnabled", isOn ? 1 : 0);
    }

    private void ExitSettings()
    {
        this.gameObject.SetActive(false);
    }
}
