using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private GameObject LevelCompleteScreen;
    [SerializeField] private Text distanceText;
    [SerializeField] private GameObject countDownScreen;
    [SerializeField] private GameObject PauseScreen;
    private Text countDownText;
    public TMP_Text LivesText;
    public TMP_Text LifeText;
    public static bool Pausegame;
    private Text levelText;


    public static GameManager instance = null;
    private Vector3 lastPlayerPosition;



    private float forwardProgress =0;
    private Queue<float> collectionOfVelocity = new Queue<float>();
    private int collectionSize = 0; // Size of rolling average;
    private int maxCollectionSize = 50;
    
    [SerializeField] GameObject endGameMeteor;
    private GameObject finishLine;

    [SerializeField] GameObject collectable1;
    [SerializeField] GameObject collectable2;
    [SerializeField] GameObject collectable3;
    [SerializeField] GameObject collectable4;
    private List<GameObject> collectables = new List<GameObject>();

    [SerializeField] GameObject powerUpGrabber;
    [SerializeField] GameObject powerUpShield;

    private Dictionary<string, GameObject> powerUps = new Dictionary<string, GameObject>();


    [SerializeField] public GameObject player;
    [SerializeField] private GameObject treadmill;

    private bool isGameOver = false;

    // This is for the time dilation powerup
    private float lastTimeMeasurement;
    private float maxTimeScale = 1.0f;
    private bool isTimeSlowedDown;
    private float timeAcceleration = 0.015f;
    private int countDownTime = 3;

    //For number of lives
    public int lives = 0;

    private AudioSource source;
    [SerializeField] AudioClip countdownNoise;
    [SerializeField] AudioClip countdownGoNoise;


    private void Awake(){
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        ShowGameOverScreen(false);
        ShowLevelCompleteScreen(false);
        source = GameObject.Find("Camera").GetComponent<AudioSource>();
        

    }
    // Start is called before the first frame update
    void Start()
    {
        countDownText = countDownScreen.transform.GetChild(0).GetComponent<Text>();
        // levelText = GameObject.FindGameObjectWithTag("Level_Text").GetComponent<Text>();
       
        
        collectables.Add(collectable1);
        collectables.Add(collectable2);
        collectables.Add(collectable3);
        collectables.Add(collectable4);

        powerUps.Add("HandPickup(Clone)", powerUpGrabber);
        powerUps.Add("ShieldPickup(Clone)", powerUpShield);
	
        finishLine = GameObject.Find("FinishLine(Clone)");
        if (finishLine == null)
            finishLine = GameObject.FindGameObjectWithTag("Finish_Line");


        if (GameObject.FindGameObjectWithTag("Player") != null){
            lastPlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        
        }
        lastTimeMeasurement = Time.realtimeSinceStartup;
        
        StartCoroutine(CountDownToStart());

    }

    private void Update()
    {
        LivesText.text = $"Lives: {lives}";
    }

    IEnumerator CountDownToStart(){
        Time.timeScale = 0;
        countDownScreen.gameObject.SetActive(true);
        
        // levelText.text = "LEVEL " + Loader.GetLevel() + " STARTING IN" ;
        while(countDownTime > 0){
            source.Play();
            countDownText.text = countDownTime.ToString();
            yield return new WaitForSecondsRealtime(1f);
            countDownTime--;
        }

        countDownScreen.GetComponent<RawImage>().color = new Color(25f, 35f,25f,0f);
        
        Time.timeScale =1;
        source.clip = countdownGoNoise;
        countDownText.text = "Go!";
        source.Play();
        yield return new WaitForSecondsRealtime(1f);
        countDownScreen.GetComponent<RawImage>().color = new Color(25f, 35f,25f,1f);
        countDownScreen.gameObject.SetActive(false);

        
    }


    void FixedUpdate()
    {
        if (finishLine == null)
            finishLine = GameObject.FindGameObjectWithTag("Finish_Line");

        if (!isGameOver){
        // This is for the time dilation pickup
        if (isTimeSlowedDown){
            var myDeltaTime = Time.realtimeSinceStartup - lastTimeMeasurement;
            lastTimeMeasurement = Time.realtimeSinceStartup;
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, maxTimeScale, timeAcceleration*myDeltaTime);
        }


            Vector3 currentPosition = Player.instance.transform.position;
            
            float treadmillSpeed = treadmill.GetComponent<TreadmillGeneration>().GetTreadmillSpeed();
            
        
            float positionChange = (currentPosition.z - lastPlayerPosition.z); 
            float gain;
            
            // 0.05 is holding player constantly forward, -0.01 is standing still, -0.05 is going backwards
            //Forward progress
            gain = (positionChange / 0.04f)
                    * (treadmill.GetComponent<TreadmillGeneration>().GetTreadmillMaxSpeed()
                    - treadmill.GetComponent<TreadmillGeneration>().GetTreadmillMinSpeed());
            
            AddToFloatingAverage(gain);
            
            //
            if (positionChange > -0.01f){
                //Forward progress
                IncreaseSpeed(GetFloatingAverage() / 2 );
                        
            } else if (positionChange <= -0.01f){
                IncreaseSpeed(GetFloatingAverage() / 2);
                
            } 
             
            GameObject[] meteors = GameObject.FindGameObjectsWithTag("Meteor");
            GameObject[] pickups = GameObject.FindGameObjectsWithTag("Collectable");
            
            //This moves all meteors in time with the treadmill
            foreach (GameObject meteor in meteors) { 
                meteor.transform.Translate(-Vector3.forward* treadmill.GetComponent<TreadmillGeneration>().GetTreadmillSpeed()* Time.deltaTime);
            }
            
            if (positionChange > -0.01 ){ 
                forwardProgress += positionChange; 
                }
            
            lastPlayerPosition = currentPosition;

            // Moving cave forward
            /*finishLine.transform.Translate(-Vector3.forward * treadmill.GetComponent<TreadmillGeneration>().GetTreadmillSpeed()* Time.deltaTime);
*/

            float distanceBetweenPlayerAndFinishLine = finishLine.transform.position.z - player.transform.position.z;
            if (distanceBetweenPlayerAndFinishLine > 1) {
                distanceText.text = Mathf.RoundToInt(distanceBetweenPlayerAndFinishLine).ToString() + "m to go!";
            } else{
                distanceText.text = "0m to go!";
            }
            // Move End Game Meteor in front of camera    
            if ((distanceBetweenPlayerAndFinishLine) < 50){  endGameMeteor.transform.Translate(Vector3.forward * Time.deltaTime);}
                
        } else {
            ShowGameOverScreen(true);
        }   
    }

    // Calculates a floating average to avoid rapid acceleration and decceleration
    private void AddToFloatingAverage(float newValue){
        collectionOfVelocity.Enqueue(newValue);
        collectionSize++;
        if (collectionSize >= maxCollectionSize){
            collectionOfVelocity.Dequeue();
            collectionSize--;
        }
        
    }

    private float GetFloatingAverage(){
        float sum = 0;
        int size = 0;
        foreach(float number in collectionOfVelocity){
            sum += number;
            size++;
        }
        return sum/size;
    }

    // Increase treadmill speed
    public void IncreaseSpeed(float amount){
        treadmill.GetComponent<TreadmillGeneration>().ChangeTreadmillSpeed(amount);
    }

    /**
    * This function receives the string name of the item collected by the player and instatiates the correct power up;
    */
    public void PowerupGained(string name){
        
        if (name=="HandPickup(Clone)"){
            Vector3 behindPlayer = new Vector3(player.transform.position.x + 2, player.transform.position.y, player.transform.position.z);
            Instantiate(powerUps["HandPickup(Clone)"], behindPlayer,new Quaternion(0,0,0,0), player.transform);

        } else if (name=="ShieldPickup(Clone)"){
            Instantiate(powerUps["ShieldPickup(Clone)"]);
            player.GetComponent<Player>().SetShield(true);
        } else if (name=="TimePickup(Clone)"){
            isTimeSlowedDown = true;
            Time.timeScale = 0.4f;
        }
        else if (name=="DiamondPickup(Clone)"){
            lives++;
            Debug.Log("Diamond pickup");
        }

    }

    public void InstatiatePrize(int number, Vector3 position, GameObject parent){
       
        switch (number){
            case (7):
                //Instantiate Prize 1 - Grabbing Hand
                Instantiate(collectables[0], position, new Quaternion(0,0,0,0), parent.transform);
                break;
            case (8):
                //Instantiate Prize 2 - Shield
                Instantiate(collectables[1], position, new Quaternion(0,0,0,0), parent.transform);
                break;
            case (9):
                //Instantiate Prize 3 - Time
                Instantiate(collectables[2], position, new Quaternion(0,0,0,0), parent.transform);
                break;
            case (10):
                //Instantiate Prize 4 - Life
                Instantiate(collectables[3], position, new Quaternion(0,0,0,0), parent.transform);
                break;
            default:
                break;

        }

    }
    
    
    public void ShowGameOverScreen(bool status){
        GameOverScreen.gameObject.SetActive(status);
    }

    public void ShowLevelCompleteScreen(bool status){

        ScoreKeeperScript.instance.CalculateScore();
       LevelCompleteScreen.SetActive(status);
    }

    public void SetGameOverStatus(bool status){ 
        isGameOver = status;
        Time.timeScale = maxTimeScale; // In case time powerup was used.
        ShowGameOverScreen(true);
    }


    public void ResetTheGame(){
       Loader.Reload(Loader.Scene.Main);
    }
    
    public void ExitGame(){
        Application.Quit();
    }

    public void LevelComplete(){
        Time.timeScale = 0f;
        ScoreKeeperScript.instance.level++;
        ShowLevelCompleteScreen(true);
    }

    public void Pause(){
        if(lives == 1){
            LifeText.text = $"You have {lives} life left. Click resume to continue.";
        }
        else{
            LifeText.text = $"You have {lives} lives left. Click resume to continue.";
        }   
        PauseScreen.SetActive(true);
        Time.timeScale = 0f;
        Pausegame = true;
    }

    public void Resume(){
        lives-- ; 
        PauseScreen.SetActive(false);
        Time.timeScale = 1f;
        Pausegame = false;
    }

    public void LevelUp(){
        Time.timeScale = maxTimeScale;
        Loader.NextLevel(Loader.Scene.Main);
    }

}
