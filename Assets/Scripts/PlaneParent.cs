using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneParent : MonoBehaviour
{
    public BlockContainer[] biomes;
    public Transform tipPosition;

    private bool isFirst = false;
    private bool isLast = false;
    private BlockContainer biome;
    private bool readyToDraw = false;
    // Start is called before the first frame update
    void Awake()
    {
        int rand = Random.Range(0, biomes.Length);
        biome = biomes[rand];
    }

    public BlockContainer GetBiome()
    {
        return biome;
    }

    public void SetReadyToDraw(bool state)
    {
        readyToDraw = state;
    }

    public bool ReadyToDraw()
    {
        return readyToDraw;
    }

    public void SetIsFirst(bool state)
    {
        isFirst = state;
        SetReadyToDraw(true);
    }

    public bool GetIsFirst() { return isFirst; }
    public bool GetIsLast() { return isLast; }

    public void SetIsLast(bool state)
    {
        isLast = state;
        SetReadyToDraw(true);
    }
}
