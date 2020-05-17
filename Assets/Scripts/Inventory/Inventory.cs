using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Variables
    public Slot[] inventory;
    private List<Item> iDB;
    private GameObject[] inventoryUI;

    [Range(0, 100)]
    public int InventorySize = 0;
    [HideInInspector]
    public int ChosenItem = -1;

    private GameObject UI;
    private GameObject MainInventoryUI;
    private GameObject SlotPrefab;
    private GameObject DragImg;

    private GameObject InfoOfItemUI;
    private GameObject localInfoOfItemUI;

    private GameObject persGG;

    [HideInInspector]
    public GameObject ObjInArm;
    private int chosenTollBetSlot = -1;

    public EventSystem eSys;
    public InventoryType InvType;
    private Manager manager = new Manager();
    private CameraUI CUI;

    private Vector3 offset = new Vector3(50, 50);

    [HideInInspector]
    public bool IsDragging = false;
    [HideInInspector]
    public bool IsMenuActive = false;

    private bool search = true;
    #endregion

    private void Start()
    {
        UI = GameObject.Find("Main Camera");

        InitUI();
        InitInventory();

        iDB = manager.LoadDataBase();

        CUI = GetComponents<CameraUI>()[0];

        inventoryUI = new GameObject[InventorySize];
        SlotPrefab = Resources.Load<GameObject>("UI/SlotPrefab");
        InfoOfItemUI = Resources.Load<GameObject>("UI/InfoOfItem");

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

        while (search)
        {
            persGG = GameObject.FindGameObjectWithTag("Player");

            search = false;
        }

        InteractWithTollBet();
    }

    #region Init
    private void InitUI()
    {
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
            if (inventory[i].Count + count > stackSize)
            {
                count = count + inventory[i].Count - stackSize;
                inventory[i].Count = stackSize;
                return count;
            }
            else
            {
                inventory[i].Count += count;
                return 0;
            }
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
        if (!CUI.DragInOtherInventory(InvType))
        {
            if (IsDragging == false)
            {
                ChosenItem = int.Parse(eSys.currentSelectedGameObject.name);
                
                if (inventory[ChosenItem].Item != null)
                {
                    StartDrag();

                    if (InvType == InventoryType.MainInventory ||
                        InvType == InventoryType.Chest) SpawnInfoOfItem();
                }
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
                DestroyInfoOfItem();
                UpdateInventory();
            }
        }
        else
        {
            //Debug.Log("переместить ");

            CUI.SwapItemsOtherInventorys();
        }
    }
    public void Dragging()
    {
        if (IsDragging)
        {
            DragImg.transform.position = Input.mousePosition + offset;
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1)) && IsDragging == true) StopDrag();
        }
    }
    public void StartDrag()
    {
        IsDragging = true;

        ChosenItem = int.Parse(eSys.currentSelectedGameObject.name);
        
        DragImg.GetComponent<Image>().sprite = inventory[ChosenItem].Item.Icon;
        DragImg.transform.GetChild(0).GetComponent<Text>().text = inventory[ChosenItem].Count.ToString();
    }
    public void StopDrag()
    {
        DragImg.transform.position = new Vector3(- 1030, 58);

        ChosenItem = -1;

        IsDragging = false;

        DestroyInfoOfItem();
    }
    public void UpdateInventory()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].Count < 1) inventory[i].Item = null;

            if (inventory[i].Item == null)
            {
                inventoryUI[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemSprite/Empty");
                inventoryUI[i].transform.GetChild(1).GetComponent<Text>().text = "";

                if (InvType == InventoryType.ToolBet && chosenTollBetSlot == i)
                {
                    Destroy(ObjInArm);
                }
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
    public void ClearAndDrop(int invSlotNum)
    {
        DropItem(inventory[invSlotNum].Item, inventory[invSlotNum].Count);

        inventory[invSlotNum].Item = null;
        inventory[invSlotNum].Count = 0;

        StopDrag();
        UpdateInventory();
    }
    public void DropItem(Item item, int count)
    {
        //Debug.Log($"Предмет: {item.Title}, выброшен в количестве: {count}");

        GameObject dropItem = Instantiate(item.WorldObj, persGG.transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        dropItem.tag = "Item";

        PickUp pickUp = dropItem.AddComponent<PickUp>();
        pickUp.id = item.Id;
        pickUp.count = count;

        BoxCollider boxCollider = dropItem.AddComponent<BoxCollider>();
        boxCollider.center = new Vector3(0, 0.5f, 0);
        boxCollider.size = new Vector3(1, 1, 1);
        boxCollider.isTrigger = true;
    }
    public void SpawnInfoOfItem()
    {
        localInfoOfItemUI = Instantiate(InfoOfItemUI, UI.transform.Find("MainScreen"));
        localInfoOfItemUI.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { ClearAndDrop(ChosenItem); });
        localInfoOfItemUI.transform.GetChild(1).GetComponent<Image>().sprite = inventory[ChosenItem].Item.Icon;
        localInfoOfItemUI.transform.GetChild(2).GetComponent<Text>().text = inventory[ChosenItem].Item.Title;
    }
    public void DestroyInfoOfItem()
    {
        Destroy(localInfoOfItemUI);
    }
    public void InteractWithTollBet()
    {
        if (InvType == InventoryType.ToolBet)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && inventory[0].Item != null)
            {
                UseObjectInTollBet(0, inventory[0].Item);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && inventory[1].Item != null)
            {
                UseObjectInTollBet(1, inventory[1].Item);
            }
        }
    }
    public void UseObjectInTollBet(int numSlot, Item item)
    {
        switch (numSlot)
        {
            case 0:
                UseObjectItemType(numSlot, item);
                break;
            case 1:
                UseObjectItemType(numSlot, item);
                break;
        }
    }
    public void UseObjectItemType(int numSlot, Item item)
    {
        switch (item.ItemType)
        {
            case ItemType.Food:
                inventory[numSlot].Count -= 1;
                persGG.GetComponent<PlayerParameters>().localFoodPoints += (item as FoodItem).FoodPower;
                persGG.GetComponent<PlayerParameters>().localWaterPoints += (item as FoodItem).WaterPower;
                break;
            case ItemType.Heal:
                inventory[numSlot].Count -= 1;
                persGG.GetComponent<PlayerParameters>().localHitPoints += (item as HealItem).HealPower;
                break;
            case ItemType.Weapon:
                if (ObjInArm == null)
                {
                    chosenTollBetSlot = numSlot;
                    ObjInArm = Instantiate(inventory[numSlot].Item.WorldObj, persGG.transform.GetChild(1).GetChild(0));
                    ObjInArm.transform.localPosition = new Vector3(-0.002193f, -0.000732f, -0.000479f);
                    ObjInArm.transform.localEulerAngles = new Vector3(80, 0, 0);
                    ObjInArm.transform.localScale = new Vector3(0.008f, 0.008f, 0.008f);
                    ObjInArm.tag = "Weapon";

                    WeaponInfoInArm weaponInfoInArm = ObjInArm.AddComponent<WeaponInfoInArm>();
                    weaponInfoInArm.Damage = (item as WeaponItem).Damage;
                    weaponInfoInArm.Durability = (item as WeaponItem).MaxDurability;
                } 
                else
                {
                    chosenTollBetSlot = -1;
                    Destroy(ObjInArm);
                }
                break;
        }
        UpdateInventory();
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