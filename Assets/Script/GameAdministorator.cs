using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAdministorator : MonoBehaviour
{
    public static GameAdministorator instance;
    public float gametime;
    public float maxGameTime = 5 * 5f;
    public PoolManager pool;
    public PlayerMovement player;

    void Awake(){
        instance = this;
    }

    void Update()
    {
        gametime += Time.deltaTime;

        if(gametime > maxGameTime){
            gametime = maxGameTime;
        }
    }
}
