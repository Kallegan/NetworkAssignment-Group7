using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private int worldWidth = 20;
    [SerializeField] private int worldHeight = 20;

    private readonly float xOffset = 1.8f;  
    private readonly float zOffset = 1.6f;

    float radius = 14f;

    private List<GameObject> hexList = new();

    [SerializeField] private GameObject hexPrefab;

    void Start()
    {
        GenerateHexGrid();
        //StartCoroutine(ShrinkGrid());
        //InvokeRepeating("ShrinkGrid", 1, 1);

        
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
                Vector3 gridPosition;
                
                if (z % 2 == 0)
                {
                    gridPosition = new Vector3(x * xOffset, 0, z * zOffset);

                }
                else
                {
                    gridPosition = new Vector3(x * xOffset + xOffset / 2, 0, z * zOffset);
                }
                             

                tempGameObject.transform.position = gridPosition;
                tempGameObject.name = "Hex_" + x + "," + z;

                hexList.Add(tempGameObject);
                SetHexShape(); //temp move out from loop

            }
        }
    }

    

    private void SetHexShape()
    {
        for (int i = 0; i < hexList.Count; i++)
        {
            if (Vector3.Distance(hexList[i].transform.position, transform.position) > radius)
            {
                Destroy(hexList[i]);
                hexList.RemoveAt(i);
            }
        }
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
IEnumerator ShrinkGrid()
    {
        yield return new WaitForSeconds(1);

        if(radius > 5)
            radius = radius - 2;

        SetHexShape();
        StartCoroutine(ShrinkGrid());
    }

*/