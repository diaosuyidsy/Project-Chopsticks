using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{

    public static int timerValue = 10;
    public ScoreManager player1Score;
    public ScoreManager player2Score;

    public Text player1Text;
    public Text player2Text;
    public GameObject blur;
    public GameObject startbutton;
    
    

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
        blur.SetActive(true);

        
        //here add finish game logic
        if (player1Score.scoreValue > player2Score.scoreValue)
        {

            player1Text.text = "Win";
            player2Text.text = "Lose";
            
        }else if (player1Score.scoreValue < player2Score.scoreValue)
        {
            player2Text.text = "Win";
            player1Text.text = "Lose";
        }
        else
        {
            player2Text.text = "Win";
            player1Text.text = "Win";
        }
        player1Score.enabled = false;
        player2Score.enabled = false;
        startbutton.SetActive(true);
        
        
        

    }
}
