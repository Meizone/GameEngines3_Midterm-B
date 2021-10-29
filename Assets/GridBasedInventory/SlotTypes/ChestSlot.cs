/*
Nathan Nguyen
101268067
Grid-Based Inventory
Chest Slot


*NEW COMMENTS BELOW*
Description:
Handle Parenting "Chest" type ItemSlots and the Initial Movement only
Because we are changing the parent our Canvas Scope has also changed which means 
if the Canvas these slots are being put in are Identical the ItemSlot will be able to use 
its same code to deal with the Movement Within the script
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChestSlot : MonoBehaviour, IDropHandler
{
    //public Vector2 Offset = new Vector2(25,-25);

   
    private ItemSlot itemSlot;

    public void OnDrop(PointerEventData eventData)
    {

        // Check if gameobject is not null *NEW*
        if(eventData.pointerDrag != null)
        {
            // Check if the GameObject ItemSlot.InChest is false, If False set the parent of the Object to the Chest Inventory *NEW*
            // And set its initial Position
            if (!eventData.pointerDrag.GetComponent<ItemSlot>().InChest)
            {
                itemSlot = eventData.pointerDrag.GetComponent<ItemSlot>();
                itemSlot.InChest = true;

                for (int x = 0; x < itemSlot.item.Dimensions.x; x++)
                {
                    for (int y = 0; y < itemSlot.item.Dimensions.y; y++)
                    {
                        // Set own spot before dropping to "Unslotted"
                        itemSlot.slot.grid[(int)itemSlot.oldPosition.x + x, (int)itemSlot.oldPosition.y + y] = 0;
                    }
                }

                // Leaving this here because I think its cool
                //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = switchToRectTransform(GetComponent<RectTransform>(), eventData.pointerDrag.GetComponent<RectTransform>());
                eventData.pointerDrag.GetComponent<RectTransform>().parent = GameObject.Find("Chest_Grid").GetComponent<RectTransform>();
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            }

        }
    }

    
    /*
     * 
     * This is just cool and cant really be mentioned in the video due to only having 10 minutes to explain
     * but this basically creates a fake camera holding the perspective of the rect Transform that "from" is from
     * and making it transform its anchoredPosition into the perspective of the "to" rectTransform
     * Thought it was really cool and just left it in here for my future reference just incase if i ever needed it
    private Vector2 switchToRectTransform(RectTransform from, RectTransform to)
    {
        Vector2 localPoint;
        // Calculating a fake "Camera" associated with the screen space position from getting the position of this ChestSlot to ScreenPoint + 
        // the position of the Item GameObject based off of the RectTransform it originated from relative to its Pivot Point.
        Vector2 fromPivotDerivedOffset = new Vector2(from.rect.width * from.pivot.x + from.rect.xMin, from.rect.height * from.pivot.y + from.rect.yMin);
        Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, from.position);
        // Fake "Camera"
        screenP += fromPivotDerivedOffset;
        // Calculate the Local Pivot getting the LocalPoint (Point in Local Space of the Rect Transform) using the RectTransform we want
        // The object to be placed in, with our fake "Camera" holding the prospective of the RectTransform from the original GameObject
        // and set out localPoint to the calculated LocalPoint
        RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out localPoint);
        // Calculate the Pivot offset for our gameobject 
        Vector2 pivotDerivedOffset = new Vector2(to.rect.width * to.pivot.x + to.rect.xMin, to.rect.height * to.pivot.y + to.rect.yMin);
        // Return the new AnchoredPosition relative to our RectTransform in the inventory with an Offset for Finetuning
        return to.anchoredPosition + localPoint - pivotDerivedOffset - Offset;
    }
    */

    

}

