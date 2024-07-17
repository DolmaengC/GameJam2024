using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagisionDetail : MonoBehaviour
{
    public MTunitMovement Magision;
    private float MGspeed = 2f;
    private float MGattackRange = 3f;
    private float MGcooltime = 1.1f;
    private float MGdamage = 11f;
    private int MGHp = 10;
    private int MGcount = 0;
    private float MGbulletSpeed = 0.2f;
    private void Awake()
    {
        Magision = GetComponent<MTunitMovement>();
        Magision.setAttackRange(MGattackRange);
        Magision.setCooltime(MGcooltime);
        Magision.setDamage(MGdamage);
        Magision.setUnitHP(MGHp);
        Magision.setUnitSpeed(MGspeed);
        Magision.setCount(MGcount);
        Magision.setBulletspeed(MGbulletSpeed);
    }
}
