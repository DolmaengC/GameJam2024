using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class GTunitMovement : MonoBehaviour
{
    public Scanner scanner;
    private float unitSpeed = 2f;
    private float attackRange = 1.1f;
    public float cooltime = 1.1f;
    private int unitHP = 10;
    private Animator animator;
    public RaycastHit2D[] hits;
    private bool IsAttacking = false;
    private bool IsDamagging = false;
    SpriteRenderer spriteRenderer;
    private bool alive = true;
     void Awake()
    {
        scanner = GetComponent<Scanner>();
        animator = GetComponent<Animator>();
        animator.SetBool("Uview", false);
        animator.SetBool("Dview", true);
        animator.SetBool("Rview", false);
        animator.SetBool("Lview", false);
        animator.SetBool("IsWalking", false);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (scanner.nearestTarget != null)
        {
            Vector3 dic = scanner.nearestTarget.transform.position - transform.position;
            if(IsAttacking==false&&alive){
                isMoving(dic);
                transform.Translate(dic.normalized * Time.deltaTime * unitSpeed);
            }
        }
        else {
            animator.SetBool("IsWalking", false);
        }
    }
    void FixedUpdate() {
        hits = Physics2D.CircleCastAll(transform.position, attackRange, Vector2.zero, 0, scanner.targetLayer);
        if (hits.Length > 0 && !IsAttacking && alive)
        {
            IsAttacking = true;
            animator.SetTrigger("Attacking");
            Invoke("killing", 0.8f);
            StartCoroutine(Attacking(cooltime));
        }
    }
    IEnumerator Attacking(float cooltime) {
            yield return new WaitForSeconds(cooltime);
            IsAttacking = false;
    }

    void isMoving(Vector3 moving) {
            if (moving.x > 0.5f)
            {
                animator.SetBool("Rview", true);
                animator.SetBool("Lview", false);
                animator.SetBool("Uview", false);
                animator.SetBool("Dview", false);
                animator.SetBool("IsWalking", true);
            }
            else if (moving.x < -0.5f)
            {
                animator.SetBool("Lview", true);
                animator.SetBool("Rview", false);
                animator.SetBool("Uview", false);
                animator.SetBool("Dview", false);
                animator.SetBool("IsWalking", true);
            }
            else if (moving.y > 0.5f)
            {
                animator.SetBool("Uview", true);
                animator.SetBool("Dview", false);
                animator.SetBool("Rview", false);
                animator.SetBool("Lview", false);
                animator.SetBool("IsWalking", true);
            }
            else if (moving.y < -0.5f)
            {
                animator.SetBool("Dview", true);
                animator.SetBool("Uview", false);
                animator.SetBool("Rview", false);
                animator.SetBool("Lview", false);
                animator.SetBool("IsWalking", true);
            }
    }
    void killing() {
            scanner.nearestTarget.transform.CompareTag("Enemy");
            scanner.nearestTarget.gameObject.SetActive(false);
    }
    void OnDrawGizmos() // 범위 그리기
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy")&&!IsDamagging&&alive) {
            StartCoroutine(Damagging(1f));
        }    
    }
    IEnumerator Damagging(float cooltime) {
        float animationtime = 0.05f;
        Debug.Log("Damaged");
        unitHP -= 5;
        IsDamagging = true;
        spriteRenderer.color = new Color(1, 0.75f, 0.75f, 1);
        yield return new WaitForSeconds(animationtime);
        spriteRenderer.color = new Color(1, 0.5f, 0.5f, 1);
        yield return new WaitForSeconds(animationtime);
        spriteRenderer.color = new Color(1, 0.25f, 0.25f, 1);
        yield return new WaitForSeconds(animationtime);
        spriteRenderer.color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(animationtime);
        spriteRenderer.color = new Color(1, 0.25f, 0.25f, 1);
        yield return new WaitForSeconds(animationtime);
        spriteRenderer.color = new Color(1, 0.5f, 0.5f, 1);
        yield return new WaitForSeconds(animationtime);
        spriteRenderer.color = new Color(1, 0.75f, 0.75f, 1);
        yield return new WaitForSeconds(animationtime);
        spriteRenderer.color = new Color(1, 1, 1, 1);
        if(unitHP<=0) {
            death();
        }
        yield return new WaitForSeconds(cooltime);
        IsDamagging = false;
    }
    void death(){
        float animationtime = 0.8f;
        alive = false;
        animator.SetTrigger("Death");
        Invoke("distroyUnit", animationtime);
        
    }

    void distroyUnit(){
        gameObject.SetActive(false);
    }
}