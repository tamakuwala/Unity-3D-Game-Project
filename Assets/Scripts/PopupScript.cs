using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupScript : MonoBehaviour
{
    public Vector3 offset;

    private float randOffsetX;
    private void Start()
    {
        transform.position = Player.instance.transform.position + offset;
        transform.parent = Player.instance.transform;

        randOffsetX = Random.Range(-1.0f, 1.0f);
        float lifespan = Random.Range(0, 0.4f) + 1.0f;

        Destroy(gameObject, lifespan);
    }

    private void Update()
    {
        // transform.position = Player.instance.transform.position + offset;
        Vector3 rotation = transform.rotation.eulerAngles;
        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.Euler(-transform.rotation.eulerAngles.x, rotation.y, rotation.z);

        float moveY = 2;
        

        transform.position += new Vector3(randOffsetX, moveY, 0) * Time.deltaTime;
    }
}
