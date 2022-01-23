using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodSpawner : MonoBehaviour
{
    public List<FoodBase> AllFoods = new List<FoodBase>();

    public List<Transform> spawnPoints;
    public Dictionary<Transform, bool> spawnPointsState = new Dictionary<Transform, bool>();

    public int allowedActiveFoodCount = 3;
    public int activeFoodCount = 0;

    private void Awake()
    {
        AllFoods = GetComponentsInChildren<FoodBase>(true).ToList();

        foreach (var sp in spawnPoints) spawnPointsState.Add(sp, false);

        for (int i = 0; i < allowedActiveFoodCount; i++)
        {
            SpawnFood();
        }
    }

    private void SpawnFood()
    {
        int rand = Random.Range(0, AllFoods.Count);
        var foodToSpawn = AllFoods[rand];

        while (foodToSpawn.gameObject.activeInHierarchy)
        {
            rand = Random.Range(0, AllFoods.Count);
            foodToSpawn = AllFoods[rand];
        }
        // 选中了 非active的 foodToSpawn

        rand = Random.Range(0, spawnPointsState.Count);
        
        while (spawnPointsState.ElementAt(rand).Value)
        {
            // SpawnPoint被占用,随机抽取新value
            rand = Random.Range(0, spawnPointsState.Count);
        }

        var chosenTransform = spawnPointsState.ElementAt(rand).Key;
        Vector3 spawnPosition = chosenTransform.position;
        // 选中了未被占用的 chosenTransform

        spawnPointsState[chosenTransform] = true;
        
        foodToSpawn.spawnTransform = chosenTransform;
        foodToSpawn.transform.position = spawnPosition;
        foodToSpawn.gameObject.SetActive(true);
        
        activeFoodCount++;
            
        StartCoroutine(foodToSpawn.SetFoodActive());

        m_Timer = 0f;
    }
    
    private float m_Timer = 0f;
    public float spawnFoodInterval = 5f;
    void Update()
    {
        if (activeFoodCount < allowedActiveFoodCount)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer >= spawnFoodInterval)
            {
                SpawnFood();
            }
        }
    }
}
