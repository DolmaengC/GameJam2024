using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    public Rigidbody2D rb;
    public WeaponManager weaponManager;
    public int level;
    public float maxHP;
    public float currentHP;
    public float maxEXP;
    public float currentEXP;
    public float coin;
    

    public TMP_Text HPText;
    public TMP_Text EXPText;
    public TMP_Text levelText;
    public Slider HPBar;
    public Slider EXPBar;
    public TMP_Text coinText;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initializeStatus();

    }
    void FixedUpdate() {
        coin += 0.05f;
        UpdateCoin();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        currentHP -= other.gameObject.GetComponent<EnemyManager>().damage;
        UpdateHP();
        if(currentHP > 0){
            
        }else{
            // Dead();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ExpItem"))
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);
            float pickupRange = 1.0f; // 원하는 아이템 획득 범위 설정

            if (distance <= pickupRange)
            {
                string itemName = other.gameObject.name;
                string expValueString = itemName.Replace("ExpItem_", "").Replace("(Clone)", "").Trim();
                int expValue;
                if (int.TryParse(expValueString, out expValue))
                {
                    currentEXP += expValue;
                }
                else
                {
                    Debug.LogWarning("Invalid EXP value in item name: " + other.gameObject.name);
                }

                if (currentEXP >= maxEXP)
                {
                    level++;
                    currentEXP -= maxEXP;
                    maxEXP = level * 10;
                    UpdateLevel();
                }
                UpdateEXP();
                Destroy(other.gameObject);
            }
        }
    }


    private void initializeStatus() {
        coin = 0;
        maxHP = 10;
        currentHP = maxHP;
        maxEXP = 10;
        currentEXP = 0;
        UpdateHP();
        UpdateEXP();
        UpdateCoin();
        UpdateLevel();
    }

    public void UpdateHP() {
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        HPBar.maxValue = maxHP; 
        HPBar.value = currentHP;
    }
    public void UpdateEXP() {
        EXPText.text = currentEXP.ToString() + "/" + maxEXP.ToString();
        EXPBar.maxValue = maxEXP;
        EXPBar.value = currentEXP;
    }
    public void UpdateLevel() {
        levelText.text = "Level " + level.ToString();
    }  
    public void UpdateCoin() {
        coinText.text = Mathf.FloorToInt(coin).ToString();
    }


}
