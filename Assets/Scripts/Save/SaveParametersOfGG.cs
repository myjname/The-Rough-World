using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveParametersOfGG : MonoBehaviour
{
    //public GameObject Camera;
    public GameObject PersGG;

    private SaveParametrs parametrs = new SaveParametrs();

    private string wayToFile;
    public string nameOfSave = "MySave01";

    private void Start()
    {
        Directory.CreateDirectory(Application.dataPath + "/Saves/" + nameOfSave);//создаём папку где будет сейв

        wayToFile = Path.Combine(Application.dataPath, "Saves/" + nameOfSave + "/SaveParametrsOfGG.json");//путь к файлу сохранения

        if (File.Exists(wayToFile))//если файл существует
        {
            SetParametrs();
        }
        else
        {
            SaveButton();
        }
    }

    public void SetParametrs()//задаём параметры при запуске
    {
        parametrs = JsonUtility.FromJson<SaveParametrs>(File.ReadAllText(wayToFile));//присваеваем классу сохранённые данные из файла

        PersGG.transform.position = parametrs.CharacterCoordinates;//присваеваем объекту его координаты
        PersGG.transform.eulerAngles = parametrs.CharacterRotation;//вращение
    }

    public void SaveButton()//сохраняем параметры персонажа
    {
        parametrs.CharacterCoordinates = PersGG.transform.position;
        parametrs.CharacterRotation = PersGG.transform.eulerAngles;

        parametrs.SceneIndex = SceneManager.GetActiveScene().buildIndex;

        File.WriteAllText(wayToFile, JsonUtility.ToJson(parametrs));//записываем всё в файл
    }

    private void OnApplicationQuit()//сохранение файла при выходе
    {
        File.WriteAllText(wayToFile, JsonUtility.ToJson(parametrs));
    }
}

[Serializable]
public class SaveParametrs//класс с сохранёнными данными
{
    public Vector3 CharacterCoordinates;
    public Vector3 CharacterRotation;

    public int SceneIndex;
}