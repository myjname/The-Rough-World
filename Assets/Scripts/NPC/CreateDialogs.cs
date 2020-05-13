using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Чтобы добавить диалог нужно:
// • В enum добавить "имя" персонажа
// • В конструкции "switch (npc)" создать диалог
// После этого его можно будет вызвать в TrigerNPC

public class CreateDialogs
{
    private List<Dialog> Dialogs;
    private List<Quest> Quests;

    QuestList QList = new QuestList();

    public List<Dialog> LoadDialog(NPCharacters npc)
    {
        Dialogs = new List<Dialog>();
        Quests = QList.LoadQuests();
        string speaker;

        switch (npc)
        {
            case NPCharacters.FirstNPC:
                speaker = "FirstNPC";
                CreateDialog(speaker, "Ну здарова!");
                CreateDialog(speaker, "Квестов нет...");
                CreateDialog(speaker, "Мойте руки, это сейчас актуально.");
                break;
            case NPCharacters.FirstNPCQuest:
                speaker = "SecondNPS";
                CreateDialog(speaker, "...");
                CreateDialog(speaker, "Есть квест");
                CreateDialog(speaker, "...");
                CreateDialog(speaker, "Хочешь?", "FirstTravel");
                break;
            case NPCharacters.FirstNPCCong:
                speaker = "SecondNPS";
                CreateDialog(speaker, "Спасибо, что выполнил квест!");
                break;
        }
        
        return Dialogs;
    }

    private void CreateDialog(string speaker, string text)
    {
        Dialog dialog = new Dialog { Speaker = speaker, Text = text, Quest = null };
        Dialogs.Add(dialog);
    }
    private void CreateDialog(string speaker, string text, string questName)
    {
        Dialog dialog = new Dialog { Speaker = speaker, Text = text, Quest = FindQuest(questName) };
        Dialogs.Add(dialog);
    }

    private Quest FindQuest(string questName)
    {
        for (int i = 0; i < Quests.Count; i++) if (Quests[i].QuestName == questName) return Quests[i];
        return null;
    }
}

public enum NPCharacters
{
    FirstNPC,
    FirstNPCQuest,
    FirstNPCCong
}