using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class TreadmillGeneration : MonoBehaviour
{
    public static TreadmillGeneration instance = null;

    public SpeedBar speedBar;
    public int rowLength;
    public int columnLength;
    public int numberOfPlanes = 3;
    public int numberOfPlanesDrawn = 3;
    public int pathsPerPlane = 2;
    public int drawBatchSize = 5;

    public GameObject planeObject;
    public GameObject caveObject;
    public GameObject finishLine;
    public GameObject planeBarrier;
    public GameObject enemy;

    public GameObject test_obstacle;
    public GameObject test_food;
    public Material crevasseMat;
    public Transform deleteRowTransform;
    public Transform spawnRowTransform;
    public List<GameObject> obstacles = new List<GameObject>();

    public GameManager gameManager;
    
    [SerializeField] private float treadmillSpeed = 1.0f;
    [SerializeField] private float timePerEachPlane = 1.0f;
    private float maxSpeed = 10.0f;
    private float minSpeed = 1.0f;

    public List<GameObject> activePlanes = new List<GameObject>();

    private float timeRemainingInLevel = 0;
    private bool startTimer = false;

    private Queue<SpawnOnPlaneRequest> objectSpawnRequests = new Queue<SpawnOnPlaneRequest>();
    private bool isSpawningObject = false;

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

    // Start is called before the first frame update
    void Start()
    {
        speedBar.setMaxSpeed(treadmillSpeed);

        StartTreadmill(timePerEachPlane);

        spawnRowTransform.position = GetInitialSpawnPosition();

        if (ScoreKeeperScript.instance.level < 0)
            Player.instance.SetEasyMode(true);

    }

    public void StartTreadmill(float timePerPlane)
    {
        numberOfPlanes = numberOfPlanes + ScoreKeeperScript.instance.level;
        for (int i = 0; i < numberOfPlanes + 2; i++)
        {
            if (i == 0)
                GeneratePlane(isFirstPlane: true);
            else if (i == numberOfPlanes - 1)
                GeneratePlane(isFinalPlane: true);
            else
                GeneratePlane();
        }

        SetTimeForLevel(timePerPlane * numberOfPlanes - (ScoreKeeperScript.instance.level * 0.2f * numberOfPlanes));
        startTimer = true;
    }

    private void Update()
    {
        if (startTimer)
            timeRemainingInLevel -= Time.deltaTime;

        if (timeRemainingInLevel <= 0)
        {
            TriggerEndOfWorld();
            timeRemainingInLevel = 0;
        }
            
    }

    private void TriggerEndOfWorld()
    {
        FindObjectOfType<MeteorSpawner>().ChangeSpawnRate(0.01f);
    }

    public void RequestSpawnObject(SpawnOnPlaneRequest request)
    {
        objectSpawnRequests.Enqueue(request);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isSpawningObject)
        {
            StartCoroutine("SpawnObstacle");
        }
        MoveTreadMill();
    }

    private Vector3 GetInitialSpawnPosition()
    {
        return (transform.position + (transform.right * (rowLength - 1))) + (transform.forward * columnLength) + transform.up;
    }

    public float GetTimeRemaining()
    {
        return timeRemainingInLevel;
    }

    public void SetTimeForLevel(float time)
    {
        timeRemainingInLevel = time;
    }

    public void AddTime(float time)
    {
        timeRemainingInLevel += time;
    }
    public void MoveTreadMill()
    {
        float adjustedTreadmillSpeed = treadmillSpeed;
        transform.Translate(-Vector3.forward * adjustedTreadmillSpeed * Time.deltaTime);
        int planesRemoved = 0;

        for (int i = 0; i < activePlanes.Count; i++)
        {
            var plane = activePlanes[i];
            if (Player.instance == null)
                return;
            if (plane.activeInHierarchy && (plane.transform.position.z < -22f || Player.instance.transform.position.z > plane.GetComponent<PlaneParent>().tipPosition.position.z + 7))
            {
                planesRemoved++;
                plane.SetActive(false);
                try
                {
                    var futurePlane = activePlanes[i + 3];
                    if (!futurePlane.activeInHierarchy)
                        futurePlane.SetActive(true);
                }
                catch { }
            }
        }

    }

    [ContextMenu("Spawn Plane")]
    public void DebugSpawnPlane()
    {
        GeneratePlane();
    }

    public void GeneratePlane(bool isFirstPlane = false, bool isFinalPlane=false)
    {
        var plane = Instantiate(planeObject);
        plane.transform.parent = transform;

        if (isFirstPlane)
        {
            plane.GetComponentInParent<PlaneParent>().SetIsFirst(true);
        }
        if (activePlanes.Count > 0)
        {
            plane.transform.position = activePlanes[activePlanes.Count - 1].transform.position + transform.forward * rowLength * 2;
            if (isFinalPlane)
            {
                plane.GetComponentInParent<PlaneParent>().SetIsLast(true);
                var lastPlane = plane;
                var cave = Instantiate(caveObject, lastPlane.transform);
                lastPlane.transform.position = transform.position + transform.forward * rowLength * 2 * (numberOfPlanes + 1) + Vector3.up;
                // GameObject finishLineObject = Instantiate(finishLine, cave.transform);
                Player.instance.transform.parent = plane.transform;
            }
        }
        plane.GetComponentInParent<PlaneParent>().SetReadyToDraw(true);

        if (!isFinalPlane)
            activePlanes.Add(plane.gameObject);

        if (activePlanes.Count > numberOfPlanesDrawn && !isFinalPlane && !isFirstPlane)
            plane.SetActive(false);

    }

    private IEnumerator SpawnObstacle()
    {
        isSpawningObject = true;

        while (objectSpawnRequests.Count > 0)
        {
            for (int i = 0; i < drawBatchSize; i++)
            {
                if (objectSpawnRequests.Count > 0)
                {
                    var request = objectSpawnRequests.Dequeue();
                    var obstacle = Instantiate(request.objectToSpawn, request.plane);
                    obstacle.transform.localPosition = request.positionToSpawn;
                }
            }
            
            yield return null;
        }

        isSpawningObject = false;
    }

    public void ChangeTreadmillSpeed(float amount)
    {
        if (((treadmillSpeed + amount) < minSpeed) || treadmillSpeed < minSpeed){
            treadmillSpeed = minSpeed;
        } else if( (treadmillSpeed + amount) > maxSpeed) {
            treadmillSpeed = maxSpeed;
        } else {
            treadmillSpeed += amount;     
            speedBar.SetSpeed(treadmillSpeed);    
        }
       
    }


    /*public rowData GenerateRow(Vector3 pos, BlockContainer biome, int col, planeSlot[,] planeMap, bool init=false)
    {
        List<ProBuilderMesh> meshesToCombine = new List<ProBuilderMesh>();
        List<GameObject> subColliders = new List<GameObject>();
        rowData data = new rowData(meshesToCombine, subColliders);

        for (int i = 0; i < rowLength; i++)
        {
            // var block = Instantiate(biome.GetBlock()).GetComponent<ProBuilderMesh>();
            var block = Instantiate(GetBlock(planeMap, i, col, biome)).GetComponent<ProBuilderMesh>();
            var blockBehaviour = block.GetComponent<BlockBehaviour>();
            block.transform.position = pos + blockBehaviour.spawnOffset;
            GameObject collider = blockBehaviour.SpawnCollider(pos);
            if (collider != null)
                data.subColliders.Add(collider);
            pos += Vector3.right;
            // meshesToCombine.Add(block);
            data.meshesToCombine.Add(block);
            Destroy(block.gameObject);
        }

        return data;
    }

    private GameObject GetBlock(planeSlot[,] map, int x, int y, BlockContainer biome)
    {
        if (map[x, y].icon == "O" || map[x, y].icon == "X")
            return biome.GetBlock();
        else if (map[x, y].icon == "C")
        {
            return crevasseBlock;
        }
        else if (map[x, y].icon == "T")
            return tarBlock;
        else
        {
            return pathBlock;
        }
    }*/

    public float GetTreadmillSpeed() { return treadmillSpeed; }

    public float GetTreadmillMaxSpeed() { return maxSpeed; }

    public float GetTreadmillMinSpeed() { return minSpeed; }

}

public struct rowData
{
    public List<ProBuilderMesh> meshesToCombine;
    public List<GameObject> subColliders;

    public rowData(List<ProBuilderMesh> meshesToCombine, List<GameObject> subColliders)
    {
        this.meshesToCombine = new List<ProBuilderMesh>();
        this.subColliders = new List<GameObject>();
    }
}

public class SpawnOnPlaneRequest
{
    public GameObject objectToSpawn;
    public Vector3 positionToSpawn;
    public Transform plane;

    public SpawnOnPlaneRequest(GameObject objectToSpawn, Vector3 positionToSpawn, Transform plane)
    {
        this.objectToSpawn = objectToSpawn;
        this.positionToSpawn = positionToSpawn;
        this.plane = plane;
    }
}


