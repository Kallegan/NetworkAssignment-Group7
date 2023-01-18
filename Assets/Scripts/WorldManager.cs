using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
  
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
            //Destroy(gridBlockList[gridIndex]);        
           // gridBlockList.RemoveAt(gridIndex);
        }
    }
}
