using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Slot[] inventory;
    private List<Item> iDB;

    public GameObject InventoryUI;
    public GameObject SlotPrefab;

    [Range(0, 100)]
    public int InventorySize = 0;
    [Range(0, 100)]
    public int ChosenItem = 0;
    public int StackSize = 10;

    //private InventoryType InvType = InventoryType.MainInventory;

    private Manager manager = new Manager();

    private void Start()
    {
        inventory = new Slot[InventorySize];

        iDB = manager.LoadDataBase();

        AddItem(1000, 12);
        AddItem(2000, 23);
        AddItem(5000, 17);

        DebugInventure();

        //надо бы создать ui и его выбирать и спавнить
    }

    public void AddItem(int id, int count)//TODO: добавить проверку на возможность стакаться
    {
        int localcount = count;

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = new Slot { Item = FindItem(id), Count = localcount };

                if (inventory[i].Count > StackSize)
                {
                    localcount = inventory[i].Count - StackSize;
                    inventory[i].Count = StackSize;
                }
                else break;
            }
            else
            {
                if (inventory[i].Item.Id == id)
                {
                    inventory[i] = new Slot { Item = FindItem(id), Count = localcount };

                    if (inventory[i].Count > StackSize)
                    {
                        localcount = inventory[i].Count - StackSize;
                        inventory[i].Count = StackSize;
                    }
                    else break;
                }
            }
        }
        DropItem();
    }

    public void AddItem(string title, int count)//TODO: добавить проверку на возможность стакаться
    {
        int localcount = count;

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = new Slot { Item = FindItem(title), Count = localcount };

                if (inventory[i].Count > StackSize)
                {
                    localcount = inventory[i].Count - StackSize;
                    inventory[i].Count = StackSize;
                }
                else break;

            }
            else
            {
                if (inventory[i].Item.Title == title)
                {
                    inventory[i].Count += localcount;

                    if (inventory[i].Count > StackSize)
                    {
                        localcount = inventory[i].Count - StackSize;
                        inventory[i].Count = StackSize;
                    }
                    else break;
                }
            }
        }
        DropItem();
    }

    public void DropItem()
    {
        //надо написать
    }

    public void DebugInventure()// дебаг
    {
        for (int i = 0; i < StackSize; i++)
        {
            if (inventory[i] != null)
            {
                Debug.Log($"Item {i}: {inventory[i].Item.Title}, CountOfItem {inventory[i].Count}");
            }
        }
    }

    public Item FindItem(int id)
    {
        Item item = new Item();

        for (int i = 0; i < iDB.Count; i++)
        {
            if (iDB[i].Id == id) item = iDB[i];
        }

        return item;
    }

    public Item FindItem(string title)
    {
        Item item = new Item();

        for (int i = 0; i < iDB.Count; i++)
        {
            if (iDB[i].Title == title) item = iDB[i];
        }

        return item;
    }
}

public class Slot
{
    public Item Item { get; set; }
    public int Count { get; set; }
}

public enum InventoryType
{
    MainInventory,
    Chest
}