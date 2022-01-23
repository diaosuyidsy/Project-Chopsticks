using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int scoreValue = 0;

    private Text score;
    
    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<Text>();
       
    }

    // Update is called once per frame
    void Update()
    {
        score.text = scoreValue.ToString();
    }


    public void AddScore(int score)
    {
        scoreValue += score;
    }
    


}
