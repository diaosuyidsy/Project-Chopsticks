using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodSpawner : MonoBehaviour
{

    public List<FoodBase> foods;
    public int allowedActiveFoodCount = 4;

    public int activeFoodCount = 0;
    // Start is called before the first frame update




    void  Start()
    {
        foods = FindObjectsOfType<FoodBase>(true).ToList();
      
            StartCoroutine(SpawnRoutine());
        
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (activeFoodCount < allowedActiveFoodCount && foods.Count>0)
            {
                int rand = Random.Range(0, foods.Count - 1);
                FoodBase selectedFood = foods[rand];
                foods.Remove(selectedFood);
                activeFoodCount++;
                StartCoroutine(selectedFood.SpawnRoutine());
            }

            yield return new WaitForSecondsRealtime(1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
