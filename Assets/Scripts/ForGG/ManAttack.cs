using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManAttack : MonoBehaviour
{
    public float Delay = 1;
    private float localDelay;

    public GameObject AttackCollider;
    private GameObject localAttackCollider;
    private GameObject ObjInArm;

    private void Start()
    {
        localDelay = Delay;
    }

    private void Update()
    {
        if (ObjInArm != GameObject.Find("Main Camera").GetComponents<Inventory>()[1].ObjInArm)
        {
            ObjInArm = GameObject.Find("Main Camera").GetComponents<Inventory>()[1].ObjInArm;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && localDelay <= 0)
        {
            localDelay = Delay;
            localAttackCollider = Instantiate(AttackCollider, transform);
            localAttackCollider.transform.position = transform.position;
            localAttackCollider.transform.rotation = transform.rotation;
        }

        if (localDelay > 0)
        {
            localDelay -= Time.deltaTime;
        }

        if (localDelay <= Delay * 0.9f)
        {
            Destroy(localAttackCollider);
        }
    }
}
