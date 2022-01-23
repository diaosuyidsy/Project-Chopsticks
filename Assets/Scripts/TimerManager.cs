using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{

    public static int timerValue = 100;

    private Text time;
    
    // Start is called before the first frame update
    void Start()
    {
        time = GetComponent<Text>();
        StartCoroutine(CountTime());
    }

    // Update is called once per frame
    void Update()
    {
        time.text = timerValue.ToString();
    }

    IEnumerator CountTime()
    {
        while (timerValue >0)
        {
            timerValue--;
            yield return new WaitForSecondsRealtime(1f);
        }
        //here add finish game logic
    }
}
