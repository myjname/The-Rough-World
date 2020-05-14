using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyBlock : MonoBehaviour
{
    public GameObject EnemyBlock;

    private void Start()
    {
        StartCoroutine(SpawnEnum());
    }

    private IEnumerator SpawnEnum()
    {
        yield return new WaitForSeconds(3);
        Instantiate(EnemyBlock, transform);
        StopCoroutine(SpawnEnum());
    }
}
