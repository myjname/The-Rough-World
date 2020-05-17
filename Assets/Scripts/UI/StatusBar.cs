using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    private PlayerParameters playerParameters;
    private GameObject statusBar;
    
    private void Start()
    {
        playerParameters = GetComponent<PlayerParameters>();
        GameObject UI = GameObject.Find("Main Camera").gameObject;
        statusBar = UI.transform.Find("MainScreen/StatusBar").gameObject;
        UpdateStatusBar();
    }

    public void UpdateStatusBar()
    {
        statusBar.transform.GetChild(0).GetComponent<Text>().text = $"HP: {playerParameters.localHitPoints}/{playerParameters.HitPoints}";
    }
}
