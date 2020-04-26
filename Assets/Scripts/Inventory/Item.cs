using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string Title { get; set; }
    public int StackSize { get; set; }
    public int Id { get; set; }
    public Sprite Icon { get; set; }
    public GameObject WorldObj { get; set; }
    public ItemType ItemType { get; set; }
}

public class HealItem : Item
{
    public int HealPower { get; set; }

    public HealItem()
    {
        ItemType = ItemType.Heal;
    }
}

public class FoodItem : Item
{
    public int FoodPower { get; set; }
    public int WaterPower { get; set; }

    public FoodItem()
    {
        ItemType = ItemType.Food;
    }
}

public class QuestItem : Item
{
    //пока не уверен что тут должно быть

    public QuestItem()
    {
        ItemType = ItemType.Quest;
    }
}

public class ToolItem : Item
{
    public int MaxDurability { get; set; }
    public ResourceType ResourceType { get; set; }

    public ToolItem()
    {
        ItemType = ItemType.Tool;
    }
}

public class WeaponItem : Item
{
    public int MaxDurability { get; set; }
    public int Damage { get; set; }

    public WeaponItem()
    {
        ItemType = ItemType.Weapon;
    }
}

public class ClothingItem : Item
{
    public int MaxDurability { get; set; }
    public int Armor { get; set; }

    public ClothingItem()
    {
        ItemType = ItemType.Clothing;
    }
}

public class ResourceItem : Item
{
    public ResourceType ResourceType { get; set; }

    public ResourceItem()
    {
        ItemType = ItemType.Resource;
    }
}

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

public enum ResourceType
{
    None,
    Wood,
    Metal
}