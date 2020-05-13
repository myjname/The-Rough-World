using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TrigerNPC : MonoBehaviour
{
    public List<NPCharacters> npcCharacters;
    private int numCharacter = -1;

    private List<Dialog> dialogs = new List<Dialog>();
    private int numOfDialog = 0;

    private CreateDialogs CDs = new CreateDialogs();

    private Transform MainScreen;
    private GameObject dialogUI;
    private GameObject localDialogUI;

    private List<QuestWithState> QWSs = new List<QuestWithState>();

    private SaveQuests SaveQs = new SaveQuests();

    private string wayToFile;
    private string nameOfSave = "MySave01";

    private QuestHandler QH;

    private static JsonSerializerSettings JsonSettings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private void OnTriggerEnter(Collider colider)
    {
        dialogs = new List<Dialog>();

        wayToFile = Path.Combine(Application.dataPath, "Saves/" + nameOfSave + "/SaveQuestsData.json");

        GameObject UI = GameObject.Find("Main Camera");
        MainScreen = UI.transform.GetChild(1);

        dialogUI = Resources.Load<GameObject>("UI/Dialog");

        QH = UI.GetComponent<QuestHandler>();

        dialogs = CDs.LoadDialog(npcCharacters[1]);

        if (colider.tag == "Player")
        {
            if (File.Exists(wayToFile))
            {
                SaveQs = JsonConvert.DeserializeObject<SaveQuests>(File.ReadAllText(wayToFile), JsonSettings);

                for (int i = 0; i < SaveQs.TakedQuests.Count; i++)
                {
                    if (SaveQs.TakedQuests[i].Quest.QuestName == dialogs[dialogs.Count - 1].Quest.QuestName)
                    {
                        if (SaveQs.TakedQuests[i].QuestState == QuestState.Complete)
                        {
                            if (SaveQs.TakedQuests[i].QuestState == QuestState.Passed)
                            {
                                numCharacter = 0;
                            }
                            else
                            {
                                SaveQs.TakedQuests[i].QuestState = QuestState.Passed;
                                File.WriteAllText(wayToFile, JsonConvert.SerializeObject(SaveQs, JsonSettings));
                                numCharacter = 2;
                            }
                            break;
                        }
                        else
                        {
                            numCharacter = 0;
                            break;
                        }
                    }
                    else numCharacter = 1;
                }
            }
            else numCharacter = 1;

            if (numCharacter >= 0)
            {
                SpawnDialog(npcCharacters[numCharacter]);
            }
            
        }
    }
    private void OnTriggerExit(Collider colider)
    {
        DestroyDialog();
        numOfDialog = 0;
        CDs = new CreateDialogs();
    }

    private void SpawnDialog(NPCharacters npcCharacter)
    {
        dialogs = CDs.LoadDialog(npcCharacter);
        if (numOfDialog != 0) DestroyDialog();

        if (numOfDialog < dialogs.Count)
        {
            localDialogUI = Instantiate(dialogUI, MainScreen);

            localDialogUI.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { DestroyDialog(); });
            localDialogUI.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { SpawnDialog(npcCharacters[numCharacter]); });

            localDialogUI.transform.GetChild(2).GetComponent<Text>().text = dialogs[numOfDialog].Text;
            localDialogUI.transform.GetChild(3).GetComponent<Text>().text = dialogs[numOfDialog].Speaker;

            numOfDialog++;
        }
        else
        {
            if (dialogs[numOfDialog - 1].Quest != null) AddQuest(dialogs[numOfDialog - 1].Quest);

            DestroyDialog();
            numOfDialog = 0;
        }
    }
    private void DestroyDialog()
    {
        Destroy(localDialogUI);
    }

    public void AddQuest(Quest quest)
    {
        QuestWithState qWS = new QuestWithState { Quest = quest, QuestState = QuestState.InProces };

        if (File.Exists(wayToFile))//Если файла есть
        {
            SaveQs = JsonConvert.DeserializeObject<SaveQuests>(File.ReadAllText(wayToFile), JsonSettings);
            QWSs = SaveQs.TakedQuests;
            QWSs.Add(qWS);
        }
        else
        {
            QWSs.Add(qWS);
        }
        File.WriteAllText(wayToFile, JsonConvert.SerializeObject(new SaveQuests { TakedQuests = QWSs }, JsonSettings));
        QH.QuestUpdate();
    }
}