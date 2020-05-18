using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveParametrs//класс с сохранёнными данными
{
    public Vector3 CharacterCoordinates;
    public Quaternion CharacterRotation;

    public int SceneIndex;
}

[Serializable]
public class SaveMapSeed
{
    public int Seed;
}

[Serializable]
public class SaveQuests
{
    public List<QuestWithState> TakedQuests;
}

[Serializable]
public class SaveInventory
{
    public Slot[] ItemsAvailable;
}