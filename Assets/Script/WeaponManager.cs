using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Scanner scanner;
    public int attackType;
    public GameObject bullet;
    public List<GameObject> bulletPool;
    public float damage;
    public int count;
    public float speed;
    public float coolTime;
    public float timer;

    void Awake()
    {
        scanner = GetComponent<Scanner>();
        bulletPool = new List<GameObject>();
    }

    void Update()
    {
        switch (attackType)
        {
            case 0:
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                break;
            case 1:
                timer += Time.deltaTime;

                if (timer > coolTime)
                {
                    timer = 0f;
                    if (scanner.nearestTarget == null)
                    {
                        return;
                    }

                    Vector3 targetPos = scanner.nearestTarget.position;
                    
                    Fire(targetPos);
                    
                }
                break;

            default:
                // 다른 공격 타입 처리
                break;
        }

        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(20, 5);
        }
    }

    void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (attackType == 0)
        {
            Batch();
        }
    }

    void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bulletTransform;
            if (i < transform.childCount)
            {
                bulletTransform = transform.GetChild(i);
            }
            else
            {
                bulletTransform = generateBullet().transform;
            }

            bulletTransform.parent = transform;
            bulletTransform.localPosition = Vector3.zero;
            bulletTransform.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bulletTransform.Rotate(rotVec);
            bulletTransform.Translate(bulletTransform.up * 1.5f, Space.World);
            bulletTransform.GetComponent<BulletManager>().Init(damage, -1, Vector3.zero); // -1 is Infinity Per.
        }
    }

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
        bullet.GetComponent<BulletManager>().Init(damage, count, dir);
    }
}
