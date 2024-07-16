using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public Scanner scanner;
    // Start is called before the first frame update
    void Awake()
    {
        scanner = GetComponent<Scanner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
