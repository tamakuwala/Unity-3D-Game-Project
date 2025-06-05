using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float timeAdded = 5f;
    public float speedImpact;
    public GameObject treadmill;
    private float speed;
    [SerializeField] private float moveSpeed = 5.0f;
    public GameObject deathEffect;
    public float liftHeight;
    private bool isGrabbed = false;
    private bool boulderFound = false;
    private AudioSource source;
    [SerializeField] private AudioClip deathSound;
    private float hpIncrease = 25.0f;

    void Start(){
        speed = treadmill.GetComponent<TreadmillGeneration>().GetTreadmillSpeed();
    }
    // Update is called once per frame
    void Update()
    {
        
        if (isGrabbed){
            GrabbedMovement();
            
        } else {
            speed = treadmill.GetComponent<TreadmillGeneration>().GetTreadmillSpeed();
            MovePlayer();
        }
        
    }

    private void MovePlayer() {
        if(boulderFound){
            transform.Translate(Vector3.forward*moveSpeed*Time.deltaTime);
        }
        else{
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime); 
        }
        
    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "Player"){
            //Death Effect
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1.0f); 
            Destroy(this.gameObject);

            //Audio Play
            source = this.gameObject.transform.GetComponent<AudioSource>();
            AudioSource.PlayClipAtPoint(deathSound, transform.position);

            //Increase survival time
            TreadmillGeneration.instance.AddTime(timeAdded); 
            //Increaseplayer speed
            TreadmillGeneration.instance.ChangeTreadmillSpeed(speedImpact);
            //Increase player health
            Player.instance.GetComponent<Player>().IncreaseHealth(hpIncrease);

            //popup message
            //Player.instance.GetComponent<Player>().ShowPopupmessage("+25 HP\n+5sec");
            ScoreKeeperScript.instance.AddScore(5);
        }

        //checking for stones for pathfinding.
        if (other.transform.tag == "Boulder"){
            boulderFound = true;
        }   

        if (other.tag == "Hand"){
            
            // Grab him! 
            transform.Rotate(new Vector3(-90, 0, 0));
            
        }

        //Destroy wolves when collide with boundary.
        if (other.tag == "PlaneBarrier"){ 
            Destroy(this.gameObject);
        } 
    }

    private void OnTriggerStay(Collider other){
        if (other.tag == "Hand"){
            // Finish him!
           
        }
        
        //Checking for stones
        if (other.transform.tag == "Boulder"){
            boulderFound = true;
        }

        else if (other.tag == "HandSphereOfInfluence"){

        }
    }

    private void OnTriggerExit(Collider other)
    {   
        //checking for stones
        if (other.CompareTag("Boulder")){
            boulderFound = false;
        }   
    }

    public void EnemyIsGrabbed(bool boo){
        isGrabbed = boo;
    }

    public void GrabbedMovement(){
                            
        if (GameObject.FindGameObjectWithTag("Player")!=null){
            transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform); //Aim at target
            transform.position += transform.forward *  (moveSpeed/2) * Time.deltaTime;
        }
    }

}
