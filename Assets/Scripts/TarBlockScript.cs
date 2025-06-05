using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarBlockScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Debug.Log("Player walked on tar!");
    }
}
