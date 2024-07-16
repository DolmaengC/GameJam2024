using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public Rigidbody2D rb;
    public int playerLevel;
    public float playerMaxHP;
    public float playerCurrentHP;
    public float playermaxMP;
    public float playerCurrentMP;
    public WeaponManager weaponManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initializePlayerStatus();

    }
    private void OnCollisionEnter(Collision other) {
        if(!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        playerCurrentHP -= other.gameObject.GetComponent<BulletManager>().damage;
        if(playerCurrentHP > 0){

        }else{
            // Dead();
        }
    }

    private void initializePlayerStatus() {

    }


}
