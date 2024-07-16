using System.Collections;
using System.Collections.Generic;

using System.Data.Common;
using Unity.VisualScripting;


using System.Numerics;

using UnityEngine;
using Vector2 = UnityEngine.Vector2;




public class EnemyManager : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriter;
    void Awake()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        spriter = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
    }

    
        
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isLive) return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;  
    }

    void LateUpdate(){
        if(!isLive) return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable(){
        target = GameAdministorator.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
    }

    public void Init(SpawnData data){
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(!collision.CompareTag("Bullet"))
        {
            return;
        }

        health -= collision.GetComponent<BulletManager>().damage;
        if(health > 0){

        }else{
            Dead();
        }
    }

    void Dead(){
        gameObject.SetActive(false);
        // isLive = false;
        // anim.SetTrigger("dead");
        // Destroy(gameObject, 1.5f);
    }
}
