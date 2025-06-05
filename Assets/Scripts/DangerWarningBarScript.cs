using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerWarningBarScript : MonoBehaviour
{
    public float warningDistance;

    private MeshRenderer rend;
    private Transform delete_zone;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        delete_zone = GameObject.FindGameObjectWithTag("Kill_Bar").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckForKillBar(-Vector3.forward) && rend.enabled == false)
        {
            rend.enabled = true;
            AudioManager.instance.Play("Alert");
            Vector3 rotation = transform.rotation.eulerAngles;
            transform.LookAt(Camera.main.transform);
            transform.rotation = Quaternion.Euler(-transform.rotation.eulerAngles.x, rotation.y, rotation.z);
        }
        else if (!CheckForKillBar(-Vector3.forward))
        {
            rend.enabled = false;
        }
            
    }

    private bool CheckForKillBar(Vector3 direction)
    {
        int layerMask = 1 << 7;

        RaycastHit hit;
        if (Physics.Raycast(Player.instance.transform.position, direction, out hit, warningDistance, layerMask))
        {
            return true;
        }
        else
            return false;
    }

}
