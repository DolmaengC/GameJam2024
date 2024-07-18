using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public int unitCost;
    public Sprite unitImg;
    public void setUnitCost(int cost) {
        unitCost = cost;
    }
}
