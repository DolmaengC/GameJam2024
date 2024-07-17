using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public Scanner scanner;
    public WeaponManager weaponManager;
    public float coolTime;
    public float timer;
    public int buildCost;
    public int enhanceCost;
    public static Dictionary<string, int> towerStates = new Dictionary<string, int>(); // 타워 종류별 상태를 저장할 static 변수
    public int towerHp = 30;
    private bool IsDamagging = false;
    SpriteRenderer spriteRenderer;

    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        scanner = GetComponent<Scanner>();
        spriteRenderer = transform.Find("AcherTower Variant").GetComponent<SpriteRenderer>();

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

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > coolTime)
        {
            timer = 0f;
            if (scanner.nearestTarget == null)
            {
                return;
            }

            Vector3 targetPos = scanner.nearestTarget.position;
            if (weaponManager != null)
            {
                weaponManager.Fire(targetPos);
            }
        }
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
                Animator animator = towerManager.GetComponentInChildren<Animator>();
                if (animator != null)
                {
                    animator.SetInteger("state", towerStates[towerName]);
                }
            }
        }
    }
    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy")&&!IsDamagging) {
            StartCoroutine(Damagging(1f));
        }   
    }
    IEnumerator Damagging(float cooltime) {
        float animationtime = 0.05f;
        Debug.Log("Damaged");
        towerHp -= 5;
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
        if(towerHp<=0) {
            distroytower();
        }
        yield return new WaitForSeconds(cooltime);
        IsDamagging = false;
    }
    void distroytower() {
        gameObject.SetActive(false);
    }
}
