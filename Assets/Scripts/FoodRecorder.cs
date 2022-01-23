using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRecorder : MonoBehaviour
{
    public List<FoodBase> FoodInRange = new List<FoodBase>();
    public ScoreManager TargetScoreManager;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<FoodBase>() != null)
        {
            FoodInRange.Add(other.GetComponent<FoodBase>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<FoodBase>() != null)
        {
            FoodInRange.Remove(other.GetComponent<FoodBase>());
        }
    }

    public void OnConsumeFood()
    {
        for (int i = FoodInRange.Count - 1; i >= 0 ; i--)
        {
            TargetScoreManager.AddScore(FoodInRange[i].Score);
            Destroy(FoodInRange[i].gameObject);
        }
        
        FoodInRange = new List<FoodBase>();
    }
}
