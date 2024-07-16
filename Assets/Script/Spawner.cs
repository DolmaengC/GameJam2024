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

        if(spawnData[level].spriteType == 0){
            spawnData[level].spawnTime = 3;
            spawnData[level].health = 10;
            spawnData[level].speed = 1.2f;
        }else if(spawnData[level].spriteType == 1){
            spawnData[level].spawnTime = 2;
            spawnData[level].health = 15;
            spawnData[level].speed = 2.2f;
        }else if(spawnData[level].spriteType == 2){
            spawnData[level].spawnTime = 2;
            spawnData[level].health = 20;
            spawnData[level].speed = 2.4f;
        }else if(spawnData[level].spriteType == 3){
            spawnData[level].spawnTime = 1.5f;
            spawnData[level].health = 35;
            spawnData[level].speed = 2f;
        }else if(spawnData[level].spriteType == 4){
            spawnData[level].spawnTime = 4f;
            spawnData[level].health = 55;
            spawnData[level].speed = 2.7f;
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
