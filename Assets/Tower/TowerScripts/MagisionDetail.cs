using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagisionDetail : MonoBehaviour
{
    public MTunitMovement unit;
    private float speed = 2f;
    private float attackRange = 3f;
    private float cooltime = 1.1f;
    private float damage = 11f;
    private int Hp = 10;
    private int count = 0;
    private float bulletSpeed = 0.2f;
    private void Awake()
    {
        unit = GetComponent<MTunitMovement>();
        unit.setAttackRange(attackRange);
        unit.setCooltime(cooltime);
        unit.setDamage(damage);
        unit.setUnitHP(Hp);
        unit.setUnitSpeed(speed);
        unit.setCount(count);
        unit.setBulletspeed(bulletSpeed);
    }
}
