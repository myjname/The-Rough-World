using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CameraUI : MonoBehaviour
{
    private GameObject EscMenu;
    private GameObject FirstInventoryUI;
    private GameObject UI;

    private int ChosenItem = -1;
    private int LastItem = -1;

    private bool activated = false;

    private Inventory MainInventory;
    private Inventory ToolBet;

    private InventoryType inventoryType;

    public EventSystem eSys;

    private void Start()
    {
        UI = GameObject.Find("Main Camera");

        EscMenu = UI.transform.Find("MainScreen/EscMenu").gameObject;
        
        MainInventory = GetComponents<Inventory>()[0];
        ToolBet = GetComponents<Inventory>()[1];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) &&
            MainInventory.IsDragging == false &&
            ToolBet.IsDragging == false)
        {
            ActivateEscMenu();
        }
    }

    public bool DragInOtherInventory(InventoryType InvType)
    {
        if ((!MainInventory.IsDragging || 
            !ToolBet.IsDragging) && 
            (MainInventory.IsDragging == ToolBet.IsDragging))
        {
            FirstInventoryUI = eSys.currentSelectedGameObject.transform.parent.gameObject;
        }

        GameObject SecondInventoryUI = eSys.currentSelectedGameObject.transform.parent.gameObject;//берём родителя от ребёнка

        ChosenItem = int.Parse(eSys.currentSelectedGameObject.name);

        if (MainInventory.IsDragging && SecondInventoryUI.name != FirstInventoryUI.name ||
            ToolBet.IsDragging && SecondInventoryUI.name != FirstInventoryUI.name)
        {
            return true;
        }
        else
        {
            inventoryType = InvType;
            LastItem = int.Parse(eSys.currentSelectedGameObject.gameObject.name);
            return false;
        }
    }
    public void SwapItemsOtherInventorys()
    {
        Slot slot;
        switch (inventoryType)
        {
            case InventoryType.MainInventory:
                slot = MainInventory.inventory[LastItem];
                MainInventory.inventory[LastItem] = ToolBet.inventory[ChosenItem];
                ToolBet.inventory[ChosenItem] = slot;

                MainInventory.StopDrag();
                MainInventory.DestroyInfoOfItem();
                MainInventory.UpdateInventory();

                ToolBet.UpdateInventory();
                break;
            case InventoryType.ToolBet:
                slot = ToolBet.inventory[LastItem];
                ToolBet.inventory[LastItem] = MainInventory.inventory[ChosenItem];
                MainInventory.inventory[ChosenItem] = slot;

                ToolBet.StopDrag();
                ToolBet.DestroyInfoOfItem();
                ToolBet.UpdateInventory();

                MainInventory.UpdateInventory();
                break;
        }
    }

    // ToDo: Перенести в другой скрипт
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