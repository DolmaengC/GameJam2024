using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    public Scanner scanner;
    public int buildCost;
    public int enhanceCost;
    public static Dictionary<string, int> towerStates = new Dictionary<string, int>(); // 타워 종류별 상태를 저장할 static 변수
    public float towerMaxHp;
    public float towerCurrentHp;
    public Slider HPBar;
    private bool IsDamagging = false;
    SpriteRenderer spriteRenderer;

    private Animator animator;
    public Sprite towerImg;

    // Start is called before the first frame update
    void Awake()
    {
        scanner = GetComponent<Scanner>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // 자식 오브젝트에서 애니메이터를 찾습니다.
        animator = GetComponentInChildren<Animator>();

        // 타워 이름을 키로 상태를 저장합니다.
        string towerName = gameObject.name.Replace("(Clone)", "").Trim();
        if (!towerStates.ContainsKey(towerName))
        {
            towerStates[towerName] = 0;
        }

        // 초기 상태를 애니메이터에 설정합니다.
        if (animator != null)
        {
            animator.SetInteger("state", towerStates[towerName]);
        }
    }
    void Start()
    {
        towerCurrentHp = towerMaxHp;
        UpdateHP();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateHP();
    }

    // 상태를 증가시키고 애니메이터에 상태를 전달하는 메서드
    public static void IncreaseState(string towerName)
    {
        towerName = towerName.Replace("(Clone)", "").Trim();

        if (towerStates.ContainsKey(towerName))
        {
            towerStates[towerName]++;
        }
        else
        {
            towerStates[towerName] = 1;
        }

        // 모든 타워 오브젝트의 애니메이터 상태를 업데이트
        foreach (TowerManager towerManager in FindObjectsOfType<TowerManager>())
        {
            string managerTowerName = towerManager.gameObject.name.Replace("(Clone)", "").Trim();
            if (managerTowerName == towerName)
            {
                Animator[] animators = towerManager.GetComponentsInChildren<Animator>();
                foreach (Animator animator in animators)
                {
                    if (animator != null)
                    {
                        animator.SetInteger("state", towerStates[towerName]);
                    }
                }
            }
        }
    }
    
    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy")&&!IsDamagging) {
            StartCoroutine(Damagging(1f, other.gameObject.GetComponent<EnemyManager>().damage));
        }   
    }
    IEnumerator Damagging(float cooltime, float damage) {
        float animationtime = 0.05f;
        Debug.Log("Damaged");
        towerCurrentHp -= damage;
        UpdateHP();
        IsDamagging = true;
        Color towercolor = spriteRenderer.color;
        spriteRenderer.color = new Color(towercolor.r, towercolor.b * 0.75f, towercolor.g * 0.75f, 1);
        yield return new WaitForSeconds(animationtime);
        spriteRenderer.color = new Color(towercolor.r, towercolor.b * 0.5f, towercolor.g * 0.5f, 1);
        yield return new WaitForSeconds(animationtime);
        spriteRenderer.color = new Color(towercolor.r, towercolor.b * 0.25f, towercolor.g * 0.25f, 1);
        yield return new WaitForSeconds(animationtime);
        spriteRenderer.color = new Color(towercolor.r, 0, 0, 1);
        yield return new WaitForSeconds(animationtime);
        spriteRenderer.color = new Color(towercolor.r, towercolor.b * 0.25f, towercolor.g * 0.25f, 1);
        yield return new WaitForSeconds(animationtime);
        spriteRenderer.color = new Color(towercolor.r, towercolor.b * 0.5f, towercolor.g * 0.5f, 1);
        yield return new WaitForSeconds(animationtime);
        spriteRenderer.color = new Color(towercolor.r, towercolor.b * 0.75f, towercolor.g * 0.75f, 1);
        yield return new WaitForSeconds(animationtime);
        spriteRenderer.color = towercolor;
        if(towerCurrentHp<=0) {
            distroytower();
        }
        yield return new WaitForSeconds(cooltime);
        IsDamagging = false;
    }
    void distroytower() {
        gameObject.SetActive(false);
    }

    public void UpdateHP() {
        HPBar.value = towerCurrentHp;
        HPBar.maxValue = towerMaxHp;
    }
}
