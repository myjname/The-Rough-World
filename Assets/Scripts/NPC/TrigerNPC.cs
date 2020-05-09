using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TrigerNPC : MonoBehaviour
{
    public NPCharacters npcCharacters;

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

        GameObject UI = GameObject.Find("Main Camera");
        MainScreen = UI.transform.GetChild(1);

        dialogUI = Resources.Load<GameObject>("UI/Dialog");
        dialogs = CDs.LoadDialog(npcCharacters);

        QH = UI.GetComponent<QuestHandler>();

        if (colider.tag == "Player")
        {
            SpawnDialog();
        }
    }
    private void OnTriggerExit(Collider colider)
    {
        DestroyDialog();
        numOfDialog = 0;
        CDs = new CreateDialogs();
    }

    private void SpawnDialog()
    {
        if (numOfDialog != 0) DestroyDialog();
        
        if (numOfDialog < dialogs.Count)
        {
            localDialogUI = Instantiate(dialogUI, MainScreen);

            localDialogUI.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { DestroyDialog(); });
            localDialogUI.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { SpawnDialog(); });

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
        wayToFile = Path.Combine(Application.dataPath, "Saves/" + nameOfSave + "/SaveQuestsData.json");

        QuestWithState qWS = new QuestWithState { Quest = quest, QuestState = QuestState.InProces };

        if (File.Exists(wayToFile))//Если файла есть
        {
            //SaveQs = JsonUtility.FromJson<SaveQuests>(File.ReadAllText(wayToFile));
            SaveQs = JsonConvert.DeserializeObject<SaveQuests>(File.ReadAllText(wayToFile), JsonSettings);
            QWSs = SaveQs.TakedQuests;
            QWSs.Add(qWS);
        }
        else
        {
            QWSs.Add(qWS);
        }

        //File.WriteAllText(wayToFile, JsonUtility.ToJson(new SaveQuests { TakedQuests = QWSs }));
        File.WriteAllText(wayToFile, JsonConvert.SerializeObject(new SaveQuests { TakedQuests = QWSs }, JsonSettings));
        QH.QuestUpdate();
    }
}