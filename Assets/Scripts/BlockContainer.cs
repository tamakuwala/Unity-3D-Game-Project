using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BlockContainer : ScriptableObject
{
    public string biomeName = "";
    public int terrainMeshIndex = 0;

    public List<GameObject> blocks;
    public List<GameObject> obstacles;
    public List<GameObject> foodObjects;

    public List<GameObject> blockColliders;

    public GameObject GetBlock()
    {
        int rand = Random.Range(0, 10);

        if (blocks.Count == 2)
        {
            if (rand < 9)
                return blocks[0];
            else
                return blocks[1];
        }

        else
            return blocks[0];
        
    }

    public int GetBlockMeshIndex()
    {
        return terrainMeshIndex;
    }

    public GameObject GetObstacle()
    {
        int i = Random.Range(0, obstacles.Count);
        return obstacles[i];
    }

    public GameObject GetFoodObject()
    {
        int i = Random.Range(0, 100);
        if (i < 95)
            return foodObjects[0];
        else
            return foodObjects[1];
    }

    public GameObject GetCollider(string icon)
    {
        if (icon == "C")
            return blockColliders[0];
        else if (icon == "T")
            return blockColliders[1];
        else
            return null;
    }
}

