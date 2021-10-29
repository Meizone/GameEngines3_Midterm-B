/*
Nathan Nguyen
101268067
Grid-Based Inventory
Inventory Slot


Description:
Handle Parenting "Inventory" type ItemSlots and the and the Initial Movement only
Because we are changing the parent our Canvas Scope has also changed which means 
if the Canvas these slots are being put in are Identical the ItemSlot will be able to use 
its same code to deal with the Movement Within the script
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

class ArguementIndexException : System.Exception
{
    public ArguementIndexException(string message) : base(message)
    {
    }
}

public class InventorySlot : MonoBehaviour, IDropHandler
{
    //public Vector2 Offset = new Vector2(25, -25);
    private ItemSlot itemSlot;

    // Check whenever an item is "Dropped" according to the Event System in Unity
    public void OnDrop(PointerEventData eventData)
    {
        // Make sure the GameObject being dropped is not null
        if (eventData.pointerDrag != null)
        {
            // Set the Itemslot to the GameObjects Itemslot Component if available
            itemSlot = eventData.pointerDrag.GetComponent<ItemSlot>();

            // Check if the Item slot is in a chest, if true change the parent to the Inventory and set its anchored position
            if (eventData.pointerDrag.GetComponent<ItemSlot>().InChest)
            {
                for (int x = 0; x < itemSlot.item.Dimensions.x; x++)
                {
                    for (int y = 0; y < itemSlot.item.Dimensions.y; y++)
                    {
                        // Set own spot before dropping to "Unslotted"
                        itemSlot.slot.grid[(int)itemSlot.oldPosition.x + x, (int)itemSlot.oldPosition.y + y] = 0;
                    }
                }

                eventData.pointerDrag.GetComponent<ItemSlot>().InChest = false;
                eventData.pointerDrag.GetComponent<RectTransform>().parent = GameObject.Find("Items_Grid").GetComponent<RectTransform>();
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            }

        }
    }


}
