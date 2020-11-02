using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MapGenerate : MonoBehaviour
{
    public int mapSizeX, mapSizeY;
    public GameObject centerTile;
    public GameObject[] edgeTile;
    public GameObject corner;
    public GameObject obstacle;
    public int obsCount;

    private bool[,] obstacleMap;

    // Start is called before the first frame update
    void Start()
    {
        mapSizeX -= 4;
        mapSizeY -= 4;
        for (int i = 1; i <= mapSizeX; i++)
        {
            for (int j = 1; j <= mapSizeY; j++)
            {
                Instantiate(centerTile, Coord2Pos(i, j), Quaternion.Euler(90, 0, 0), transform);
            }
        }

        for (int i = 1; i <= mapSizeX; i++)
        {
            Instantiate(edgeTile[Random.Range(0,edgeTile.Length)], Coord2Pos(i, 0), Quaternion.Euler(90, 0, -90), transform);
            Instantiate(edgeTile[Random.Range(0, edgeTile.Length)], Coord2Pos(i, mapSizeY + 1), Quaternion.Euler(90, 0, 90), transform);
        }

        for (int j = 1; j <= mapSizeY; j++)
        {
            Instantiate(edgeTile[Random.Range(0, edgeTile.Length)], Coord2Pos(0, j), Quaternion.Euler(90, 0, 180), transform);
            Instantiate(edgeTile[Random.Range(0, edgeTile.Length)], Coord2Pos(mapSizeX + 1, j), Quaternion.Euler(90, 0, 0), transform);
        }

        Instantiate(corner, new Vector3(-1, 0, mapSizeY + 1), Quaternion.Euler(0, 180, 0), transform);
        Instantiate(corner, new Vector3(-1, 0, -1), Quaternion.Euler(0, 90, 0), transform);
        Instantiate(corner, new Vector3(mapSizeX + 1, 0, -1), Quaternion.Euler(0, 0, 0), transform);
        Instantiate(corner, new Vector3(mapSizeX + 1, 0, mapSizeY + 1), Quaternion.Euler(0, -90, 0), transform);

        transform.Translate(new Vector3(2, 0, 2));

        mapSizeX += 4;
        mapSizeY += 4;

        obstacleMap = GenObstacles(obsCount);

        this.GetComponent<BoxCollider>().center = new Vector3(mapSizeX / 2 - 2, -0.005f, mapSizeY / 2 - 2);
        this.GetComponent<BoxCollider>().size = new Vector3(mapSizeX, 0.01f, mapSizeY);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 Coord2Pos(int x, int y)
    {
        return new Vector3(x - 0.5f, 0, y - 0.5f);
    }

    public bool MapIsFullyAccessible(bool[,] _obstacleMap, int _currentObsCount)
    {
        bool[,] mapFlags = new bool[mapSizeX,mapSizeY];
        Queue<Vector2> queue = new Queue<Vector2>();
        queue.Enqueue(new Vector2(1,1));
        mapFlags[1, 1] = true;

        int accessibleTileCount = 1;

        while (queue.Count > 0)
        {
            Vector2 currentTile = queue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neightbourX = (int)(currentTile.x + x);
                    int neightbourY = (int)(currentTile.y + y);

                    if (x == 0 || y == 0)
                    {
                        if (neightbourX >= 0 && neightbourX < mapSizeX && neightbourY >= 0 && neightbourY < mapSizeY)
                        {
                            if (mapFlags[neightbourX, neightbourY] == false && _obstacleMap[neightbourX, neightbourY] == false)
                            {
                                mapFlags[neightbourX, neightbourY] = true;
                                queue.Enqueue(new Vector2(neightbourX, neightbourY));

                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(mapSizeX * mapSizeY - _currentObsCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    public bool[,] GenObstacles(int obsCount)
    {
        int currentObsCount = 0;
        bool[,] obsMap = new bool[mapSizeX, mapSizeY];
        int max = mapSizeY * mapSizeX;
        List<int> obsPosList = new List<int>();
        obsPosList.Add(0);
        while (obsCount != 0)
        {
            int posIndex = Random.Range(0,max);
            if (!obsPosList.Contains(posIndex))
            {
                obsPosList.Add(posIndex);
                int x = posIndex % mapSizeX + 1;
                int y = posIndex / mapSizeY + 1;
                obsMap[x - 1, y - 1] = true;
                currentObsCount++;

                if (MapIsFullyAccessible(obsMap,currentObsCount))
                {
                    Instantiate(obstacle, Coord2Pos(x, y), Quaternion.Euler(0, 0, 0), transform);
                    obsCount--;
                }
                else
                {
                    obsMap[x - 1, y - 1] = false;
                    currentObsCount--;
                }
            }
        }
        return obsMap;
    }
}
