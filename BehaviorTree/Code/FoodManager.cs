using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FoodManager : MonoBehaviour
{
    public static event EventHandler<Vector2> foodSpawned;
    
    public List<GameObject> foodPrefabs;
    public int maxFood;
    public Text foodCountText;
    public int spawnRate;
    
    private int currentFoodCount;
    
    public void SpawnFood()
    {
        if (currentFoodCount >= maxFood) return;
        
        var randomIndex = Random.Range(0, foodPrefabs.Count);
        var randomPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-5, 5));
        Instantiate(foodPrefabs[randomIndex], randomPosition, Quaternion.identity);
        currentFoodCount++;
        foodCountText.text = currentFoodCount.ToString();
        foodSpawned?.Invoke(this, randomPosition);
    }

    // Start is called before the first frame update
    void Start()
    {
        foodCountText.text = currentFoodCount.ToString();
        StartCoroutine(SpawnFoodTimer());
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator SpawnFoodTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnFood();
        }
    }
}
