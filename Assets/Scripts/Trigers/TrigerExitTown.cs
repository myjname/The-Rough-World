using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TrigerExitTown : MonoBehaviour
{
    public GameObject dialog;

    public Vector3 coordinate;
    public int sceneIndex = 2;

    private SaveParametrs parametrs = new SaveParametrs();

    private string wayToFile;
    private string nameOfSave = "MySave01";

    private void OnTriggerEnter(Collider colider)
    {
        if (colider.tag == "Player")
        {
            Debug.Log("Вы хотите выйти?");
            Time.timeScale = 0;
            dialog.gameObject.SetActive(true);
        }
    }

    public void DialogNo()
    {
        Time.timeScale = 1;

        dialog.gameObject.SetActive(false);
    }

    public void DialogYes()
    {
        Time.timeScale = 1;

        dialog.gameObject.SetActive(false);

        wayToFile = Path.Combine(Application.dataPath, "Saves/" + nameOfSave + "/SaveParametrsOfGG.json");//путь к файлу сохранения

        parametrs.CharacterCoordinates = coordinate;
        parametrs.SceneIndex = sceneIndex;

        File.WriteAllText(wayToFile, JsonUtility.ToJson(parametrs));//записываем всё в файл

        SceneManager.LoadScene(sceneIndex);
    }
}
