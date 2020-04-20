using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigerNPS : MonoBehaviour
{
    private void OnTriggerEnter(Collider colider)
    {
        if (colider.tag == "Player")
        {
            Debug.Log("Мойте руки");
        }
    }
}
