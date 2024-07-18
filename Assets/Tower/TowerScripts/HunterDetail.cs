using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterDetail : MonoBehaviour
{
    public GTunitMovement Hunter;
    private float HTspeed = 2f;
    private float HTattackRange = 1.1f;
    private float HTcooltime = 0.7f;
    private int HTHp = 20;
    private void Awake()
    {
        Debug.Log("Awake");
        Hunter = GetComponent<GTunitMovement>();
        Hunter.setUnitSpeed(HTspeed);
        Hunter.setAttackRange(HTattackRange);
        Hunter.setCooltime(HTcooltime);
        Hunter.setUnitHP(HTHp);
    }
}
