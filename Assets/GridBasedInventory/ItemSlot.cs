using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Vector2Int size = new Vector2Int(45, 45); //slot cell size 
    public Item item;

    // Show to the User
    public Vector2 NewPosition; // Debugging and UI Purposes, Works in Slotted Space (1,2,3,4..)
    public Vector2 oldPosition; // Debugging and UI Purposes, Works in Slotted Space (1,2,3,4..)


    [HideInInspector]
    public bool InChest = false;

    [HideInInspector]
    public Vector2 currentPosition; // For anchorPosition settings Works in Canvas Space (45*)
    public Vector2 previousPosition;
    public ItemGridGenerator slot;

    void Start()
    {
        // Resize items given current anchors
        foreach (RectTransform AnchoredPosition in transform)
        {
            AnchoredPosition.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.Dimensions.x * AnchoredPosition.rect.width);
            AnchoredPosition.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.Dimensions.y * AnchoredPosition.rect.height);

            foreach (RectTransform Icon in AnchoredPosition)
            {
                Icon.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.Dimensions.x * Icon.rect.width);
                Icon.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.Dimensions.y * Icon.rect.height);
                // Set Icon image to the center of the "Slot"
                Icon.localPosition = new Vector2(AnchoredPosition.localPosition.x + AnchoredPosition.rect.width / 2, -(AnchoredPosition.localPosition.y + AnchoredPosition.rect.height / 2));
            }
        }

        slot = GameObject.Find("Inventory_Grid").GetComponent<ItemGridGenerator>();

    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        // Set the Old position and let the mouse Raycast go through the object.
        previousPosition = transform.GetComponent<RectTransform>().anchoredPosition;
        oldPosition.x = (previousPosition.x / size.x);
        oldPosition.y = (-previousPosition.y / size.y);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        for (int x = 0; x < item.Dimensions.x; x++)
        {
            for (int y = 0; y < item.Dimensions.y; y++)
            {
                // Set own spot before dropping to "Unslotted"
                slot.grid[(int)oldPosition.x + x, (int)oldPosition.y + y] = 0;
            }
        }
        // Move Game Object
        transform.position = eventData.position;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            #region PositionHandling
            /// <summary>
            /// Handle the Positioning of ItemObjects after Drag has ended and PointerIsOverGameObject
            /// Check the Destination through variable "EndDrag" and convert it to World->CellSpace 45:1 and check
            /// for a Valid location between the Slots of the grid, After check if the Grid Array returns 0/1
            /// 1 for already Slotted - Return to previous location, 0 for Unslotted - Free to be slotted
            /// </summary>

            // Current Position OnEndDrag
            currentPosition = GetComponent<RectTransform>().anchoredPosition;

            // Dropped Position Divided by Cellsize (45,45)
            Vector2 destinationSlot = new Vector2();

            // Round Interger to find the "Slot" in the Grid 
            // change to a different "Scaling" to work with the Grid
            destinationSlot.x = Mathf.Floor(currentPosition.x / size.x);
            destinationSlot.y = Mathf.Floor(-currentPosition.y / size.y);


            // Slotted and Position Handling
            List<Vector2> newPositionCheck = new List<Vector2>(); // Hold new position for Item.
            bool Slotted = false; // Create Slotted Information, Assume item has not been "Slotted" yet



            // Make Sure Item was Dropped within Slots In Own Inventory, In Anchored Chordinates
            // Subtract 1 to deal with the Grid starting at position 0
            // 
            if (((destinationSlot.x) + (item.Dimensions.x) - 1) < slot.GridMaxX // Make sure Destination X is less than Max Slot Y (Outside Right) 
            && ((destinationSlot.y) + (item.Dimensions.y) - 1) < slot.GridMaxY // Make sure Destination Y is less than  Max Slot X (Outside Down)
            && ((destinationSlot.x)) >= 0 && destinationSlot.y >= 0) // Make sure Slot destination isn't Below 0 (Outside Left) (Outside Up)
            {


                // Check through all the Slots based on the itemSize of the Item that it would Occupy
                for (int x = 0; x < item.Dimensions.x; x++)
                {
                    for (int y = 0; y < item.Dimensions.y; y++)
                    {
                        // Check if the slots for the size of the item + its current destination if any are slotted
                        if (slot.grid[(int)destinationSlot.x + x, (int)destinationSlot.y + y] != 1)
                        {
                            Vector2 Position;
                            Position.x = destinationSlot.x + x;
                            Position.y = destinationSlot.y + y;
                            newPositionCheck.Add(Position);
                            Slotted = true;
                        }
                        // If the area is slotted,
                        else
                        {
                            // Set transform back to previous location
                            Debug.Log("Didnt Work");
                            transform.GetComponent<RectTransform>().anchoredPosition = previousPosition;
                            // Issue with Break, Setting sizeY and sizeX simply broke out of the for Statement more Consistantly
                            x = (int)item.Dimensions.x;
                            y = (int)item.Dimensions.y;
                            newPositionCheck.Clear();
                            
                        }
                        

                    }

                }

                #region SlottedHandling
                /// <summary>
                /// Handle Slotting based on the "Slotted" variable handed from the PositionHandling
                /// If Position is true treverse through the X and Y of the ItemSize, make the previous location 
                /// Grid Array position 0 (Open up for other items to be slotted) and set the destination location
                /// to 1 so that no other Items can be slotted.
                /// </summary>
                if (Slotted)
                {
                    // Traversing through Dimension of Item
                    for (int x = 0; x < item.Dimensions.x; x++)
                    {
                        for (int y = 0; y < item.Dimensions.y; y++)
                        {
                            // Change the previous Slot so that new items may be slotted
                            slot.grid[(int)oldPosition.x + x, (int)oldPosition.y + y] = 0;

                        }
                    }
                    // Change the new position to be "Slotted"
                    for (int i = 0; i < newPositionCheck.Count; i++)
                    {
                        // Change slot.grid array so that destination shows A Slotted Item
                        slot.grid[(int)newPositionCheck[i].x, (int)newPositionCheck[i].y] = 1;
                    }

                    NewPosition = newPositionCheck[0]; // set new start position
                    transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(NewPosition.x * size.x, -NewPosition.y * size.y);
                }
                else // Item "Was" Previously Slotted.
                {
                    for (int x = 0; x < item.Dimensions.x; x++)
                    {
                        for (int y = 0; y < item.Dimensions.y; y++)
                        {
                            slot.grid[(int)oldPosition.x + x, (int)oldPosition.y + y] = 1; //back to position 1;
                        }
                    }
                }
            }

            // Handle if its being Slotted from just outside of the Inventory Range
            else if (((destinationSlot.x) + (item.Dimensions.x) - 1) <= slot.GridMaxX
            && ((destinationSlot.y) + (item.Dimensions.y) - 1) <= slot.GridMaxY
            && ((destinationSlot.x)) >= -1 && destinationSlot.y >= -1)
            {
                // Change the location back to its previous location
                this.transform.GetComponent<RectTransform>().anchoredPosition = previousPosition;

                // Set the item's old position to be slotted
                for (int x = 0; x < item.Dimensions.x; x++)
                {
                    for (int y = 0; y < item.Dimensions.y; y++)
                    {
                        slot.grid[(int)oldPosition.x + x, (int)oldPosition.y + y] = 1; //back to position 1;

                    }
                }
            }

            #endregion SlottedHandling


            #region OtherSlots
            /// <summary>
            /// We make a list of all hovered Objects and go through each of them, if we see
            /// that we are hovering our other "ChestSlots" we don't return our Object to 
            /// the OriginPosition and we instead pass to our ChestSlot to handle the movement
            /// </summary>

            else // Still Over Game Object put Not Inventory Grid
            {
                // Get a list of all hovered game objects
                bool onSlot = false;
                List<GameObject> hoveredList = eventData.hovered;
                foreach (var obj in hoveredList)
                {
                    // If object is over SlotTetris, Let SlotTetris OnDrop script work
                    if (obj.name == "ChestSlot(Clone)" || obj.name == "ItemSlot(Clone)")
                    {
                        onSlot = true;
                    }
                }
                if (!onSlot)
                {
                    // If its on any other object change position to its old position as per usual
                    this.transform.GetComponent<RectTransform>().anchoredPosition = previousPosition;
                }
                GetComponent<CanvasGroup>().blocksRaycasts = true; // Allow Raycasting
            }


            #endregion OtherSlots

        }
        else
        {
            Debug.Log("Didnt Work");
            // Object was Dragged on not a game object, Send back to original location
            this.transform.GetComponent<RectTransform>().anchoredPosition = previousPosition;
        }



        #endregion PositionHandling

        Debug.Log("drag Ended");
        GetComponent<CanvasGroup>().blocksRaycasts = true; // Allow Raycasting
    }

    public void OnUse()
    {
        item.Use();
    }

}


