using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public float spawnRadios = 9;
    public float spawnPeriod = 10;
    public float spawnPeriodOffset = 5;
    public GameObject itemFolder;
    public List<GameObject> itemPrefabList;

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    void SpawnItem()
    {
        Vector2 spawnPos = transform.position;
        spawnPos += Random.insideUnitCircle * spawnRadios;
        GameObject itemPrefab = itemPrefabList[Random.Range(0, itemPrefabList.Count)];
        GameObject item = Instantiate(itemPrefab, itemFolder.transform);
        item.transform.position = spawnPos;
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            SpawnItem();
            yield return new WaitForSeconds(Random.Range(spawnPeriod - spawnPeriodOffset, spawnPeriod + spawnPeriodOffset));
        }
    }
}
