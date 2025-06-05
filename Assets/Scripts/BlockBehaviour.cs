using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    public Vector3 spawnOffset = Vector3.zero;
    public GameObject colliderObject;

    public GameObject SpawnCollider(Vector3 position)
    {
        if (colliderObject != null)
            return Instantiate(colliderObject, position, transform.rotation);
        else
            return null;
    }
}
