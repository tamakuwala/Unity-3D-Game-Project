using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    //Should refactor to get from controller to be in sync with treadmill
    [SerializeField] private GameObject treadmill;


    // Update is called once per frame
    void Update()
    {
        
        GameObject[] meteors = GameObject.FindGameObjectsWithTag("Meteor");
          
        //This moves all meteors in time with the treadmill
        foreach (GameObject meteor in meteors)
        {
            //meteor.transform.Translate(-Vector3.forward * treadmill.GetComponent<TreadmillGeneration>().GetTreadmillSpeed() * Time.deltaTime);

        }


    }
}

