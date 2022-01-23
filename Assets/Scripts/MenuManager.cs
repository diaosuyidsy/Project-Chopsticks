using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject startBtn, settingBtn, exitBtn, sliders, musicSlider, SFXSlider;
    public string sceneName;

    void Start()
    {
        startBtn.GetComponent<Button>().onClick.AddListener(OnStartBtnClicked);
        settingBtn.GetComponent<Button>().onClick.AddListener(OnSettingBtnClicked);
        exitBtn.GetComponent<Button>().onClick.AddListener(OnExitBtnClicked);

        musicSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { OnMusicChange(); });
        SFXSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { OnSFXChange(); });
        MusicSliderAndSFXSliderInitialize();
    }

    private void Update()
    {
        if (ReInput.players.GetPlayer(0).GetButtonDown("StartGame"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    // when start button is clicked
    void OnStartBtnClicked()
    {
        SceneManager.LoadScene(sceneName);
    }

    // when setting button is clicked
    void OnSettingBtnClicked()
    {
        sliders.SetActive(!sliders.activeSelf);
    }

    // when exit button is clicked
    void OnExitBtnClicked()
    {
        Application.Quit();
    }

    // when music volumn is changed
    void OnMusicChange()
    {
        float value = musicSlider.GetComponent<Slider>().value;
        var Music = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        Music.setVolume(value);
    }

    // when sound effect volumn is changed
    void OnSFXChange()
    {
        float value = SFXSlider.GetComponent<Slider>().value;
        var SFX = FMODUnity.RuntimeManager.GetBus("bus:/Sounds");
        SFX.setVolume(value);
    }
    
    void MusicSliderAndSFXSliderInitialize()
    {
        FMODUnity.RuntimeManager.GetBus("bus:/Music").setVolume(musicSlider.GetComponent<Slider>().value = 0.5f);
        FMODUnity.RuntimeManager.GetBus("bus:/Sounds").setVolume(SFXSlider.GetComponent<Slider>().value = 0.5f);
    }
}
