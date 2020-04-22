using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager
{
    private List<Item> IDB = new List<Item>(); //ItemsDataBase

    //public int StackSize = 10;

    public List<Item> LoadDataBase() // содержит и загружет базу предметов
    {
        CreateHealItem("Bandage", true, 1000, 10);

        CreateFoodItem("Apple", true, 2000, 20, 5);

        CreateWeaponItem("Stick", false, 5000, 10, 3);

        return IDB;
    }

    private void CreateHealItem(string title, bool canStack, int id, int healPower)// id 1000-1999
    {
        HealItem item = new HealItem { Title = title, CanStack = canStack, Id = id, HealPower = healPower };

        item.Icon = Resources.Load<Sprite>("ItemSprite/" + id);
        if (item.Icon == null) item.Icon = Resources.Load<Sprite>("ItemSprite/Unknown");

        item.WorldObj = Resources.Load<GameObject>("ItemObj/" + id);
        if (item.WorldObj == null) item.WorldObj = Resources.Load<GameObject>("ItemObj/Unknown");

        IDB.Add(item);
    }

    private void CreateFoodItem(string title, bool canStack, int id, int foodPower, int waterPower)// id 2000-2999
    {
        FoodItem item = new FoodItem { Title = title, CanStack = canStack, Id = id, FoodPower = foodPower, WaterPower = waterPower };

        item.Icon = Resources.Load<Sprite>("ItemSprite/" + id);
        if (item.Icon == null) item.Icon = Resources.Load<Sprite>("ItemSprite/Unknown");

        item.WorldObj = Resources.Load<GameObject>("ItemObj/" + id);
        if (item.WorldObj == null) item.WorldObj = Resources.Load<GameObject>("ItemObj/Unknown");

        IDB.Add(item);
    }

    private void CreateQuestItem(string title, bool canStack, int id)// id 3000-3999
    {
        QuestItem item = new QuestItem { Title = title, CanStack = canStack, Id = id };

        item.Icon = Resources.Load<Sprite>("ItemSprite/" + id);
        if (item.Icon == null) item.Icon = Resources.Load<Sprite>("ItemSprite/Unknown");

        item.WorldObj = Resources.Load<GameObject>("ItemObj/" + id);
        if (item.WorldObj == null) item.WorldObj = Resources.Load<GameObject>("ItemObj/Unknown");

        IDB.Add(item);
    }

    private void CreateToolItem(string title, bool canStack, int id, int maxDurability, ResourceType resourceType)// id 4000-4999
    {
        ToolItem item = new ToolItem { Title = title, CanStack = canStack, Id = id, MaxDurability = maxDurability, ResourceType = resourceType };

        item.Icon = Resources.Load<Sprite>("ItemSprite/" + id);
        if (item.Icon == null) item.Icon = Resources.Load<Sprite>("ItemSprite/Unknown");

        item.WorldObj = Resources.Load<GameObject>("ItemObj/" + id);
        if (item.WorldObj == null) item.WorldObj = Resources.Load<GameObject>("ItemObj/Unknown");

        IDB.Add(item);
    }

    private void CreateWeaponItem(string title, bool canStack, int id, int maxDurability, int damage)// id 5000-5999
    {
        WeaponItem item = new WeaponItem { Title = title, CanStack = canStack, Id = id, MaxDurability = maxDurability, Damage = damage };

        item.Icon = Resources.Load<Sprite>("ItemSprite/" + id);
        if (item.Icon == null) item.Icon = Resources.Load<Sprite>("ItemSprite/Unknown");

        item.WorldObj = Resources.Load<GameObject>("ItemObj/" + id);
        if (item.WorldObj == null) item.WorldObj = Resources.Load<GameObject>("ItemObj/Unknown");

        IDB.Add(item);
    }

    private void CreateClothingItem(string title, bool canStack, int id, int maxDurability, int armor)// id 6000-6999
    {
        ClothingItem item = new ClothingItem { Title = title, CanStack = canStack, Id = id, MaxDurability = maxDurability, Armor = armor };

        item.Icon = Resources.Load<Sprite>("ItemSprite/" + id);
        if (item.Icon == null) item.Icon = Resources.Load<Sprite>("ItemSprite/Unknown");

        item.WorldObj = Resources.Load<GameObject>("ItemObj/" + id);
        if (item.WorldObj == null) item.WorldObj = Resources.Load<GameObject>("ItemObj/Unknown");

        IDB.Add(item);
    }

    private void CreateResourceItem(string title, bool canStack, int id, ResourceType resourceType)// id 7000-7999
    {
        ResourceItem item = new ResourceItem { Title = title, CanStack = canStack, Id = id, ResourceType = resourceType };

        item.Icon = Resources.Load<Sprite>("ItemSprite/" + id);
        if (item.Icon == null) item.Icon = Resources.Load<Sprite>("ItemSprite/Unknown");

        item.WorldObj = Resources.Load<GameObject>("ItemObj/" + id);
        if (item.WorldObj == null) item.WorldObj = Resources.Load<GameObject>("ItemObj/Unknown");

        IDB.Add(item);
    }
}


