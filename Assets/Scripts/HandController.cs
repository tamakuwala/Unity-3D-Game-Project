using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    private GameObject pivotObject;
    [SerializeField] private float revolutionSpeed = 20f;

    private bool onEnemy = false;
    private GameObject enemy;
    private int usageCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        pivotObject = GameObject.FindGameObjectWithTag("Player");      
        transform.RotateAround(pivotObject.transform.position, Vector3.up, revolutionSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {   
        if (GameObject.FindGameObjectWithTag("Player")!=null){
            if ((enemy == null) && (onEnemy)){
                // Enemy is dead
                onEnemy = false;
                transform.RotateAround(pivotObject.transform.position, Vector3.up, revolutionSpeed * Time.deltaTime);
                usageCount++;
                if (usageCount > 1) {
                    Destroy(this.gameObject);
                }
            } else if ((enemy!=null) && (onEnemy)) {
                // On enemy
                transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.5f, enemy.transform.position.z);
                    
            } else {
                // Not on enemy
                transform.RotateAround(pivotObject.transform.position, Vector3.up, revolutionSpeed * Time.deltaTime);
            }
    
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!onEnemy){
            if (other.tag == "Enemy"){
                onEnemy = true;
                enemy = other.gameObject;
            }
        }


    }




}



