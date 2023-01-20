using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class WorldManager : MonoBehaviour
{
    [SerializeField] private int worldWidth = 20;
    [SerializeField] private int worldHeight = 20;

    private readonly float xOffset = 1.8f;
    private readonly float zOffset = 1.6f;


    [SerializeField] float playfieldSize = 14f;
    [SerializeField] float levelMinSize = 5;
    [SerializeField] float shrinkAmount = 2;
    [SerializeField] float shrinkStartDelay = 10;
    [SerializeField] float shrinkRepeatTimer = 10;

    private readonly List<GameObject> hexList = new();
    private readonly List<GameObject> hexMarkedForDeletion = new();


    [SerializeField] private GameObject hexPrefab; 
        

    void Start()
    {
        GenerateHexGrid();
        SetHexShape(); 
        StarShrinkLevel(); //move out from start and trigger from game manager instead.
    }

    public void StarShrinkLevel()
    {
        InvokeRepeating(nameof(ShrinkGrid), shrinkStartDelay, shrinkRepeatTimer);
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

   

    private void ShrinkGrid()
    {
        if (playfieldSize > levelMinSize)
            playfieldSize -= shrinkAmount;

        SetHexShape();        
    }

    private void SetHexShape()
    {
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < hexList.Count; i++)
            {
                if (Vector3.Distance(hexList[i].transform.position, transform.position) > playfieldSize)
                {
                    hexList[i].GetComponent<MaterialPropertyBlockTest>().PendingTileOutOfBound();

                    hexMarkedForDeletion.Add(hexList[i]);
                    hexList.RemoveAt(i);
                    StartCoroutine(DestroyHexOutOfRange());

                }
            }
        }

    }


    IEnumerator DestroyHexOutOfRange()
    {
        yield return new WaitForSeconds(shrinkRepeatTimer / 2);

        for (int i = 0; i < hexMarkedForDeletion.Count; i++)
        {
            hexMarkedForDeletion[i].GetComponent<MaterialPropertyBlockTest>().TileOutOfBound();
            //Destroy(hexMarkedForDeletion[i]);
        }

        hexMarkedForDeletion.Clear();
    }
}
    