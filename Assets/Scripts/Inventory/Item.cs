using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public string Title { get; set; }
    public int StackSize { get; set; }
    public int Id { get; set; }
    public string Icon { get; set; }
    public string WorldObj { get; set; }
    public ItemType ItemType { get; set; }
}

[Serializable]
public class HealItem : Item
{
    public int HealPower { get; set; }

    public HealItem()
    {
        ItemType = ItemType.Heal;
    }
}

[Serializable]
public class FoodItem : Item
{
    public int FoodPower { get; set; }
    public int WaterPower { get; set; }

    public FoodItem()
    {
        ItemType = ItemType.Food;
    }
}

[Serializable]
public class QuestItem : Item
{
    //пока не уверен что тут должно быть

    public QuestItem()
    {
        ItemType = ItemType.Quest;
    }
}

[Serializable]
public class ToolItem : Item
{
    public int MaxDurability { get; set; }
    public ResourceType ResourceType { get; set; }

    public ToolItem()
    {
        ItemType = ItemType.Tool;
    }
}

[Serializable]
public class WeaponItem : Item
{
    public int MaxDurability { get; set; }
    public int Damage { get; set; }

    public WeaponItem()
    {
        ItemType = ItemType.Weapon;
    }
}

[Serializable]
public class ClothingItem : Item
{
    public int MaxDurability { get; set; }
    public int Armor { get; set; }

    public ClothingItem()
    {
        ItemType = ItemType.Clothing;
    }
}

[Serializable]
public class ResourceItem : Item
{
    public ResourceType ResourceType { get; set; }

    public ResourceItem()
    {
        ItemType = ItemType.Resource;
    }
}

[Serializable]
public enum ItemType
{
    Heal,
    Food,
    Quest,
    Tool,
    Weapon,
    Clothing,
    Resource
}

[Serializable]
public enum ResourceType
{
    None,
    Wood,
    Metal
}