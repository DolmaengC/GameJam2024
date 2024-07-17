using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public float damage;
    public float spawnTime;
    public int score;
    public GameObject expItemPrefab; // 경험치 아이템 프리팹 참조 추가
    public RuntimeAnimatorController[] animCon;

    bool isLive;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriter;

    public Scanner scanner;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    void FixedUpdate()
    {
        if (!isLive) return;

        Transform target = scanner.nearestTarget;
        if (target != null)
        {
            Vector2 dirVec = (Vector2)target.position - rigid.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
        }
    }

    void LateUpdate()
    {
        if (!isLive) return;

        Transform target = scanner.nearestTarget;
        if (target != null)
        {
            spriter.flipX = target.position.x < rigid.position.x;
        }
    }

    void OnEnable()
    {
        isLive = true;
        health = maxHealth;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
        {
            return;
        }

        health -= collision.GetComponent<BulletManager>().damage;
        
        if (GameObject.Find("IceSword") == true)
        {
            speed = 0.3f;
            EndIceTime();
        }

        if (health <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        // 경험치 아이템 생성
        if (expItemPrefab != null)
        {
            Instantiate(expItemPrefab, transform.position, Quaternion.identity);
        }
        
        gameObject.SetActive(false);
        GameManager.instance.IncreaseScore(score);

        

        // isLive = false;
        // anim.SetTrigger("dead");
        // Destroy(gameObject, 1.5f);
    }
    IEnumerator EndIceTime() {
        yield return new WaitForSeconds(5.0f);
        speed = 1.0f;
    }
}
