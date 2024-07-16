using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemyList;
    public List<GameObject>[] enemyPools;
    int level;
    float timer;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        enemyPools = new List<GameObject>[enemyList.Length];
        for (int i = 0; i < enemyList.Length; i++)
        {
            enemyPools[i] = new List<GameObject>();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(timer / 10), enemyList.Length); // level은 enemyList의 길이를 넘지 않도록 제한

        // 올바른 구문으로 수정
        if (timer > enemyList[level].GetComponent<EnemyManager>().spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GenerateEnemy(level);
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
