using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveParametersOfGG : MonoBehaviour
{
    //public GameObject Camera;
    public GameObject PersGG;

    private SaveParametrs parametrs = new SaveParametrs();

    private string wayToFile;
    public string nameOfSave = "MySave01";

    private void Start()
    {
        Directory.CreateDirectory(Application.dataPath + "/Saves/" + nameOfSave);//создаём папку с где будет сейв

        wayToFile = Path.Combine(Application.dataPath, "Saves/" + nameOfSave + "/SaveParametrsOfGG.json");//путь к файлу сохранения

        if (File.Exists(wayToFile))//если файл существует
        {
            parametrs = JsonUtility.FromJson<SaveParametrs>(File.ReadAllText(wayToFile));//присваеваем классу сохранённые данные из файла

            PersGG.transform.position = parametrs.CharacterCoordinates;//присваеваем объекту его координаты
            PersGG.transform.eulerAngles = parametrs.CharacterRotation;
        }
        else
        {
            SaveButton();
        }
    }

    public void SaveButton()//сохраняем координаты персонажа
    {
        parametrs.CharacterCoordinates = PersGG.transform.position;
        parametrs.CharacterRotation = PersGG.transform.eulerAngles;
        File.WriteAllText(wayToFile, JsonUtility.ToJson(parametrs));
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
}