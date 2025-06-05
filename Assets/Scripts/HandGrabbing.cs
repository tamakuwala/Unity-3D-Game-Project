using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrabbing : MonoBehaviour
{
    [SerializeField] private float grabbingSpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other) {
        
        if (other.tag == "Enemy"){
            //transform.parent.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);
            other.gameObject.GetComponent<Enemy>().EnemyIsGrabbed(true);
            Movement(other.gameObject); // Move to enemy
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Enemy"){
            transform.parent.Rotate(new Vector3(-90.0f, 0.0f,  0.0f));
        }
    }
    
    private void Movement(GameObject target){
        // Move hand
        transform.parent.LookAt(target.transform.position); //Aim at target
        transform.parent.Rotate(new Vector3(-90.0f, 0.0f,  0.0f));
        transform.parent.position += transform.forward * grabbingSpeed * Time.deltaTime;
    }
}
