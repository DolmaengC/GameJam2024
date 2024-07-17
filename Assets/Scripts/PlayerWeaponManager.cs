using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public Scanner scanner;
    public GameObject[] bullets;
    public List<WeaponManager> weaponManagerList;
    public float[][] weaponInitData = {
        // attackType, damage, count, speed, coolTime
        new float[] {0, 11, 5, -150, 2},
        new float[] {1, 11, 5, 0.03f, 3},
        new float[] {1, 11, 5, 0.03f, 5},
        new float[] {1, 11, 5, 0.03f, 2},
        new float[] {1, 11, 5, 0.03f, 7}
    };

    void Start() {
        scanner = GetComponent<Scanner>();
        
        // Check if bullets array and weaponInitData have the same length
        int weaponCount = Mathf.Min(bullets.Length, weaponInitData.Length);

        for (int i = 0; i < weaponCount; i++) {
            weaponManagerList[i].setData(bullets[i], scanner,
            weaponInitData[i][0], weaponInitData[i][1], 
            weaponInitData[i][2], weaponInitData[i][3], weaponInitData[i][4]);
        }
    }

    public void UpgradeSkill(int index) {
        weaponManagerList[index].LevelUp(3, 3);
    }    
}

