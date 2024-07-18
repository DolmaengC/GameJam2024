using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightDetail : MonoBehaviour
{
    public GTunitMovement unit;
    private float speed = 1.5f;
    private float attackRange = 1.1f;
    private float cooltime = 0.6f;
    private int Hp = 30;
    private void Awake()
    {
        Debug.Log("Awake");
        unit = GetComponent<GTunitMovement>();
        unit.setUnitSpeed(speed);
        unit.setAttackRange(attackRange);
        unit.setCooltime(cooltime);
        unit.setUnitHP(Hp);
    }
}
