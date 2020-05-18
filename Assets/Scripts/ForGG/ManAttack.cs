using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManAttack : MonoBehaviour
{
    public float Delay = 1;

    public GameObject AttackCollider;
    private GameObject localAttackCollider;
    private GameObject ObjInArm;

    private void Update()
    {
        if (ObjInArm != GameObject.Find("Main Camera").GetComponents<Inventory>()[1].ObjInArm)
        {
            ObjInArm = GameObject.Find("Main Camera").GetComponents<Inventory>()[1].ObjInArm;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && Delay <= 0)
        {
            Delay = 1;
            localAttackCollider = Instantiate(AttackCollider, transform);
            localAttackCollider.transform.position = transform.position;
            localAttackCollider.transform.rotation = transform.rotation;
        }

        if (Delay > 0)
        {
            Delay -= Time.deltaTime;
        }

        if (Delay <= 0.9f)
        {
            Destroy(localAttackCollider);
        }
    }
}
