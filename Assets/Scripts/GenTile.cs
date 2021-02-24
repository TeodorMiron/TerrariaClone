using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;


public class GenTile : MonoBehaviour
{
    
    [Range(0, 100)]
    public int iniChance;
    [Range(1, 8)]
    public int birthLimit;
    [Range(1, 8)]
    public int deathLimit;

    [Range(1, 20)]
    public int numR;


    private int[,] terrainMap;
    public Vector3Int tmpSize;
    public Tilemap topMap;
    public Tilemap botMap;
    public Tile topTile;
    public Tile botTile;
    public Tile grassTile;

    int width;
    int height;

    public void doSim(int nu)
    {
        
        clearMap(false);
        width = tmpSize.x;
        height = tmpSize.y;

        if (terrainMap == null)
        {
            terrainMap = new int[width, height];
            initPos();
        }


        for (int i = 0; i < nu; i++)
        {
            terrainMap = genTilePos(terrainMap);
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (terrainMap[x, y] == 1)
                { 
                    topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), topTile);
                    if (y <= 35 &&terrainMap[x, y - 1] == 0)
                    {
                        topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), grassTile);
                    }
                }
                botMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), botTile);

            }
        }


    }

    public void initPos()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 20; y < height; y++)
            {
                
                terrainMap[x, y] = Random.Range(1, 101) < iniChance ? 1 : 0;
                if (y == 35)
                { terrainMap[x, y] = Random.Range(1, 33) < iniChance ? 1 : 0; }
            }

        }


    }


    public int[,] genTilePos(int[,] oldMap)
    {
        int[,] newMap = new int[width, height];
        int neighb;
        BoundsInt myB = new BoundsInt(-1, -1, 0, 3, 3, 1);


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                neighb = 0;
                foreach (var b in myB.allPositionsWithin)
                {
                    if (b.x == 0 && b.y == 0) continue;
                    if (x + b.x >= 0 && x + b.x < width && y + b.y >= 0 && y + b.y < height)
                    {
                        neighb += oldMap[x + b.x, y + b.y];
                    }
                    else
                    if(y+b.y==height)
                    {
                        neighb++;
                    }
                        
                }

                if (oldMap[x, y] == 1)
                {
                    if (neighb < deathLimit) newMap[x, y] = 0;

                    else
                    {
                        newMap[x, y] = 1;

                    }
                }

                if (oldMap[x, y] == 0)
                {
                    if (neighb > birthLimit) newMap[x, y] = 1;

                    else
                    {
                        newMap[x, y] = 0;
                    }
                }

            }

        }



        return newMap;
    }
  public void clearMap(bool complete)
    {

        topMap.ClearAllTiles();
        botMap.ClearAllTiles();
        if (complete)
        {
            terrainMap = null;
        }


    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            doSim(numR);
        }

        if (Input.GetMouseButtonDown(1))
        {
            clearMap(true);
        }
    }

    void Start()
    {
        doSim(numR);
    }



  



}