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

    public void OnDrop(PointerEventData eventData)
    {

        if (eventData.pointerDrag != null)
        {
            itemSlot = eventData.pointerDrag.GetComponent<ItemSlot>();
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
