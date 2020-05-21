using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SpawnSaveLoad : MonoBehaviour
{
    public GameObject persGG;
    private GameObject localPersGG;

    private Inventory MainInventory;
    private Inventory ToolBet;

    private PlayerParameters PlayerParameters;

    public string nameOfSave = "MySave01";
    private string wayToFile;

    public Vector3 firstPoint = new Vector3(0, 3.3f, -57.6f);

    private SaveParametrs data = new SaveParametrs();

    private void Start()
    {
        MainInventory = GetComponents<Inventory>()[0];
        ToolBet = GetComponents<Inventory>()[1];

        FirstInit();
    }

    private void FirstInit()
    {
        wayToFile = Path.Combine(Application.dataPath, "Saves/" + nameOfSave + "/SaveDataPersGG.json");

        if (File.Exists(wayToFile))
        {
            LoadData();
        }
        else
        {
            Directory.CreateDirectory(Application.dataPath + "/Saves/" + nameOfSave);//создаём папку где будут сейвы

            localPersGG = Instantiate(persGG, firstPoint, Quaternion.identity) as GameObject;

            PlayerParameters = localPersGG.GetComponent<PlayerParameters>();

            PlayerParameters.InitParameters();

            SaveData();
        }
        FinishScript();
    }

    public void SaveData()
    {
        data.CharacterCoordinates = localPersGG.transform.position;
        data.CharacterRotation = localPersGG.transform.rotation;

        data.SceneIndex = SceneManager.GetActiveScene().buildIndex;

        data.HP = PlayerParameters.localHitPoints;
        data.AP = PlayerParameters.localActionPoints;
        data.WP = PlayerParameters.localWaterPoints;
        data.FP = PlayerParameters.localFoodPoints;
        data.D = PlayerParameters.localDamage;

        File.WriteAllText(wayToFile, JsonUtility.ToJson(data));

        MainInventory.InventorySave();
        ToolBet.InventorySave();
    }

    public void LoadData()
    {
        data = JsonUtility.FromJson<SaveParametrs>(File.ReadAllText(wayToFile));

        localPersGG = Instantiate(persGG, data.CharacterCoordinates, data.CharacterRotation) as GameObject;

        PlayerParameters = localPersGG.GetComponent<PlayerParameters>();

        PlayerParameters.localHitPoints = data.HP;
        PlayerParameters.localActionPoints = data.AP;
        PlayerParameters.localWaterPoints = data.WP;
        PlayerParameters.localFoodPoints = data.FP;
        PlayerParameters.localDamage = data.D;
    }

    public void FinishScript()
    {
        GetComponent<CameraControl>().enabled = true;
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}