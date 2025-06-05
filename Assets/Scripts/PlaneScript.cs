using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class PlaneScript : MonoBehaviour
{
    public int numberOfPools = 1;

    public Material material;

    // private planeSlot[] planeMap;
    private List<planeSlot> planeMap;
    private ProBuilderMesh planeMesh;
    private MeshRenderer meshRenderer;
    private BlockContainer biome;
    private PlaneParent parent;

    private bool planeDrawn = false;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        parent = GetComponentInParent<PlaneParent>();
        biome = parent.GetBiome();

        if (biome.biomeName == "Desert")
        {
            // Debug.Log("Disabling material");
            meshRenderer.materials[5] = null;
        }
        // DrawMesh();
    }

    private void Update()
    {
        if (!planeDrawn && parent.ReadyToDraw())
        {
            planeDrawn = true;
            DrawMesh();
        }
    }

    private void DrawMesh()
    {
        planeMap = new List<planeSlot>();
        planeMesh = GetComponent<ProBuilderMesh>();

        int col = Mathf.Abs((int)meshRenderer.bounds.size.x); // 15
        int row = Mathf.Abs((int)meshRenderer.bounds.size.z); // 30

        var map = PlaneMapGen.GenerateMap(col, row, 2, numberOfPools, isFirst:parent.GetIsFirst(), isLast:parent.GetIsLast());

        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < col; y++)
            {
                planeMap.Add(map[y, x]);
            }
        }

        for (int i = 0; i < planeMesh.faceCount; i++)
        {
            // Get the planeMap
            // Get the tile type

            var slot = planeMap[i];
            // Debug.Log($"Position: {planeMesh.positions[i]}");

            if (slot.icon == "O") // Terrain
            {
                planeMesh.faces[i].submeshIndex = biome.GetBlockMeshIndex();
                CheckSpawnObject(planeMesh.positions[i * 4]);
            }
            else if (slot.icon == "C") // Lava
            {
                planeMesh.faces[i].submeshIndex = 2;
                GameObject temp = Instantiate(biome.GetCollider("C"));
                temp.transform.parent = transform;
                temp.transform.localPosition = planeMesh.positions[i * 4];
                temp.transform.localPosition -= Vector3.forward;
            }
            else if (slot.icon == "T") // Tar
            {
                planeMesh.faces[i].submeshIndex = 3;
                GameObject temp = Instantiate(biome.GetCollider("T"));
                temp.transform.parent = transform;
                temp.transform.localPosition = planeMesh.positions[i * 4];
                temp.transform.localPosition -= Vector3.forward;
            }
            else
                planeMesh.faces[i].submeshIndex = planeMap[i].meshIndex;

            // Set corresponding tile material based on the biome
            // Spawn any necessary colliders
        }

        planeMesh.ToMesh();
        planeMesh.Refresh();

        // Debug.Log($"number of faces: {planeMesh.faceCount}, number of positions: {planeMesh.positions.Count}");
    }

    private void CheckSpawnObject(Vector3 pos)
    {

        int rand = Random.Range(0, 100);

        if (rand >= 95 - ScoreKeeperScript.instance.level)
        {
            if (parent.GetIsFirst()) // Avoid spawning obstacles on the first plane
                return;
            var obstacle = biome.GetObstacle();
            SpawnOnPlaneRequest request = new SpawnOnPlaneRequest(obstacle, pos, transform);
            TreadmillGeneration.instance.RequestSpawnObject(request);
        }
        else if (rand <= 5)
        {
            var obstacle = biome.GetFoodObject();
            SpawnOnPlaneRequest request = new SpawnOnPlaneRequest(obstacle, pos, transform);
            TreadmillGeneration.instance.RequestSpawnObject(request);
        }
    }

}
