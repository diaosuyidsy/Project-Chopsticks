using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRecorder : MonoBehaviour
{
    public HashSet<FoodBase> FoodInRange = new HashSet<FoodBase>();

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
}
