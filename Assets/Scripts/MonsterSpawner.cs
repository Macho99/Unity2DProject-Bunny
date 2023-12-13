using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public float spawnPeriod;
    public float spawnPeriodOffset;
    public GameObject monsterFolder;
    public List<GameObject> monsterPrefabList;

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    void SpawnMonster()
    {
        GameObject monsterPrefab = monsterPrefabList[Random.Range(0, monsterPrefabList.Count)];
        GameObject monster = Instantiate(monsterPrefab, monsterFolder.transform);
        monster.transform.position = transform.position;
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnPeriod - spawnPeriodOffset, spawnPeriod + spawnPeriodOffset));
            SpawnMonster();
        }
    }
}
