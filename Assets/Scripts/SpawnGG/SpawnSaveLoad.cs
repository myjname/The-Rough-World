using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SpawnSaveLoad : MonoBehaviour
{
    public GameObject persGG;
    private GameObject localPersGG;

    public string nameOfSave = "MySave01";
    private string wayToFile;

    public Vector3 firstPoint = new Vector3(0, 3.3f, -57.6f);

    private SaveParametrs data = new SaveParametrs();

    void Start()
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

            SaveData();
        }
        FinishScript();
    }

    public void SaveData()
    {
        data.CharacterCoordinates = localPersGG.transform.position;
        data.CharacterRotation = localPersGG.transform.rotation;

        data.SceneIndex = SceneManager.GetActiveScene().buildIndex;

        File.WriteAllText(wayToFile, JsonUtility.ToJson(data));
    }

    public void LoadData()
    {
        data = JsonUtility.FromJson<SaveParametrs>(File.ReadAllText(wayToFile));

        localPersGG = Instantiate(persGG, data.CharacterCoordinates, data.CharacterRotation) as GameObject;
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