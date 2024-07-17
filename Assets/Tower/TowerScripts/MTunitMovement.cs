using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MTunitMovement : MonoBehaviour
{
    public Scanner scanner;
    private float unitSpeed ;
    private float attackRange;
    public float cooltime;
    private int unitHP;
    private float damage;
    private int count;
    private float bulletspeed;
    public List<GameObject> bulletPool;
    public GameObject bullet;
    private Animator animator;
    public RaycastHit2D[] hits;
    private bool IsAttacking = false;
    private bool IsDamagging = false;
    private SpriteRenderer spriteRenderer;
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
        Debug.Log(IsAttacking);
        hits = Physics2D.CircleCastAll(transform.position, attackRange, Vector2.zero, 0, scanner.targetLayer);
        if (hits.Length > 0 && !IsAttacking && alive)
        {
            IsAttacking = true;
            turn(scanner.nearestTarget.transform.position);
            animator.SetTrigger("Attacking");
            Invoke("magic", 0.8f);
            StartCoroutine(Attacking(cooltime));
        }
    }
    IEnumerator Attacking(float cooltime) {
        yield return new WaitForSeconds(cooltime);
        IsAttacking = false;
    }

    void isMoving(Vector3 moving) {
            if (moving.x * moving.x > moving.y * moving.y)
            {
                if(moving.x > 0) {
                    animator.SetBool("Rview", true);
                    animator.SetBool("Lview", false);
                    animator.SetBool("Uview", false);
                    animator.SetBool("Dview", false);
                    animator.SetBool("IsWalking", true);
                }
                else {
                    animator.SetBool("Lview", true);
                    animator.SetBool("Rview", false);
                    animator.SetBool("Uview", false);
                    animator.SetBool("Dview", false);
                    animator.SetBool("IsWalking", true);
                }
            }
            else {
                if(moving.y > 0) {
                    animator.SetBool("Uview", true);
                    animator.SetBool("Dview", false);
                    animator.SetBool("Rview", false);
                    animator.SetBool("Lview", false);
                    animator.SetBool("IsWalking", true);
                }
                else {
                    animator.SetBool("Dview", true);
                    animator.SetBool("Uview", false);
                    animator.SetBool("Rview", false);
                    animator.SetBool("Lview", false);
                    animator.SetBool("IsWalking", true);
                }
            }
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
    //bullet

    public GameObject generateBullet()
    {
        GameObject select = null;

        foreach (GameObject item in bulletPool)
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
            select = Instantiate(bullet, transform);
            bulletPool.Add(select);
        }

        return select;
    }

    public void Fire(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position).normalized;

        Transform bullet = generateBullet().transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<BulletManager>().Init(damage, count, dir*bulletspeed);
    }

    private void magic() {
        Vector3 targetPos = scanner.nearestTarget.transform.position;
        Fire(targetPos);
    }

    private void turn(Vector3 targetPos) {
        Vector3 dir = (targetPos - transform.position).normalized;
        Quaternion target = Quaternion.FromToRotation(Vector3.up, dir);
        if(target.eulerAngles.x * target.eulerAngles.x > target.eulerAngles.y * target.eulerAngles.y) {
            if(target.eulerAngles.x<0) {
                animator.SetBool("Rview", true);
                animator.SetBool("Lview", false);
                animator.SetBool("Uview", false);
                animator.SetBool("Dview", false);
            }
            else {
                animator.SetBool("Lview", true);
                animator.SetBool("Rview", false);
                animator.SetBool("Uview", false);
                animator.SetBool("Dview", false);
            }
        }
        else {
            if(target.eulerAngles.y<0) {
                animator.SetBool("Uview", true);
                animator.SetBool("Dview", false);
                animator.SetBool("Rview", false);
                animator.SetBool("Lview", false);
            }
            else {
                animator.SetBool("Dview", true);
                animator.SetBool("Uview", false);
                animator.SetBool("Rview", false);
                animator.SetBool("Lview", false);
            }
        }
    }
    public void setUnitSpeed(float speed)
    {
        unitSpeed = speed;
    }
    public void setAttackRange(float range)
    {
        attackRange = range;
    }
    public void setCooltime(float cool)
    {
        cooltime = cool;
    }
    public void setUnitHP(int hp)
    {
        unitHP = hp;
    }
    public void setDamage(float damage)
    {
        this.damage = damage;
    }
    public void setCount(int count)
    {
        this.count = count;
    }
    public void setBulletspeed(float speed)
    {
        bulletspeed = speed;
    }
}
