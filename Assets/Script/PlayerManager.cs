using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Rigidbody2D rb;
    public PlayerWeaponManager playerWeaponManager;
    public int level;
    public float maxHP;
    public float currentHP;
    public float maxEXP;
    public float currentEXP;

    public TMP_Text HPText;
    public TMP_Text EXPText;
    public TMP_Text levelText;
    public Slider HPBar;
    public Slider EXPBar;

    private bool isTakingDamage = false;
    public Button[] enhancePlayerButtons; 
    public int skillPoint;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initializeStatus();
        playerWeaponManager = GetComponent<PlayerWeaponManager>();
    }
    void Start()
    {
        
        for (int i = 0; i < enhancePlayerButtons.Length; i++)
        {
            int index = i; // 로컬 복사본 생성
            enhancePlayerButtons[i].onClick.AddListener(() => OnEnhancePlayerButtonClicked(index));
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        if (!isTakingDamage)
        {
            StartCoroutine(TakeDamage(1f, other.gameObject.GetComponent<EnemyManager>().damage));
        }
    }

    private IEnumerator TakeDamage(float delay, float damage)
    {
        isTakingDamage = true;
        while (isTakingDamage)
        {
            currentHP -= damage;
            UpdateHP();
            if (currentHP <= 0)
            {
                Dead();
                break;
            }
            yield return new WaitForSeconds(delay);
        }
        isTakingDamage = false;
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
                    skillPoint++;
                    UpdateLevel();
                }
                UpdateEXP();
                Destroy(other.gameObject);
            }
        }
    }

    private void initializeStatus()
    {
        maxHP = 10;
        currentHP = maxHP;
        maxEXP = 10;
        currentEXP = 0;
        UpdateHP();
        UpdateEXP();
        UpdateLevel();
    }

    public void UpdateHP()
    {
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        HPBar.maxValue = maxHP;
        HPBar.value = currentHP;
    }

    public void UpdateEXP()
    {
        EXPText.text = currentEXP.ToString() + "/" + maxEXP.ToString();
        EXPBar.maxValue = maxEXP;
        EXPBar.value = currentEXP;
    }

    public void UpdateLevel()
    {
        levelText.text = "Level " + level.ToString();
    }

    public void Dead()
    {
        GameManager.instance.EndGame(false);
    }
    void OnEnhancePlayerButtonClicked(int index)
    {
        if (skillPoint > 0) {
            skillPoint --; 
            playerWeaponManager.UpgradeSkill(index);
        }
    }
}
