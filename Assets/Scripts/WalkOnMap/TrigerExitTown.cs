using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class TrigerExitTown : MonoBehaviour
{
    public Vector3 coordinate;
    public int sceneIndex = 2;

    public GameObject MainScreen;
    public GameObject dialog;

    public Font font;

    private SaveParametrs parametrs = new SaveParametrs();

    private string wayToFile;
    private string nameOfSave = "MySave01";

    private GameObject localDialog;

    private void OnTriggerEnter(Collider colider)
    {
        if (colider.tag == "Player")
        {
            Time.timeScale = 0;
            SpawnDialog();
        }
    }

    //ToDo: Сделать так же но через префаб и ресурсы
    private void SpawnDialog()
    {
        localDialog = Instantiate(dialog, MainScreen.transform);

        CreateButtonYes();

        CreateButtonNo();
    }
    private void CreateButtonYes()
    {
        GameObject buttonYes = new GameObject("ButtonYes", typeof(Image), typeof(Button), typeof(LayoutElement));
        buttonYes.GetComponent<LayoutElement>().minHeight = 1;
        buttonYes = Instantiate(buttonYes, localDialog.transform);
        buttonYes.GetComponent<Button>().onClick.AddListener(delegate { DialogYes(); });

        GameObject newText = new GameObject("YesText", typeof(Text));
        newText = Instantiate(newText, buttonYes.transform);
        newText.GetComponent<Text>().text = "Да";
        newText.GetComponent<Text>().font = font;
        newText.GetComponent<Text>().color = new Color(0, 0, 0);

        RectTransform textRT = newText.GetComponent<RectTransform>();
        textRT.anchorMin = new Vector2(0, 0);
        textRT.anchorMax = new Vector2(1, 1);
        textRT.sizeDelta = new Vector2(0, 0);
        textRT.anchoredPosition = new Vector2(0, 0);
    }
    private void CreateButtonNo()
    {
        GameObject buttonYes = new GameObject("ButtonNo", typeof(Image), typeof(Button), typeof(LayoutElement));
        buttonYes.GetComponent<LayoutElement>().minHeight = 1;
        buttonYes = Instantiate(buttonYes, localDialog.transform);
        buttonYes.GetComponent<Button>().onClick.AddListener(delegate { DialogNo(); });

        GameObject newText = new GameObject("YesText", typeof(Text));
        newText = Instantiate(newText, buttonYes.transform);
        newText.GetComponent<Text>().text = "Нет";
        newText.GetComponent<Text>().font = font;
        newText.GetComponent<Text>().color = new Color(0, 0, 0);

        RectTransform textRT = newText.GetComponent<RectTransform>();
        textRT.anchorMin = new Vector2(0, 0);
        textRT.anchorMax = new Vector2(1, 1);
        textRT.sizeDelta = new Vector2(0, 0);
        textRT.anchoredPosition = new Vector2(0, 0);
    }

    public void DialogNo()
    {
        Time.timeScale = 1;

        Destroy(localDialog);
    }
    public void DialogYes()
    {
        Time.timeScale = 1;

        Destroy(localDialog);

        wayToFile = Path.Combine(Application.dataPath, "Saves/" + nameOfSave + "/SaveDataPersGG.json");//путь к файлу сохранения

        parametrs.CharacterCoordinates = coordinate;
        parametrs.SceneIndex = sceneIndex;

        File.WriteAllText(wayToFile, JsonUtility.ToJson(parametrs));//записываем всё в файл

        wayToFile = Path.Combine(Application.dataPath, "Saves/" + nameOfSave + "/SaveMapSeed.json");

        if (File.Exists(wayToFile))
        {
            File.Delete(wayToFile);
        }

        SceneManager.LoadScene(sceneIndex);
    }
}
