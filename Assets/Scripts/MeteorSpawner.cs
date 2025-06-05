using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{


    public GameObject spawnObject;
    private float spawnTimer;
    private float spawnRate;
    private Vector3 spawnPosition;
    private Vector3 velocity;
    private float treadmillSpeed;



    private void Awake() {
        spawnRate = Loader.getSpawnRate();  
        if (spawnRate == 0) { Debug.Log("Loader not initialized, start from start scene");}  
    }

    void Start(){

        spawnRate = Loader.getSpawnRate();  
    }

    void Update()
    {
        
       
        if (GameObject.FindWithTag("Player") != null) {
            

           if (Time.time > spawnTimer){

                Vector3 spawnPosition = GetRandomPosition();
                Vector3 force = new Vector3(-300, 1000, 0);
                GameObject meteorTelegraph = Instantiate(spawnObject, spawnPosition, Quaternion.identity);
                meteorTelegraph.GetComponent<MeteorTelegraph>().Initialize(force, spawnPosition);
                spawnTimer = Time.time + spawnRate;
            }

        }
    }


    public void ChangeSpawnRate(float change){
        spawnRate = change;
    }

    
    private Vector3 GetRandomPosition(){
        float minZPlayer;
        if (GameObject.FindWithTag("Player")!=null){
            minZPlayer = GameObject.FindWithTag("Player").transform.position.z;
        } else {
            minZPlayer = 10f;
        }
        float minX = 17.25f;
        float maxX = 43.5f;
        float minZ = 10f;
        float maxZ = 30f;

        return new Vector3(Random.Range(minX, maxX), 2, Random.Range(minZPlayer + minZ, minZPlayer + maxZ));

        
    }


}
