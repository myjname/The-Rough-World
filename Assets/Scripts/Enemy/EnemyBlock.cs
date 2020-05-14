using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBlock : MonoBehaviour
{
    public GameObject Chest;
    public GameObject Enemy;

    private GameObject localChest;
    private GameObject localEnemy;

    private bool ActivateFight = true;

    private void OnTriggerEnter(Collider colider)
    {
        if (colider.tag == "Player" && ActivateFight)
        {
            localChest = Instantiate(Chest, transform);
            localChest.transform.position = new Vector3(transform.position.x, 0.6f, transform.position.z);

            for (int i = 0; i < Random.Range(3, 5); i++)
            {
                Instantiate(Enemy, transform);
                Enemy.transform.position = new Vector3(Random.Range(-5, 5), 0.6f, Random.Range(-5, 5));
            }
            ActivateFight = false;
        }
    }
}
