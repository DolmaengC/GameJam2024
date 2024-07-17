using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianDetail : MonoBehaviour
{
    public GTunitMovement Guardian;
    private float GDspeed = 2f;
    private float GDattackRange = 1.1f;
    private float GDcooltime = 1.1f;
    private int GDHp = 10;
    private void Awake()
    {
        Debug.Log("Awake");
        Guardian = GetComponent<GTunitMovement>();
        Guardian.setUnitSpeed(GDspeed);
        Guardian.setAttackRange(GDattackRange);
        Guardian.setCooltime(GDcooltime);
        Guardian.setUnitHP(GDHp);
    }
}
