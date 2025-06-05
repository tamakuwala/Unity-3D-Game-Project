using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeteorTelegraph : MonoBehaviour
{
    private GameObject treadmill;
    
    [SerializeField] private GameObject _projectile;
    [SerializeField] private LineRenderer _lineRenderer;
    

    [SerializeField] [Range(0.01f, 0.25f)] private float TimeBetweenPoints = 0.1f;
    [SerializeField] [Range(10, 100)] private int LinePoints = 25;



    private Vector3 spawnPosition;
    private Vector3 force;
    private Vector3 velocity;
    private float spawnTimer;
    [SerializeField] private GameObject _telegraphObject;
    [SerializeField] [Range(0.1f, 10f)] private float spawnRate = 0.10f;
    [SerializeField] private int _meteorSpawnDelay;


    private Vector3[] positions = new Vector3[300];
  
    private int teleGraphCount = 0;
    private Vector3 scaleChange;

    private GameObject finishLine;
    private Vector3 finishLinePosition;
    private Vector3 startingPosition;
    private float currentZPosition;
    private bool spawning;
    LineRenderer linerenderer;

    private void Awake() {
        spawning = false;


    }

    

    void Start(){
        finishLine = GameObject.Find("FinishLine"); 
        finishLinePosition = finishLine.transform.position; 
        startingPosition = finishLine.transform.position; 

        spawnPosition = this.transform.position;
        currentZPosition = spawnPosition.z;
        force = new Vector3(-300, 1000, 0);
        velocity = (force / _projectile.GetComponent<Rigidbody>().mass) * Time.fixedDeltaTime;
        
        linerenderer = Instantiate(_lineRenderer, spawnPosition, transform.rotation, this.transform);
        DrawProjection();

        StartCoroutine(DelayedLaunch());
        Destroy(this.gameObject, 7.0f);

    }

    void Update(){
        

        if (Time.time > spawnTimer && teleGraphCount < 10 && !spawning){
            
            
            GameObject clone = Instantiate(_telegraphObject, positions[0], Quaternion.identity, linerenderer.transform);
            spawnTimer = Time.time + spawnRate;
            clone.name = "TelegraphObject"+teleGraphCount;
            teleGraphCount++;
            

        }
        
    }
    private void FixedUpdate() {
        finishLinePosition = finishLine.transform.position; 
        Vector3 positionDifference = finishLinePosition - startingPosition;
        currentZPosition += positionDifference.z;
        startingPosition = finishLinePosition;

        this.transform.Translate(positionDifference * Time.deltaTime);
        spawnPosition = this.transform.position;
        UpdatePositions(currentZPosition);


    }

    private void UpdatePositions(float change){
        for (int i = 0; i < positions.Length; i++){
            positions[i].z = change;
        }
    }
    
    IEnumerator DelayedLaunch(){

        StartCoroutine(StopSpawningTelegraph());
        yield return new WaitForSeconds(_meteorSpawnDelay);
        Instantiate(_projectile, positions[0], Quaternion.identity, this.transform);
    }
    IEnumerator StopSpawningTelegraph(){
        yield return new WaitForSeconds(_meteorSpawnDelay-0.5f);
        spawning = true;
    }

    public Vector3[] GetPositions(){
        return positions;
    }

    public void Initialize(Vector3 initalForce, Vector3 initialPosition){
        force = initalForce;
        spawnPosition = initialPosition;
    }


    private void DrawProjection()
    {
        
        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints) + 1;
        Vector3 startPosition = spawnPosition;
        Vector3 startVelocity = velocity;
        int i = 0;
        _lineRenderer.SetPosition(i, startPosition);

        for (float time = 0; time < LinePoints; time += TimeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            _lineRenderer.SetPosition(i, point);

        }
        _lineRenderer.GetPositions(positions);
    }
}