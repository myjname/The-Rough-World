using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager
{
    [HideInInspector]
    public List<Item> IDB = new List<Item>(); //ItemsDataBase

    public int StackSize = 10;

    public void LoadDataBase()
    {

    }

    public void CreateHealItem(string title, bool canStack, int id, Sprite icon, GameObject worldObj, int healPower)
    {
        HealItem item = new HealItem { Title = title, CanStack = canStack, Id = id, HealPower = healPower };

        item.Icon = Resources.Load<Sprite>("ItemSprite/" + id);
        if (item.Icon == null) item.Icon = Resources.Load<Sprite>("ItemSprite/Unknown");

        item.WorldObj = Resources.Load<GameObject>("ItemObj/" + id);
        if (item.WorldObj == null) item.WorldObj = Resources.Load<GameObject>("ItemObj/Unknown");

        IDB.Add(item);
    }

    public void CreateFoodItem(string title, bool canStack, int id, Sprite icon, GameObject worldObj, int foodPower, int waterPower)
    {
        FoodItem item = new FoodItem { Title = title, CanStack = canStack, Id = id, FoodPower = foodPower, WaterPower = waterPower };

        item.Icon = Resources.Load<Sprite>("ItemSprite/" + id);
        if (item.Icon == null) item.Icon = Resources.Load<Sprite>("ItemSprite/Unknown");

        item.WorldObj = Resources.Load<GameObject>("ItemObj/" + id);
        if (item.WorldObj == null) item.WorldObj = Resources.Load<GameObject>("ItemObj/Unknown");

        IDB.Add(item);
    }

    public void CreateQuestItem(string title, bool canStack, int id, Sprite icon, GameObject worldObj)
    {
        QuestItem item = new QuestItem { Title = title, CanStack = canStack, Id = id };

        item.Icon = Resources.Load<Sprite>("ItemSprite/" + id);
        if (item.Icon == null) item.Icon = Resources.Load<Sprite>("ItemSprite/Unknown");

        item.WorldObj = Resources.Load<GameObject>("ItemObj/" + id);
        if (item.WorldObj == null) item.WorldObj = Resources.Load<GameObject>("ItemObj/Unknown");

        IDB.Add(item);
    }

    public void CreateToolItem(string title, bool canStack, int id, Sprite icon, GameObject worldObj, int maxDurability, ResourceType resourceType)
    {
        ToolItem item = new ToolItem { Title = title, CanStack = canStack, Id = id, MaxDurability = maxDurability, ResourceType = resourceType };

        item.Icon = Resources.Load<Sprite>("ItemSprite/" + id);
        if (item.Icon == null) item.Icon = Resources.Load<Sprite>("ItemSprite/Unknown");

        item.WorldObj = Resources.Load<GameObject>("ItemObj/" + id);
        if (item.WorldObj == null) item.WorldObj = Resources.Load<GameObject>("ItemObj/Unknown");

        IDB.Add(item);
    }

    public void CreateWeaponItem(string title, bool canStack, int id, Sprite icon, GameObject worldObj, int maxDurability, int damage)
    {
        WeaponItem item = new WeaponItem { Title = title, CanStack = canStack, Id = id, MaxDurability = maxDurability, Damage = damage };

        item.Icon = Resources.Load<Sprite>("ItemSprite/" + id);
        if (item.Icon == null) item.Icon = Resources.Load<Sprite>("ItemSprite/Unknown");

        item.WorldObj = Resources.Load<GameObject>("ItemObj/" + id);
        if (item.WorldObj == null) item.WorldObj = Resources.Load<GameObject>("ItemObj/Unknown");

        IDB.Add(item);
    }

    public void CreateClothingItem(string title, bool canStack, int id, Sprite icon, GameObject worldObj, int maxDurability, int armor)
    {
        ClothingItem item = new ClothingItem { Title = title, CanStack = canStack, Id = id, MaxDurability = maxDurability, Armor = armor };

        item.Icon = Resources.Load<Sprite>("ItemSprite/" + id);
        if (item.Icon == null) item.Icon = Resources.Load<Sprite>("ItemSprite/Unknown");

        item.WorldObj = Resources.Load<GameObject>("ItemObj/" + id);
        if (item.WorldObj == null) item.WorldObj = Resources.Load<GameObject>("ItemObj/Unknown");

        IDB.Add(item);
    }

    public void CreateResourceItem(string title, bool canStack, int id, Sprite icon, GameObject worldObj, ResourceType resourceType)
    {
        ResourceItem item = new ResourceItem { Title = title, CanStack = canStack, Id = id, ResourceType = resourceType };

        item.Icon = Resources.Load<Sprite>("ItemSprite/" + id);
        if (item.Icon == null) item.Icon = Resources.Load<Sprite>("ItemSprite/Unknown");

        item.WorldObj = Resources.Load<GameObject>("ItemObj/" + id);
        if (item.WorldObj == null) item.WorldObj = Resources.Load<GameObject>("ItemObj/Unknown");

        IDB.Add(item);
    }
}


