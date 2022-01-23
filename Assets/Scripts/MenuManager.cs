using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    }

    // when sound effect volumn is changed
    void OnSFXChange()
    {

    }
}
