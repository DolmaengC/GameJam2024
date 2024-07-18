using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightDetail : MonoBehaviour
{
    public GTunitMovement Guardian;
    private float KTspeed = 1.5f;
    private float KTattackRange = 1.1f;
    private float KTcooltime = 0.6f;
    private int KTHp = 30;
    private void Awake()
    {
        Debug.Log("Awake");
        Guardian = GetComponent<GTunitMovement>();
        Guardian.setUnitSpeed(KTspeed);
        Guardian.setAttackRange(KTattackRange);
        Guardian.setCooltime(KTcooltime);
        Guardian.setUnitHP(KTHp);
    }
}
