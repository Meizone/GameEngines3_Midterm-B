/*
Nathan Nguyen
101268067
Grid-Based Inventory
Item


Description:
Defines Item information, Only used parts for the Assignment so far are 
Dimension



*NEW COMMENTS BELOW*
In the future maybe implement the following:
Have the Icon directly change the Icon so you don't need to set the Icon Twice

Actually have the user be able to Use Items with a Text Below the Item to show, 
Be able to have the Description of the Item when hovered show up somewhere
maybe be able to have Unique Properties?,

Too focused on trying to get the Grid to actually work.

Future Reference:
Try to figure out why the Game Glitches out with an Out Of Array @ End of ItemSlot code,
Update to earlier versions to test if the issue is specifically with 2020 stable version.
(Issue OutOfBoundsArray, Parameter: Interger @ Eventsystem.Unity)

Try to make the objects stop collapsing or update their Slotted Position in Realtime to deal with
Parenting moving objects around unexpectingly and creating an issue with the GridSlotted System.

Try to make it easier to create Multiple inventories are the steps currently to create multiple inventories are annoying:
 - Duplicate Chest Grid
 - Duplicate ItemGridGenerator Item to create another canvas
 - Duplicate ChestSlot script to something else
 - Add a boolean to check which Chest
 - Change the If Statement in the Slots to Handle being in different slots
 - and all Canvas's have to be identical in size and scale

*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ItemModifiedException : System.Exception
{
    public ItemModifiedException(string message) : base(message)
    {
    }
}

//Attribute that allows us to right click->create
[CreateAssetMenu(fileName = "New Item", menuName = "ItemSystem/Item")]
public class Item : ScriptableObject
{

    //Basic information
    public string Name;
    public string description = "this is an item";
    public Sprite Icon;
    public bool isConsumable = false;
    public Vector2 Dimensions;
    public int StackCount;


    //returns whether or not the Item was successfully used
    public bool Use()
    {
        if(isConsumable)
        {
            Debug.Log("Used item: " + name);
            --StackCount;
            return true;
        }
        return false;
    }
}
