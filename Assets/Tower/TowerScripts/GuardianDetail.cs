using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GuardianDetail : MonoBehaviour
{
    public GTunitMovement unit;
    private float speed = 2f;
    private float attackRange = 1.1f;
    private float cooltime = 1.1f;
    private int Hp = 10;
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
