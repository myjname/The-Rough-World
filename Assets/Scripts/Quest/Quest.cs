using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Quest
{
    public string QuestName;
    public string DescriptionGoal;
    public QuestGoal QGoal;
}

[Serializable]
public class CollectionQuest : Quest
{
    public int Count;
    public Item Item;

    public CollectionQuest()
    {
        QGoal = QuestGoal.Collection;
    }
}

[Serializable]
public class KillQuest : Quest
{
    public int Count;
    public Item Item;

    public KillQuest()
    {
        QGoal = QuestGoal.Kill;
    }
}

[Serializable]
public class TalkQuest : Quest
{
    public int Count;
    public Item Item;

    public TalkQuest()
    {
        QGoal = QuestGoal.Talk;
    }
}

[Serializable]
public class TravelQuest : Quest
{
    public Vector3 Coord;
    public int SceneID;

    public TravelQuest()
    {
        QGoal = QuestGoal.Travel;
    }
}

[Serializable]
public enum QuestGoal
{
    Collection,
    Kill,
    Talk,
    Travel
}


[Serializable]
public class QuestWithState
{
    public Quest Quest;
    public QuestState QuestState;
}

[Serializable]
public enum QuestState
{
    NotTaken,
    InProces,
    Complete,
    Passed
}