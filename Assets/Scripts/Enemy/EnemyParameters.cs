using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParameters : MonoBehaviour
{
    [SerializeField]
    private int HitPoints = 0;
    [SerializeField]
    private int Damage = 0;

    [HideInInspector]
    public int localHitPoints = 0;
    [HideInInspector]
    public int localDamage = 0;


    private void Start()
    {
        localHitPoints = HitPoints;
        localDamage = Damage;
    }

    private void Update()
    {
        if (localHitPoints <= 0) Destroy(gameObject);
    }
}
