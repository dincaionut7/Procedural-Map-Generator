using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class mapGenerator : MonoBehaviour
{
    private int[,] map;


    [Range(0, 100)]
    public int fillAmmount;
    public int height;
    public int width;

    public string seed;
    public bool randomSeed;

    public Tilemap floorLayer;
    public Tilemap collisionLayer;
    public Tile floorTile;
    public Tile wallTile;

    public GameObject pickupObj;


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            floorLayer.ClearAllTiles();
            collisionLayer.ClearAllTiles();

            generateRandomMap();
            drawTiles();
    
        }

        if (Input.GetMouseButton(1))
        {
            floorLayer.ClearAllTiles();
            collisionLayer.ClearAllTiles();

            map = dynamicSmoothMap(map);           
            drawDynamicTiles(map);       
        } 

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            int[,] freeSpots = getFreeTiles(map);
            spawnItem(freeSpots);
        }
    }

    

    void generateRandomMap()
    {
        
        
        map = new int[width, height];

        if (randomSeed)
        {
            seed = Random.Range(1, 201650).ToString();
        }

        System.Random rng = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
            for (int y = 0; y < width; y++)
            {
                map[x, y] = rng.Next(0, 100) < fillAmmount ? 1 : 0;
            }
    }

    int[,] dynamicSmoothMap(int[,] map)
    {
        int[,] smoothedMap = map;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                int wallCount = getSurroundingWallCount(x, y);

                if (wallCount > 4)
                    smoothedMap[x, y] = 1;
                else if (wallCount < 4)
                    smoothedMap[x, y] = 0;
            }
        }
        return smoothedMap;

    }



    int getSurroundingWallCount(int posX, int posY)
    {
        int wallCount = 0;
        for (int neighbourX = posX - 1; neighbourX <= posX + 1; neighbourX++)
        {
            for (int neighbourY = posY - 1; neighbourY <= posY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != posX || neighbourY != posY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    protected int[,] getFreeTiles(int[,] map)
    {
        int[,] tempMap = new int[width,height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                tempMap[x, y] = 0;
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                if (map[x, y] == 0)
                {
                    tempMap[x, y] = 1;
                }
            }
        }

        return tempMap;
    }



    protected void spawnItem(int[,] openMap)
    {
        int randomX = Random.Range(0, openMap.GetLength(0));
        int randomY = Random.Range(0, openMap.GetLength(1));

        if (openMap[randomX, randomY] != 1)
        {
            spawnItem(openMap);
        }
        else
        {
            Instantiate(pickupObj, floorLayer.GetCellCenterWorld(new Vector3Int(randomX, randomY, 0)), Quaternion.identity);
        }
    }

    void drawDynamicTiles(int[,] map)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                if (map[x, y] == 1)
                {
                    floorLayer.SetTile(new Vector3Int(x, y, 0), floorTile);
                    collisionLayer.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
                else if (map[x, y] == 0)
                {
                    floorLayer.SetTile(new Vector3Int(x, y, 0), floorTile);
                }


            }
        }
    }

    void drawTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                if (map[x, y] == 1)
                {
                    floorLayer.SetTile(new Vector3Int(x, y, 0), floorTile);

                    collisionLayer.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
                else if (map[x, y] == 0)
                {
                    floorLayer.SetTile(new Vector3Int(x, y, 0), floorTile);
                }


            }
        }
    }




    /*
    void smoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                int nWallCount = getSurroundingWallCount(x, y);

                if (nWallCount > 4)
                    map[x, y] = 1;
                else if (nWallCount < 4)
                    map[x, y] = 0;
            }
        }
    }
    */
}
