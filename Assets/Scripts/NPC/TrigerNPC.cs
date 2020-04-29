using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrigerNPC : MonoBehaviour
{
    private List<Dialog> dialogs;
    public int numOfDialog = 0;

    public FirstNPC Fnpc = new FirstNPC();

    private Transform MainScreen;
    private GameObject dialogUI;
    private GameObject localDialogUI;

    private void OnTriggerEnter(Collider colider)
    {
        GameObject UI = GameObject.Find("Main Camera");
        MainScreen = UI.transform.GetChild(1);

        dialogUI = Resources.Load<GameObject>("UI/Dialog");
        dialogs = Fnpc.LoadDialog();

        if (colider.tag == "Player")
        {
            SpawnDialog();
        }
    }

    private void OnTriggerExit(Collider colider)
    {
        DestroyDialog();
        numOfDialog = 0;
        Fnpc = new FirstNPC();
    }

    private void SpawnDialog()
    {
        if (numOfDialog != 0) DestroyDialog();
        
        if (numOfDialog < dialogs.Count)
        {
            localDialogUI = Instantiate(dialogUI, MainScreen);

            localDialogUI.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { DestroyDialog(); });
            localDialogUI.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { SpawnDialog(); });

            localDialogUI.transform.GetChild(2).GetComponent<Text>().text = dialogs[numOfDialog].Text;
            localDialogUI.transform.GetChild(3).GetComponent<Text>().text = dialogs[numOfDialog].Speaker;

            numOfDialog++;
        }
        else
        {
            DestroyDialog();
            numOfDialog = 0;
            Fnpc = new FirstNPC();
        }
    }

    private void DestroyDialog()
    {
        Destroy(localDialogUI);
    }
}
