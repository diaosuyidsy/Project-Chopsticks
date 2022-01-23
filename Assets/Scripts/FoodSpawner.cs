using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{

    public List<FoodBase> foods;

    public int activeFoodCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        if (activeFoodCount < 2)
        {
            
        }

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
