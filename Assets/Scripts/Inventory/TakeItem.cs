using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItem : MonoBehaviour
{
    private GameObject Cam;
    private Inventory MainInventory;
    
    private PickUp pickUp;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Item")
        {
            Debug.Log("Предмет!");

            Cam = GameObject.Find("Main Camera");
            pickUp = collider.GetComponent<PickUp>();

            MainInventory = Cam.GetComponents<Inventory>()[0];
            MainInventory.AddItem(pickUp.id, pickUp.count);

            Destroy(collider.gameObject);
        }
    }
}