using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderRandomization : MonoBehaviour
{
    public float minScale;

    private void Start()
    {
        var randomScale = Random.Range(minScale, 1);
        var randomDegree = Random.Range(0, 360);

        transform.localScale = Vector3.one * randomScale;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, randomDegree, transform.rotation.eulerAngles.y);
    }
}
