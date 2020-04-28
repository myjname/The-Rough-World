using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraUI : MonoBehaviour
{
    public GameObject EscMenu;
    private bool activated = false;

    private Inventory MainInventory;
    private Inventory ToolBet;

    private void Start()
    {
        GameObject UI = GameObject.Find("Main Camera");
        EscMenu = UI.transform.Find("MainScreen/EscMenu").gameObject;

        MainInventory = GetComponents<Inventory>()[0];
        ToolBet = GetComponents<Inventory>()[1];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && MainInventory.IsDragging == false)
        {
            ActivateEscMenu();
        }
    }

    //Открыть меню
    public void ActivateEscMenu()
    {
        if (activated)
        {
            EscMenu.gameObject.SetActive(false);//закрыть меню на esc
            Time.timeScale = 1;
            activated = false;
        }
        else
        {
            EscMenu.gameObject.SetActive(true);//активация меню на esc
            Time.timeScale = 0;
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
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneIndex);
    }
}