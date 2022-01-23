using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject startBtn, settingBtn, exitBtn, sliders;
    public string sceneName;

    void Start()
    {
        startBtn.GetComponent<Button>().onClick.AddListener(OnStartBtnClicked);
        settingBtn.GetComponent<Button>().onClick.AddListener(OnSettingBtnClicked);
        exitBtn.GetComponent<Button>().onClick.AddListener(OnExitBtnClicked);
    }
    
    public void OnStartBtnClicked()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnSettingBtnClicked()
    {
        sliders.SetActive(!sliders.activeSelf);
    }

    public void OnExitBtnClicked()
    {
        Application.Quit();
    }
}
