using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerOpenner : MonoBehaviour
{
    public GameObject Slots;
    public GameObject Grid;


    public void OpenContainer()
    {
        Slots.SetActive(true);
        Grid.SetActive(true);
    }

    public void CloseContainer()
    {
        Slots.SetActive(false);
        Grid.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if (collision.gameObject.tag == "Container")
        {
            OpenContainer();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        //  if (collision.gameObject.tag == "Container")
        {
            CloseContainer();
        }
    }
}
