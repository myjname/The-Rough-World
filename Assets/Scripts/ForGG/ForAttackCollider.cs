using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForAttackCollider : MonoBehaviour
{
    private PlayerParameters playerParameters;

    private void Start()
    {
        playerParameters = GetComponentInParent<PlayerParameters>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Enemy")
        {
            collider.GetComponentInParent<EnemyParameters>().localHitPoints -= playerParameters.localDamage;

            Debug.Log($"Player attack, damage = {playerParameters.localDamage}! Enemy HP = {collider.GetComponentInParent<EnemyParameters>().localHitPoints}!");
        }
    }
}
