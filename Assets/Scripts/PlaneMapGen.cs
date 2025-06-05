using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PlaneMapGen
{
    static System.Random rand = new System.Random();

    //private static Queue<int> previousXcoords = new Queue<int>();
    private static int[] previousXCoords = new int[TreadmillGeneration.instance.pathsPerPlane];

    static planeSlot path = new planeSlot(icon: "P", 0);
    static planeSlot terrain = new planeSlot(icon: "O", 1);
    static planeSlot crevasse = new planeSlot(icon: "C", 2);
    static planeSlot tar = new planeSlot(icon: "T", 3);
    static planeSlot boulder = new planeSlot(icon: "B", 99);

    public static planeSlot[,] GenerateMap(int rowSize, int colSize, int numPaths, int numberOfPools, bool noLava=false, bool isFirst=false, bool isLast=false)
    {
        planeSlot[,] plane = new planeSlot[rowSize, colSize];
        // Debug.Log($"Generating plane map with row: {rowSize}, col: {colSize}");

        for (int r = 0; r < rowSize; r++)
        {
            for (int c = 0; c < colSize; c++)
            {
                int i = rand.Next(0, 2);

                plane[r, c] = terrain;
            }
        }

        GeneratePath(rowSize, colSize, plane, numPaths);

        if (!noLava)
        {
            numberOfPools += ScoreKeeperScript.instance.level;

            for (int i = 0; i < numberOfPools; i++)
            {
                int n = rand.Next(0, 2);
                if (n == 1)
                    DrawCrevasse(plane, rowSize, colSize, crevasse, isFirst, isLast);
                else
                    DrawCrevasse(plane, rowSize, colSize, tar, isFirst, isLast);
            }
        }
        
        //PlaceObstacles(rand.Next(1, 4), biome, rowSize, colSize, plane);
        //DrawArray(plane);

        return plane;
    }

    static planeSlot[,] GeneratePath(int rowSize, int colSize, planeSlot[,] plane, int numPaths)
    {
        for (int p = 0; p < numPaths; p++)
        {
            int x = previousXCoords[p];
            int y = 0;

            int random = rand.Next(0, rowSize);
            x = random;
            plane[x, 0] = path;

            for (int i = 1; i < colSize; i++)
            {
                // from the last P, move down and mark the new position as P
                int x_move = 0;
                int dir = rand.Next(0, 3);
                if (dir == 0 && x - 1 >= 0)
                {
                    // move left
                    x_move -= 1;
                }
                else if (dir == 1 && x + 1 < rowSize)
                {
                    // move right
                    x_move += 1;
                }

                y = i;
                plane[x + x_move, y] = path;

                x += x_move;
            }

            previousXCoords[p] = x;
            // DrawArray(plane);
        }

        return plane;
    }

    static void DrawCrevasse(planeSlot[,] plane, int rowSize, int colLength, planeSlot crevasseType, bool isFirstPlane=false, bool isLastPlane=false)
    {
        int crevasseLength = 2;
        if (crevasseType.icon == tar.icon)
        {
            if (crevasseLength + ScoreKeeperScript.instance.level <= 10)
                crevasseLength += ScoreKeeperScript.instance.level;
            if (crevasseLength <= 0)
                return;
        }
        // Get a random position for the crevasse's initial point
        bool gotPoint = false;
        int randX = 0;
        int randY = 0;
        int startY = 0;

        if (isFirstPlane)
        {
            startY = Mathf.RoundToInt(colLength / 2);
        }

        while (!gotPoint)
        {
            randX = rand.Next(0, rowSize);
            randY = rand.Next(startY, colLength);

            if (plane[randX, randY].icon != "P")
                gotPoint = true;
        }

        plane[randX, randY] = crevasseType;

        for (int i = 0; i < crevasseLength; i++)
        {
            int dir = rand.Next(0, 4);
            if (dir == 0 && randX+1 < rowSize)
            {
                // go right
                randX += 1;
                plane[randX, randY] = crevasseType;
            }
            else if (dir == 1 && randY+1 < colLength)
            {
                // go up
                randY += 1;
                plane[randX, randY] = crevasseType;
            }
            else if (dir == 2 && randX-1 >= 0)
            {
                // go left
                randX -= 1;
                plane[randX, randY] = crevasseType;
            }
            else if (dir == 3 && randY-1 >= 0)
            {
                // go down
                randY -= 1;
                plane[randX, randY] = crevasseType;
            }
        }
    }

    static void DrawArray(planeSlot[,] plane)
    {
        for (int r = 0; r < 20; r++)
        {
            var rowString = "";
            for (int c = 0; c < 10; c++)
            {
                rowString += " " + plane[c, r].icon;
                //Console.Write(plane[c, r].icon);
            }

            Debug.Log(rowString);
            //Console.Write($"     row: {r}");
            //Console.WriteLine();
        }
    }



}
public struct planeSlot
{
    public string icon;
    public int meshIndex;

    public planeSlot(string icon, int index)
    {
        this.icon = icon;
        meshIndex = index;
    }

}
