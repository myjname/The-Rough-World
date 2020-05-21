using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestMechanics : MonoBehaviour
{
    private List<Item> iDB;
    private Manager manager = new Manager();

    private void Start()
    {
        iDB = manager.LoadDataBase();
    }

    private void Update()
    {
        if (GetComponent<EnemyParameters>().localHitPoints <= 0)
        {
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                Item dropItem = GetRandomItem();

                int numDropItems = Random.Range(1, dropItem.StackSize + 1);
                if (numDropItems > 4) numDropItems = 4;

                DropItem(dropItem, numDropItems);
            }
            Destroy(gameObject);
        }
    }

    private void DropItem(Item item, int count)
    {
        //Debug.Log($"Предмет: {item.Title}, выброшен в количестве: {count}");

        GameObject dropItem = Instantiate(Resources.Load<GameObject>(item.WorldObj), transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)), Quaternion.identity);
        dropItem.tag = "Item";

        PickUp pickUp = dropItem.AddComponent<PickUp>();
        pickUp.id = item.Id;
        pickUp.count = count;

        BoxCollider boxCollider = dropItem.AddComponent<BoxCollider>();
        boxCollider.center = new Vector3(0, 0.5f, 0);
        boxCollider.size = new Vector3(1, 1, 1);
        boxCollider.isTrigger = true;
    }

    private Item GetRandomItem()
    {
        Item item = iDB[Random.Range(0, 3)];
        return item;
    }
}