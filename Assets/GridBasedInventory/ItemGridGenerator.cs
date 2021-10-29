/*
Nathan Nguyen
101268067
Grid-Based Inventory
ItemGridGenerator


Description:
Generate Grid Prefabs based on number of slots
GridMaxX and GridMaxY has to be set individually by the user, This is due to 
that Canvas being Static and not dynamic, The canvas will not expand to fit the users
Inventory Size automatically as it added complexity 

grid - Array of Intergers based on the GridMaxX and GridMaxY
Will only be either one of two states
0 - Unslotted
1 - Slotted
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemGridGenerator : MonoBehaviour
{

    [SerializeField]
    GameObject slotPrefab;
    public int numberSlots; 

    [HideInInspector]
    // 2 Dimensional Grid to handle detecting whether the Slot is "Slotted"
    public int[,] grid;
    public int GridMaxX;
    public int GridMaxY;


    void Start()
    {
        GridMaxX = 11;
        GridMaxY = numberSlots / 11;

        // Set bag Dimensions to reflect 
        grid = new int[GridMaxX, GridMaxY]; 

        for (int i = 0; i < numberSlots; i++)
        {
            GameObject newGameObject = Instantiate(slotPrefab, transform); 
        }


    }




}
