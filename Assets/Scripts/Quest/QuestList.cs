using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestList
{
    List<Quest> quests;

    public List<Quest> LoadQuests()
    {
        quests = new List<Quest>();

        CreateTravelQuest("FirstTravel", "Дойти до точки", new Vector3(38.4f, 3.2f, -32f), 1);
        CreateTravelQuest("SecondTravel", "Дойти до точки", new Vector3(30, 0, 30), 2);

        return quests;
    }

    private void CreateTravelQuest(string questName, string descriptionGoal, Vector3 coord, int sceneID)
    {
        TravelQuest quest = new TravelQuest { QuestName = questName, DescriptionGoal = descriptionGoal, Coord = coord, SceneID = sceneID };

        quests.Add(quest);
    }
}