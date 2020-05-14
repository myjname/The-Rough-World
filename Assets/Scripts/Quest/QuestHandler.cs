using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestHandler : MonoBehaviour
{
    private List<QuestWithState> QWSs;

    private SaveQuests SaveQs = new SaveQuests();

    private string wayToFile;
    private string nameOfSave = "MySave01";

    private GameObject UI;
    private GameObject QuestUI;
    private GameObject QuestListUI;
    private GameObject LocalQuestUI;

    private GameObject persGG;

    private bool search = true;

    private static JsonSerializerSettings JsonSettings = new JsonSerializerSettings // позволяет сериализовать класс, запоминая разные классы в листе
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private void Start()
    {
        UI = GameObject.Find("Main Camera");
        QuestListUI = UI.transform.Find("MainScreen/QuestList").gameObject;
        QuestUI = Resources.Load<GameObject>("UI/QuestUI");

        wayToFile = Path.Combine(Application.dataPath, "Saves/" + nameOfSave + "/SaveQuestsData.json");
    }

    private void Update()
    {
        while (search)
        {
            persGG = GameObject.FindGameObjectWithTag("Player");

            QuestUpdate();

            StartCoroutine(EnumUpdate());

            search = false;
        }
    }

    private IEnumerator EnumUpdate()
    {
        while (true)
        {
            CheckQuests();

            yield return new WaitForSeconds(3);
        }
    }

    public void QuestUpdate()
    {
        if (File.Exists(wayToFile))
        {
            SaveQs = JsonConvert.DeserializeObject<SaveQuests>(File.ReadAllText(wayToFile), JsonSettings);

            foreach (Transform child in QuestListUI.transform) Destroy(child.gameObject);//чистим объект от квестов

            for (int i = 0; i < SaveQs.TakedQuests.Count; i++)
            {
                if (SaveQs.TakedQuests[i].QuestState != QuestState.Passed)
                {
                    LocalQuestUI = Instantiate(QuestUI, QuestListUI.transform);

                    LocalQuestUI.transform.GetChild(0).GetComponent<Text>().text = SaveQs.TakedQuests[i].Quest.QuestName;
                    LocalQuestUI.transform.GetChild(1).GetComponent<Text>().text = SaveQs.TakedQuests[i].Quest.DescriptionGoal;
                    LocalQuestUI.transform.GetChild(2).GetComponent<Text>().text = SaveQs.TakedQuests[i].QuestState.ToString();
                }
            }
        }
    }

    private void CheckQuests()
    {
        if (File.Exists(wayToFile))
        {
            for (int i = 0; i < SaveQs.TakedQuests.Count; i++)
            {
                if (SaveQs.TakedQuests.Count > 0 && SaveQs.TakedQuests[i].QuestState == QuestState.InProces)
                {
                    switch (SaveQs.TakedQuests[i].Quest.QGoal)
                    {
                        case QuestGoal.Collection:
                            break;
                        case QuestGoal.Kill:
                            break;
                        case QuestGoal.Talk:
                            break;
                        case QuestGoal.Travel:
                            TravelQuest TQuest = SaveQs.TakedQuests[i].Quest as TravelQuest;
                            if (persGG.transform.position.x < TQuest.Coord.x + 2 &&
                                persGG.transform.position.x > TQuest.Coord.x - 2 &&
                                persGG.transform.position.y < TQuest.Coord.y + 2 &&
                                persGG.transform.position.y > TQuest.Coord.y - 2 &&
                                persGG.transform.position.z < TQuest.Coord.z + 2 &&
                                persGG.transform.position.z > TQuest.Coord.z - 2 &&
                                SceneManager.GetActiveScene().buildIndex == TQuest.SceneID)
                            {
                                SaveQs.TakedQuests[i].QuestState = QuestState.Complete;
                                File.WriteAllText(wayToFile, JsonConvert.SerializeObject(SaveQs, JsonSettings));
                            }
                            break;
                    }
                }
            }
            QuestUpdate();
        }
    }
}