using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance = null;

    public GameObject graphicsRoot;
    public float isSlowedModifier = 0.1f;
    public float sidewaysSpeedMod = 1.0f;
    public float moveSpeed = 1.0f;
    public float rotateSpeed = 360.06f;

    public float maxSpeed = 7f;
    public float acceleration = 1f;

    public bool stickToTreadmill = false;
    public bool isSlowed = false;
    public bool isBlocked = false;
    private bool isShielded = false;

    [SerializeField] public float health = 100.0f; 
    public HealthBar healthbar;
    public float maxHp = 100.0f;
    
    public GameManager gameManager;
    public GameObject[] boostParticleEffects;
    
    // Speed boost variables
    private float boostedSpeed = 1.8f;
    private int currentBoosts = 0;
    private int maxBoosts = 3;
    private float cooldownTime = 0.8f;
    private float nextTime;
    private float timeSlowed = 0f;

    private bool easyMode = false;
    public GameObject popupMessagePrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //controller = GetComponent<CharacterController>(); 
        healthbar.SetMaxHealth(health);
        nextTime = cooldownTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float trueMoveSpeed = moveSpeed + GetBoostedSpeed();
        SetSpeedParticle();

        if (nextTime > 0)
            nextTime -= Time.deltaTime;
        else
            RemoveSpeedBoost(removeAll: false);

        if (isSlowed)
        {
            timeSlowed += Time.deltaTime;
            trueMoveSpeed *= isSlowedModifier;
            RemoveSpeedBoost(removeAll: true);

            if (timeSlowed >= 1.3f)
            {
                timeSlowed = 0;
                isSlowed = false;
            }
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            if (moveSpeed < maxSpeed)
                moveSpeed += acceleration * Time.deltaTime;

            RotateGraphics(-Vector3.right);

            if (Input.GetKey(KeyCode.A) && !CheckForObstacle(new Vector3(-0.5f, 0, -0.5f)))
            {
                transform.Translate(-Vector3.right * trueMoveSpeed * sidewaysSpeedMod * Time.deltaTime);
                transform.Translate(Vector3.forward * Time.deltaTime);
                RotateGraphics(new Vector3(-0.5f, 0, -0.5f));
                stickToTreadmill = false;
            }
            else if (Input.GetKey(KeyCode.D) && !CheckForObstacle(new Vector3(0.5f, 0, 0.5f)))
            {
                transform.Translate(Vector3.right * trueMoveSpeed * sidewaysSpeedMod * Time.deltaTime);
                transform.Translate(Vector3.forward * Time.deltaTime);
                RotateGraphics(new Vector3(0.5f, 0, 0.5f));
                stickToTreadmill = false;
            }

            if (CheckForObstacle(Vector3.forward))
                stickToTreadmill = true;
            else
            {
                transform.Translate(Vector3.forward * trueMoveSpeed * Time.deltaTime);
                stickToTreadmill = false;
            }
            
        }
        else if (Input.GetKey(KeyCode.S))
        {
            RotateGraphics(Vector3.right);

            if (CheckForObstacle(Vector3.back))
                stickToTreadmill = true;
            else
            {
                transform.Translate(Vector3.back * trueMoveSpeed * Time.deltaTime);
                stickToTreadmill = true;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateGraphics(Vector3.forward);

            if (CheckForObstacle(Vector3.right))
                stickToTreadmill = true;
            else
            {
                transform.Translate(Vector3.right * trueMoveSpeed * sidewaysSpeedMod * Time.deltaTime);
                stickToTreadmill = true;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            RotateGraphics(-Vector3.forward);

            if (CheckForObstacle(-Vector3.right))
                stickToTreadmill = true;
            else
            {
                transform.Translate(-Vector3.right * trueMoveSpeed* sidewaysSpeedMod * Time.deltaTime);
                stickToTreadmill = true;
            }
        }
        else
        {
            stickToTreadmill = true;
            moveSpeed = 3;
        }
    }

    public void SetSpeedParticle()
    {
        if (currentBoosts == 0)
        {
            foreach (var paricle in boostParticleEffects) paricle.SetActive(false);
        }
        else if (currentBoosts == 1)
        {
            boostParticleEffects[0].SetActive(true);
            boostParticleEffects[1].SetActive(false);
            boostParticleEffects[2].SetActive(false);
        }
        else if (currentBoosts == 2)
        {
            boostParticleEffects[0].SetActive(false);
            boostParticleEffects[1].SetActive(true);
            boostParticleEffects[2].SetActive(false);
        }
        else if (currentBoosts >= 3)
        {
            boostParticleEffects[0].SetActive(false);
            boostParticleEffects[1].SetActive(false);
            boostParticleEffects[2].SetActive(true);
        }
    }

    private void RotateGraphics(Vector3 dir)
    {
        var lookRotation = Quaternion.LookRotation(dir, Vector3.up);
        graphicsRoot.transform.rotation = Quaternion.RotateTowards(graphicsRoot.transform.rotation, lookRotation, 720f * Time.deltaTime);
    }

    private bool CheckForObstacle(Vector3 direction)
    {
        int layerMask = 1 << 6;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, 0.6f, layerMask))
        {
            return true;
        }
        else
            return false;
    }

    public void SetEasyMode(bool state)
    {
        easyMode = state;
    }

    private float GetBoostedSpeed()
    {
        return currentBoosts * boostedSpeed;
    }

    public  void AddSpeedBoost(int n_boost)
    {
        int boosts = currentBoosts + n_boost;

        if (boosts > maxBoosts)
            currentBoosts = maxBoosts;
        else
            currentBoosts = boosts;

        // Reset cooldown
        nextTime = cooldownTime;

        // AudioManager.instance.Play("SpeedBoost");
        float pitch = 0;
        if (currentBoosts == 0)
            pitch = 1f;
        else if (currentBoosts == 1)
            pitch = 1.8f;
        else
            pitch = 3f;

        AudioManager.instance.PlayAndPitch("SpeedBoost", pitch);
    }

    private void RemoveSpeedBoost(bool removeAll)
    {
        // Debug.Log($"Removing speed boost: {currentBoosts}");

        if (removeAll)
            currentBoosts = 0;
        else
            currentBoosts--;

        if (currentBoosts < 0)
            currentBoosts = 0;

        nextTime = cooldownTime;
    }

    public int GetSpeedBoostCount()
    {
        return currentBoosts;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tar") && !isSlowed)
        {
            isSlowed = true;
            AudioManager.instance.Play("Squish");
        }
        //if (other.CompareTag("Boulder"))
        //    isBlocked = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tar"))
            isSlowed = false;
        //if (other.CompareTag("Boulder"))
        //    isBlocked = false;
    }

    private void OnTriggerEnter(Collider other){

        if (other.tag == "Meteor")
        {
            TakeDamage(30.0f);
        }
        else if (other.gameObject.name == "Delete_Zone")
        {

            checkForLives();

        }
        else if (other.gameObject.name == "FinishLine(Clone)" || other.gameObject.name == "FinishLine")
        {
            gameManager.LevelComplete();

        }
        
    }
    
    public void SetShield(bool boo){
        isShielded = boo;
    }

    public void KillPlayer(){

        if (easyMode)
        {
            easyMode = false;
            return;
        }



        this.gameObject.transform.Find("Camera").parent = null;
        gameManager.SetGameOverStatus(true);
        ScoreKeeperScript.instance.DecreaseLevel();
        ScoreKeeperScript.instance.ResetScore();
        Destroy(this.gameObject);
    }

    public void TakeDamage (float damage) {

        if(!isShielded){
            health -= damage; 
            healthbar.SetHealth(health);
        }
       
        ShowPopupmessage($"-{(int)damage}HP");

        if (health <= 0) {
            checkForLives();
        }
    }

    public void ShowPopupmessage(string message){
        string showmessage = message;

        var go =  Instantiate(popupMessagePrefab, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = showmessage;
    }

    public void IncreaseHealth (float hp) {

        if(health + hp <= maxHp){
            health += hp; 
            healthbar.SetHealth(health);
            ShowPopupmessage("+25 HP\n+5 sec");
        }
        else if(health + hp > maxHp){
            health = maxHp;
            healthbar.SetHealth(health);
            ShowPopupmessage("+5 sec");
        }
    }

    public void checkForLives(){
        if(gameManager.lives > 0){
            gameManager.Pause();
        }
        else{
            KillPlayer();
        }
    }

}


