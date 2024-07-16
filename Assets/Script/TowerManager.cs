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

    // Start is called before the first frame update
    void Awake()
    {
        scanner = GetComponent<Scanner>();
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
}
