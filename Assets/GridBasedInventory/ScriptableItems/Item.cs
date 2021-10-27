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
