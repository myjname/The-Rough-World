using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    private string wayToFile;
    public string nameOfSave = "MySave01";

    private SaveParametrs parametrs = new SaveParametrs();

    //Выход из игры
    public void ExitGame()
    {
        Application.Quit();
    }
    
    //Смена сцены
    public void SceneLoad(int SceneIndex)
    {
        wayToFile = Path.Combine(Application.dataPath, "Saves/" + nameOfSave + "/SaveDataPersGG.json");//путь к файлу сохранения

        if (File.Exists(wayToFile))//если файл существует
        {
            parametrs = JsonUtility.FromJson<SaveParametrs>(File.ReadAllText(wayToFile));//присваеваем классу сохранённые данные из файла

            SceneManager.LoadScene(parametrs.SceneIndex);
        }
        else
        {
            SceneManager.LoadScene(SceneIndex);
        }
    }
}
