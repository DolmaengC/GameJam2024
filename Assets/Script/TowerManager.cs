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

    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        scanner = GetComponent<Scanner>();

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
}
