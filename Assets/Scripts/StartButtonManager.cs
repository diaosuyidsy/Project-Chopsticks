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
  
    // Start is called before the first frame update
    void Start()
    {
        startBtn.GetComponent<Button>().onClick.AddListener(OnStartBtnClicked);
    }

    // Update is called once per frame
    void Update()
    {
     
        
        
    }
    void OnStartBtnClicked()
    {
        SceneManager.LoadScene(sceneName);
    }
}
