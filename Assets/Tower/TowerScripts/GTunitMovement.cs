using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class GTunitMovement : MonoBehaviour
{
    public Scanner scanner;
    private float unitSpeed = 2f;
    private Animator animator;
    private float attackRange = 1.5f;
    public RaycastHit2D[] hits;
    public float cooltime = 1.1f;
    private bool IsAttacking = false;
     void Awake()
    {
        scanner = GetComponent<Scanner>();
        animator = GetComponent<Animator>();
        animator.SetBool("Uview", false);
        animator.SetBool("Dview", true);
        animator.SetBool("Rview", false);
        animator.SetBool("Lview", false);
        animator.SetBool("IsWalking", false);
    }
    void Update()
    {
        if (scanner.nearestTarget != null)
        {
            Vector3 dic = scanner.nearestTarget.transform.position - transform.position;
            isMoving(dic);
            transform.Translate(dic.normalized * Time.deltaTime * unitSpeed);
        }
        else {
            animator.SetBool("IsWalking", false);
        }
    }
    void FixedUpdate() {
        hits = Physics2D.CircleCastAll(transform.position, attackRange, Vector2.zero, 0, scanner.targetLayer);
        if (hits.Length > 0 && !IsAttacking)
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
}
