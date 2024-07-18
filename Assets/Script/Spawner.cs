using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemyList;
    public List<GameObject>[] enemyPools;

    public List<float> timerList;

    public float respawnAccelatePercent;
    
    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        enemyPools = new List<GameObject>[enemyList.Length];
        timerList = new List<float>();
        for (int i = 0; i < enemyList.Length; i++)
        {
            enemyPools[i] = new List<GameObject>();
            timerList.Add(0);
        }
        respawnAccelatePercent = 0;
    }

    void Update()
    {
        float maxTime = 60f;
        float maxPercent = 30f;
        float percentPerSecond = maxPercent / maxTime;

        respawnAccelatePercent += Time.deltaTime * percentPerSecond;

        for (int i = 0; i < enemyList.Length; i++)
        {
            timerList[i] += Time.deltaTime;
            float spawnTime = enemyList[i].GetComponent<EnemyManager>().spawnTime;
            float adjustedSpawnTime = spawnTime * (1 - (respawnAccelatePercent / 100));

            if (timerList[i] > adjustedSpawnTime)
            {
                Spawn(i);
                timerList[i] = 0;
            }
        }

    }

    void Spawn(int index)
    {
        GameObject enemy = GenerateEnemy(index);
        enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
    }

    public GameObject GenerateEnemy(int index)
    {
        GameObject select = null;

        foreach (GameObject item in enemyPools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (select == null)
        {
            select = Instantiate(enemyList[index], transform);
            enemyPools[index].Add(select);
        }

        return select;
    }
}
