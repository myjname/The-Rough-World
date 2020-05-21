using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerParameters : MonoBehaviour
{
    public int HitPoints = 0;
    public int ActionPoints = 0;
    [SerializeField]
    private int WaterPoints = 0;
    [SerializeField]
    private int FoodPoints = 0;
    [SerializeField]
    private int Damage = 0;

    [HideInInspector]
    public int localHitPoints = 0;
    [HideInInspector]
    public int localActionPoints = 0;
    [HideInInspector]
    public int localWaterPoints = 0;
    [HideInInspector]
    public int localFoodPoints = 0;
    [HideInInspector]
    public int localDamage = 0;

    public string nameOfSave = "MySave01";
    private string wayToFile;

    private void Start()
    {
        wayToFile = Path.Combine(Application.dataPath, "Saves/" + nameOfSave + "/SaveDataPersGG.json");

        //if (!File.Exists(wayToFile))
        //{
        //    InitParameters();
        //}
    }

    private void Update()
    {
        if (localHitPoints > HitPoints) localHitPoints = HitPoints;
        if (localActionPoints > ActionPoints) localActionPoints = ActionPoints;
        if (localWaterPoints > WaterPoints) localWaterPoints = WaterPoints;
        if (localFoodPoints > FoodPoints) localFoodPoints = FoodPoints;
        if (Damage > localDamage) Damage = localDamage;

        if (localHitPoints < 0) localHitPoints = 0;
        if (localActionPoints < 0) localActionPoints = 0;
        if (localWaterPoints < 0) localWaterPoints = 0;
        if (localFoodPoints < 0) localFoodPoints = 0;
        if (Damage < 0) Damage = localDamage;

        GetComponent<StatusBar>().UpdateStatusBar();
    }

    public void InitParameters()
    {
        localHitPoints = HitPoints;
        localActionPoints = ActionPoints;
        localWaterPoints = WaterPoints;
        localFoodPoints = FoodPoints;
        localDamage = Damage;
    }
}