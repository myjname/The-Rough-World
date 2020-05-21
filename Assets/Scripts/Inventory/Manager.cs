using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager
{
    private List<Item> IDB = new List<Item>(); //ItemsDataBase

    public List<Item> LoadDataBase() // содержит и загружет базу предметов
    {
        CreateHealItem("Bandage", 10, 1000, 10);

        CreateFoodItem("Apple", 10, 2000, 20, 5);

        CreateWeaponItem("Stick", 1, 5000, 10, 3);

        return IDB;
    }

    //Resources.Load<GameObject>(item.WorldObj)
    //Resources.Load<Sprite>(item.Icon)

    private void CreateHealItem(string title, int stackSize, int id, int healPower)// id 1000-1999
    {
        HealItem item = new HealItem { Title = title, StackSize = stackSize, Id = id, HealPower = healPower };

        item.Icon = "ItemSprite/" + id;
        if (item.Icon == null) item.Icon = "ItemSprite/Unknown";

        item.WorldObj = "ItemObj/" + id;
        if (item.WorldObj == null) item.WorldObj = "ItemObj/Unknown";

        IDB.Add(item);
    }
    private void CreateFoodItem(string title, int stackSize, int id, int foodPower, int waterPower)// id 2000-2999
    {
        FoodItem item = new FoodItem { Title = title, StackSize = stackSize, Id = id, FoodPower = foodPower, WaterPower = waterPower };

        item.Icon = "ItemSprite/" + id;
        if (item.Icon == null) item.Icon = "ItemSprite/Unknown";

        item.WorldObj = "ItemObj/" + id;
        if (item.WorldObj == null) item.WorldObj = "ItemObj/Unknown";

        IDB.Add(item);
    }
    private void CreateQuestItem(string title, int stackSize, int id)// id 3000-3999
    {
        QuestItem item = new QuestItem { Title = title, StackSize = stackSize, Id = id };

        item.Icon = "ItemSprite/" + id;
        if (item.Icon == null) item.Icon = "ItemSprite/Unknown";

        item.WorldObj = "ItemObj/" + id;
        if (item.WorldObj == null) item.WorldObj = "ItemObj/Unknown";

        IDB.Add(item);
    }
    private void CreateToolItem(string title, int stackSize, int id, int maxDurability, ResourceType resourceType)// id 4000-4999
    {
        ToolItem item = new ToolItem { Title = title, StackSize = stackSize, Id = id, MaxDurability = maxDurability, ResourceType = resourceType };

        item.Icon = "ItemSprite/" + id;
        if (item.Icon == null) item.Icon = "ItemSprite/Unknown";

        item.WorldObj = "ItemObj/" + id;
        if (item.WorldObj == null) item.WorldObj = "ItemObj/Unknown";

        IDB.Add(item);
    }
    private void CreateWeaponItem(string title, int stackSize, int id, int maxDurability, int damage)// id 5000-5999
    {
        WeaponItem item = new WeaponItem { Title = title, StackSize = stackSize, Id = id, MaxDurability = maxDurability, Damage = damage };

        item.Icon = "ItemSprite/" + id;
        if (item.Icon == null) item.Icon = "ItemSprite/Unknown";

        item.WorldObj = "ItemObj/" + id;
        if (item.WorldObj == null) item.WorldObj = "ItemObj/Unknown";

        IDB.Add(item);
    }
    private void CreateClothingItem(string title, int stackSize, int id, int maxDurability, int armor)// id 6000-6999
    {
        ClothingItem item = new ClothingItem { Title = title, StackSize = stackSize, Id = id, MaxDurability = maxDurability, Armor = armor };

        item.Icon = "ItemSprite/" + id;
        if (item.Icon == null) item.Icon = "ItemSprite/Unknown";

        item.WorldObj = "ItemObj/" + id;
        if (item.WorldObj == null) item.WorldObj = "ItemObj/Unknown";

        IDB.Add(item);
    }
    private void CreateResourceItem(string title, int stackSize, int id, ResourceType resourceType)// id 7000-7999
    {
        ResourceItem item = new ResourceItem { Title = title, StackSize = stackSize, Id = id, ResourceType = resourceType };

        item.Icon = "ItemSprite/" + id;
        if (item.Icon == null) item.Icon = "ItemSprite/Unknown";

        item.WorldObj = "ItemObj/" + id;
        if (item.WorldObj == null) item.WorldObj = "ItemObj/Unknown";

        IDB.Add(item);
    }
}

[Serializable]
public class Slot
{
    public Item Item { get; set; }
    public int Count { get; set; }
}

[Serializable]
public enum InventoryType
{
    MainInventory,
    ToolBet,
    Chest
}