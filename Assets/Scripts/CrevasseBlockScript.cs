using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrevasseBlockScript : MonoBehaviour
{

    private float damageToPlayer = 20.0f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            Player.instance.GetComponent<Player>().TakeDamage(damageToPlayer);
        }
            
    }
}
