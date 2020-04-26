using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private Slot[] inventory;
    private List<Item> iDB;
    private GameObject[] inventoryUI;

    [Range(0, 100)]
    public int InventorySize = 0;
    [HideInInspector]
    public int ChosenItem = -1;

    private Manager manager = new Manager();

    private GameObject MainInventoryUI;
    private GameObject SlotPrefab;
    private GameObject DragImg;

    public EventSystem eSys;
    public InventoryType InvType;

    private Vector3 offset = new Vector3(50, 50);

    private bool IsDragging = false;
    private bool IsMenuActive = false;

    private void Start()
    {
        InitUI();
        InitInventory();

        iDB = manager.LoadDataBase();
        inventoryUI = new GameObject[InventorySize];
        SlotPrefab = Resources.Load<GameObject>("UI/SlotPrefab");

        LoadInventory();

        if (InvType == InventoryType.MainInventory)
        {
            DebugInventory();
        }
    }
    private void Update()
    {
        Dragging();

        ActiveInventory();
    }

    #region Init
    private void InitUI()
    {
        GameObject UI = GameObject.Find("Main Camera");
        DragImg = UI.transform.Find("MainScreen/DragImage").gameObject;
        switch (InvType)
        {
            case InventoryType.MainInventory: MainInventoryUI = UI.transform.Find("MainScreen/MainInventory").gameObject; break;
            case InventoryType.ToolBet: MainInventoryUI = UI.transform.Find("MainScreen/ToolBet").gameObject; break;
            case InventoryType.Chest: MainInventoryUI = UI.transform.Find("MainScreen/ToolBet").gameObject; break;
        }
    }
    private void InitInventory()
    {
        inventory = new Slot[InventorySize];
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = new Slot { Item = null, Count = 0 };
        }
    }
    private void LoadInventory()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            inventoryUI[i] = Instantiate(SlotPrefab, MainInventoryUI.transform);
            inventoryUI[i].name = i.ToString();
            inventoryUI[i].GetComponent<Button>().onClick.AddListener(delegate { ClickedSlot(); });
        }
        UpdateInventory();
    }
    #endregion

    #region FindAdd
    public Item FindItem(int id)
    {
        for (int i = 0; i < iDB.Count; i++)
        {
            if (iDB[i].Id == id) return iDB[i];
        }
        return iDB[0];
    }
    public void AddItem(int id, int count)
    {
        Item desiredItem = FindItem(id);
        for (int i = 0; i < inventory.Length; i++)
        {
            if (count > 0)
            {
                if (inventory[i].Item == null) count = SlotFilling(i, desiredItem, count, desiredItem.StackSize, false);
                else if (inventory[i].Item.Id == id) count = SlotFilling(i, desiredItem, count, desiredItem.StackSize, true);
            }
            else break;
        }
        if (count > 0)
        {
            DropItem(desiredItem, count);
        }
        UpdateInventory();
    }
    public void AddItem(int itemId, int slotId, int count)
    {
        inventory[slotId].Item = FindItem(itemId);
        inventory[slotId].Count = count;

        UpdateInventory();
    }
    private int SlotFilling(int i, Item desiredItem, int count, int stackSize, bool sameItem)
    {
        if (sameItem)
        {
            count -= (stackSize - inventory[i].Count);
            inventory[i].Count = stackSize;

            return count;
        }
        else
        {
            if (count > stackSize)
            {
                inventory[i].Item = desiredItem;
                inventory[i].Count = stackSize;

                return count - stackSize;
            }
            else
            {
                inventory[i].Item = desiredItem;
                inventory[i].Count = count;

                return 0;
            }
        }
    }
    #endregion

    #region UpdateInventoryUI
    public void ClickedSlot()
    {
        if (IsDragging == false)
        {
            ChosenItem = int.Parse(eSys.currentSelectedGameObject.name);

            if (inventory[ChosenItem].Item != null) StartDrag();
            else ChosenItem = -1;
        }
        else
        {
            if (ChosenItem != int.Parse(eSys.currentSelectedGameObject.name)) // если мы нажиимем не в ту же клетку
            {
                if (inventory[ChosenItem].Item != inventory[int.Parse(eSys.currentSelectedGameObject.name)].Item) // если выбраный предмет не такой же, как тот на который мы кликаем
                {
                    SwapItem(ChosenItem, int.Parse(eSys.currentSelectedGameObject.name));
                }
                else
                {
                    StackItem(ChosenItem, int.Parse(eSys.currentSelectedGameObject.name));
                }
            }
            StopDrag();
            UpdateInventory();
        }
    }
    public void Dragging()
    {
        if (IsDragging)
        {
            DragImg.transform.position = Input.mousePosition + offset;
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1)) StopDrag();
        }
    }
    public void StartDrag()
    {
        IsDragging = true;

        ChosenItem = int.Parse(eSys.currentSelectedGameObject.name);

        DragImg.GetComponent<Image>().sprite = inventory[ChosenItem].Item.Icon;
        DragImg.transform.GetChild(0).GetComponent<Text>().text = inventory[ChosenItem].Count.ToString();

        UpdateInventory();
    }
    public void StopDrag()
    {
        DragImg.transform.position = new Vector3(- 1030, 58);

        ChosenItem = -1;

        IsDragging = false;
    }
    private void UpdateInventory()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].Item == null)
            {
                inventoryUI[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemSprite/Empty");
                inventoryUI[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            else
            {
                inventoryUI[i].transform.GetChild(0).GetComponent<Image>().sprite = inventory[i].Item.Icon;
                inventoryUI[i].transform.GetChild(1).GetComponent<Text>().text = inventory[i].Count.ToString();
            }
        }
    }
    #endregion

    #region InteractWithInventory
    public void SwapItem(int newSlotID, int oldSlotID)
    {
        Slot oldSlot = inventory[oldSlotID];
        inventory[oldSlotID] = inventory[newSlotID];
        inventory[newSlotID] = oldSlot;
    }
    public void StackItem(int choseItemID, int existingItemID)
    {
        if (inventory[choseItemID].Count + inventory[existingItemID].Count < inventory[choseItemID].Item.StackSize)
        {
            inventory[existingItemID].Count += inventory[choseItemID].Count;
            inventory[choseItemID].Item = null;
            inventory[choseItemID].Count = 0;
        }
        else
        {
            if (inventory[existingItemID].Count != inventory[existingItemID].Item.StackSize ||
                inventory[choseItemID].Count != inventory[choseItemID].Item.StackSize)
            {
                int count = inventory[choseItemID].Count - (inventory[existingItemID].Item.StackSize - inventory[existingItemID].Count);
                inventory[existingItemID].Count = inventory[existingItemID].Item.StackSize;

                inventory[choseItemID].Item = null;
                inventory[choseItemID].Count = 0;

                AddItem(inventory[existingItemID].Item.Id, count);
            }
            else
            {
                SwapItem(existingItemID, choseItemID);
            }
        }
    }
    public void ClearAndDrop(int invСellNum)
    {
        inventory[invСellNum].Item = null;
        inventory[invСellNum].Count = 0;

        DropItem(inventory[invСellNum].Item, inventory[invСellNum].Count);
    }
    public void DropItem(Item item, int count)
    {
        //ToDo: написать выброс итема
        Debug.Log($"Предмет: {item.Title}, выброшен в количестве: {count}");
    }
    #endregion

    private void ActiveInventory()
    {
        if (InvType == InventoryType.MainInventory && Input.GetKeyDown(KeyCode.I))
        {
            if (IsMenuActive)
            {
                MainInventoryUI.SetActive(false);
                IsMenuActive = false;
            }
            else
            {
                MainInventoryUI.SetActive(true);
                IsMenuActive = true;
            }
        }
    }

    private void DebugInventory()// дебаг
    {
        //AddItem(1000, 12);
        //AddItem(1000, 9);
        //AddItem(2000, 4);
        //AddItem(5000, 3);
        AddItem(1000, 0, 3);
        AddItem(1000, 1, 3);
        AddItem(1000, 3, 8);
        AddItem(2000, 4, 6);
        AddItem(5000, 5, 1);
        AddItem(5000, 6, 1);

        //for (int i = 0; i < InventorySize; i++)
        //{
        //    if (inventory[i].Item != null)
        //    {
        //        Debug.Log($"Item {i}: {inventory[i].Item.Title}, CountOfItem {inventory[i].Count}");
        //    }
        //}
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
    ToolBet,
    Chest
}