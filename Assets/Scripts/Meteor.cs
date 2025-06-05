using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private IEnumerator coroutine;

    
    public GameObject crashEffect;
    float degreesPerSecond;
    float xAngle, yAngle, zAngle;
    [SerializeField] private AudioClip whistlingSound;
    [SerializeField] private AudioClip destructionSound;
    private AudioSource source;
    private Vector3[] positions;
    private float movementSpeed = 15.0f;
    private int index = 0;

void Awake(){
    
    positions = this.transform.parent.GetComponent<MeteorTelegraph>().GetPositions();
}
    // Start is called before the first frame update
    void Start()
    {
        xAngle = Random.Range(-1.0f, 1.0f);
        yAngle = Random.Range(-1.0f, 1.0f);
        zAngle = Random.Range(-1.0f, 1.0f);
        source = this.gameObject.transform.GetComponent<AudioSource>();
        
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<TrailRenderer>().enabled = false;
        this.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
    }

    void Update(){
        
        if (transform.position.y > 20f) {
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
            this.gameObject.GetComponent<TrailRenderer>().enabled = true;
            this.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        }

        transform.position = Vector3.MoveTowards(transform.position, positions[index], movementSpeed * Time.deltaTime);
        
        if (transform.position == positions[index]){

            index++;
        }
        if (index == positions.Length) index = 0;

        this.transform.GetChild(0).Rotate(xAngle, yAngle, zAngle, Space.Self);
        
        
    }

    void FixedUpdate(){
        positions = this.transform.parent.GetComponent<MeteorTelegraph>().GetPositions();
    }
    

    private void OnTriggerEnter(Collider other){

        if (other.gameObject.name == "Shield"){
    
            DestroyWithFanFare(other.gameObject);

        } else if (other.gameObject.name == "PlayerZone") {
            DestroyWithFanFare(other.gameObject);
        } else if (other.gameObject.name == "ProBuilder Mesh"){

            int randomNumber = Random.Range(7, 11);
            GameObject gameManager = GameObject.Find("GameManager");
            gameManager.GetComponent<GameManager>().InstatiatePrize(randomNumber, transform.position, other.gameObject);

            DestroyWithFanFare(other.gameObject);
        } 
        else if (other.CompareTag("Terrain"))
        {
            int randomNumber = Random.Range(4, 10);
            GameObject gameManager = GameObject.Find("GameManager");
            gameManager.GetComponent<GameManager>().InstatiatePrize(randomNumber, transform.position, other.gameObject);

            DestroyWithFanFare(other.gameObject);
        }
        }

    private IEnumerator TimedSelfDestruct(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
    }

    private void DestroyWithFanFare(GameObject other){
        
        AudioSource.PlayClipAtPoint(destructionSound, this.transform.position);
        GameObject effect = Instantiate(crashEffect, transform.position, transform.rotation, other.transform);
        Destroy(effect, 1.0f);
        Destroy(this.gameObject);

    }

}
