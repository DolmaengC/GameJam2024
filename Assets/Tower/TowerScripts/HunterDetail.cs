using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterDetail : MonoBehaviour
{
    public GTunitMovement unit;
    private float speed = 2f;
    private float attackRange = 1.1f;
    private float cooltime = 0.7f;
    private int Hp = 20;
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
