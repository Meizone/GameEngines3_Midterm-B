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
