using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerLine2 : MonoBehaviour
{
    public GameObject enemyPrefab; 

    [SerializeField] private float spawnRate = 2.0f;
    private float spawnTimer; 

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy(); 
    }

    private void SpawnEnemy() {

       Vector3 randomPosition = new Vector3(3 , 2, Random.Range(5,21));
        if (Time.time > spawnTimer) {
            GameObject spawnedEnemy = Instantiate(enemyPrefab, randomPosition, transform.rotation);
            spawnTimer = Time.time + spawnRate; 
        }
    }
}