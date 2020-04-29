using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Quest
{
    public List<Dialog> dialogs;
    public QuestStatus QStatus;
    public QuestGoal QGoal;
}

public class CollectionQuest : Quest
{
    public int count;
    public Item item;

    public CollectionQuest()
    {
        QGoal = QuestGoal.Collection;
    }
}

public class KillQuest : Quest
{
    public int count;
    public Item item;

    public KillQuest()
    {
        QGoal = QuestGoal.Kill;
    }
}

public class TalkQuest : Quest
{
    public int count;
    public Item item;

    public TalkQuest()
    {
        QGoal = QuestGoal.Talk;
    }
}

public enum QuestStatus
{
    NotTaken,
    InProces,
    Complete
}

public enum QuestGoal
{
    Collection,
    Kill,
    Talk
}