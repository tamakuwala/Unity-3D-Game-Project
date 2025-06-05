using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private float lifeTime = 500.0f;
    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private float collectionSpeed = 5.0f;

    private AudioSource source;
    [SerializeField] private AudioClip destructionSound;


	void Start() {

        transform.position = new Vector3(transform.position.x, transform.position.y , transform.position.z);
        source = this.gameObject.transform.GetComponent<AudioSource>();
        
        
	}


    // Update is called once per frame
    void Update()
    {
        RotateThis();

    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "TractorBeam"){
            Movement();
        }
    }
    private void OnTriggerEnter(Collider other) {
       if (other.tag == "Player"){
        AudioSource.PlayClipAtPoint(destructionSound, this.transform.position);
        //Destroy(destructionSound.gameObject, destructionSound.clip.length);
        Destroy(this.gameObject); 
        GameManager.instance.PowerupGained(transform.name);

       }
    }

    private void Movement(){
        if (GameManager.instance.player) { //Null reference check
            transform.LookAt(GameManager.instance.player.transform.position); //Aim at player
            transform.position += transform.forward * collectionSpeed * Time.deltaTime;
        }
    }

        private void RotateThis() {
        transform.Rotate(Vector3.one * rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector2.one * rotationSpeed * Time.deltaTime);

        Destroy(this.gameObject, lifeTime);
    }
}
