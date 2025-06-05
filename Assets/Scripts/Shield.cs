using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private GameObject player;
    private IEnumerator coroutine;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (player == null){ Destroy(this.gameObject);}
            
        coroutine = TimedSelfDestruct(15.0f);
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
    }

    private IEnumerator TimedSelfDestruct(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
        player.GetComponent<Player>().SetShield(false);
    }
}
