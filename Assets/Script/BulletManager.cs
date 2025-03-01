using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float damage;
    public float speed;
    public int per; // 몇명 관통 가능한지
    public int range;
    public EnemyManager enemyManager;

    Rigidbody2D rigid;

    void Awake(){
        rigid = this.GetComponent<Rigidbody2D>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    public void Init(float damage, int per, Vector3 dir){
        this.damage = damage;
        this.per = per;

        if(per > -1){
            rigid.velocity = dir *15;
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(!collision.CompareTag("Enemy") || per == -1){
            return;
        }

        per--;

        if(per == -1){
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    
}
