using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinScript : MonoBehaviour
{
    public float spinspeed = 30f;
    private void FixedUpdate()
    {
        var currentRotation = transform.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y + spinspeed * Time.deltaTime, currentRotation.z);
    }
}
