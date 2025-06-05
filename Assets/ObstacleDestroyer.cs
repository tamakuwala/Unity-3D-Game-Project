using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attackable"))
        {
            Debug.Log("Attackable detected!");
            var obScript = other.GetComponentInParent<ObstacleData>();
            if (obScript != null)
                obScript.KillObstacle();
        }
    }
}
