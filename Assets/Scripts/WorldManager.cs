using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class WorldManager : MonoBehaviour
{
    [SerializeField] private int worldWidth = 20;
    [SerializeField] private int worldHeight = 20;

    private readonly float xOffset = 1.8f;
    private readonly float zOffset = 1.6f;


    [SerializeField] float levelShrinkSize = 14f;
    [SerializeField] float shrinkStartDelay = 10;
    [SerializeField] float shrinkRepeatTimer = 5;

    private List<GameObject> hexList = new();

    [SerializeField] private GameObject hexPrefab; 
        

    void Start()
    {
        GenerateHexGrid();
        SetHexShape(); 
        StarShrinkLevel(); //move out from start and trigger from game manager instead.
    }

    public void StarShrinkLevel()
    {
        InvokeRepeating("ShrinkGrid", shrinkStartDelay, shrinkRepeatTimer);
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
                tempGameObject.transform.parent = transform;

                

                hexList.Add(tempGameObject);
                

            }
        }
    }

    private void SetHexShape()
    {
        for (int j = 0; j < 6; j++)
        {
            for (int i = 0; i < hexList.Count; i++)
            {
                if (Vector3.Distance(hexList[i].transform.position, transform.position) > levelShrinkSize)
                {
                    hexList[i].GetComponent<MaterialPropertyBlockTest>().MarkForDeletion();
                    //Destroy(hexList[i]);
                    //hexList.RemoveAt(i);
                }
            }
        }     
       
    }

    private void ShrinkGrid()
    {
        if (levelShrinkSize > 5)
            levelShrinkSize = levelShrinkSize - 2;

        SetHexShape();        
    }
}
    