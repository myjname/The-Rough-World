using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float Delay = 2;
    private float localDelay;

    private EnemyParameters enemyParameters;
    private PlayerParameters playerParameters;

    private void Start()
    {
        localDelay = Delay;
        enemyParameters = GetComponentInParent<EnemyParameters>();
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player" && localDelay <= 0)
        {
            collider.GetComponent<PlayerParameters>().localHitPoints -= enemyParameters.localDamage;

            Debug.Log($"Enemy attack! Damage = {enemyParameters.localDamage}! Player HP = {collider.GetComponent<PlayerParameters>().localHitPoints}");

            localDelay = Delay;
        }
    }

    private void Update()
    {
        if (localDelay > 0) localDelay -= Time.deltaTime;
    }
}
