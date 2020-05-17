using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForAttackCollider : MonoBehaviour
{
    private PlayerParameters playerParameters;

    private GameObject ObjInArm;

    private void Start()
    {
        playerParameters = GetComponentInParent<PlayerParameters>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        ObjInArm = GameObject.Find("Main Camera").GetComponents<Inventory>()[1].ObjInArm;

        if (collider.tag == "Enemy")
        {
            collider.GetComponentInParent<EnemyParameters>().localHitPoints -= playerParameters.localDamage + ObjInArm.GetComponent<WeaponInfoInArm>().Damage;

            collider.GetComponentInParent<Rigidbody>().AddRelativeForce(Vector3.back * 10, ForceMode.Impulse);

            Debug.Log($"Player attack, damage = {playerParameters.localDamage}! Enemy HP = {collider.GetComponentInParent<EnemyParameters>().localHitPoints}!");
        }
    }
}
