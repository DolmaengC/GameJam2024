using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public SpawnData[] spawnData;
    int level;
    float timer;
    void Awake(){
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameAdministorator.instance.gametime / 5f), spawnData.Length - 1);

        if(timer > spawnData[level].spawnTime){
            timer = 0;
            Spawn();
        }
    }

    void Spawn(){
        GameObject enemy = GameAdministorator.instance.pool.Get(0);
        enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
        enemy.GetComponent<EnemyManager>().Init(spawnData[level]);
    }
}   

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
