using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class StartButtonManager : MonoBehaviour
{
    public GameObject startBtn;
    public string sceneName;
    private FMOD.Studio.Bus MasterBus;
  
    // Start is called before the first frame update
    void Start()
    {
        startBtn.GetComponent<Button>().onClick.AddListener(OnStartBtnClicked);
        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
    }

    void OnStartBtnClicked()
    {
        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
