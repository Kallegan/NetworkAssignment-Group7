using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private int worldWidth = 20;
    [SerializeField] private int worldHeight = 20;

    private readonly float xOffset = 1.8f;
    private readonly float zOffset = 1.565f;
    
    [SerializeField] private GameObject hexPrefab;

    void Start()
    {
        GenerateHexGrid();
    }

    private void GenerateHexGrid()
    {
        float gridXMin = -worldWidth / 2;
        float gridXMax = worldWidth / 2;
        
        float gridZMin = -worldHeight / 2;
        float gridZMax = worldHeight / 2;
        
        for (float x = gridXMin; x < gridXMax; x++)
        {
            for (float z = gridZMin; z < gridZMax; z++)
            {
                GameObject tempGameObject = Instantiate(hexPrefab);
                
                if (z % 2 == 0)
                {
                    tempGameObject.transform.position = new Vector3(x * xOffset, 0, z * zOffset);
                }
                else
                {
                    tempGameObject.transform.position = new Vector3(x * xOffset + xOffset / 2, 0, z * zOffset);
                }

                SetHexInfo(tempGameObject, x, z);
            }
        }
    }

    void SetHexInfo(GameObject tempGO, float x, float z)
    {
        tempGO.transform.parent = transform;
        tempGO.name = "Hex_" + x + "," + z;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hex"))
        {
            Destroy(other.gameObject);
        }
        
    }
}

/* cubegrid test
[SerializeField] private int rows = 10;
[SerializeField] private int cols = 10;
[SerializeField] private float gridSize = 1f;
private int totalGridSize;
private float gridShrinktimer = 1;

[SerializeField] private GameObject gridBlock;
private List<GameObject> gridBlockList = new();
void Start()    
{
    transform.position = new Vector3(cols / -2, 0, rows / 2); 
    GenerateGrid();
    totalGridSize = gridBlockList.Count;
}

private void Update()
{
    totalGridSize--;
    RemoveGridLayer(totalGridSize);
}
    
private void GenerateGrid()
{
    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        { 
            GameObject tile = Instantiate(gridBlock, transform);
            tile.name = "Tile_" + i + "_" + j;  
            tile.transform.localScale = new Vector3(gridSize, 1, gridSize);

            Vector3 gridStartPosition = transform.position;
            float positionX = j * gridSize + gridStartPosition.x;     
            float positionY = i * -gridSize + gridStartPosition.z;

                
            tile.transform.position = new Vector3(positionX, 0,  +positionY);
            gridBlockList.Add(tile);
        }
    }
}

private void RemoveGridLayer(int gridIndex)
{
    if (gridIndex >= 0)
    {
        Destroy(gridBlockList[gridIndex]);        
        // gridBlockList.RemoveAt(gridIndex);
    }
}
}

*/