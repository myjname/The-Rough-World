using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraUI : MonoBehaviour
{
    public GameObject Panel;
    private bool activated = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivateEscMenu();
        }
    }

    //Открыть меню
    public void ActivateEscMenu()
    {
        if (activated)
        {
            Panel.gameObject.SetActive(false);
            activated = false;
        }
        else
        {
            Panel.gameObject.SetActive(true);
            activated = true;
        }

    }

    //Выход из игры
    public void ExitGame()
    {
        Application.Quit();
    }

    //Смена сцены
    public void SceneLoad(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }


}
