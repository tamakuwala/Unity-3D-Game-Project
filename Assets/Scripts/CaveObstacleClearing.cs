using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveObstacleClearing : MonoBehaviour
{
    public float timeUntilDisable = 1.0f;
    public bool destroyAfterStart = false;

    private void Start()
    {
        if (destroyAfterStart)
            Destroy(gameObject, timeUntilDisable);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Boulder"))
            Destroy(other.gameObject);
    }
}
