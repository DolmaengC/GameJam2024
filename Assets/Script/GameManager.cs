using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    // public PlayerMovement playerMovement;
    public float gametime;
    public float maxGameTime = 5 * 5f;
    void Awake()
    {
        gametime = 0;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }



    void FixedUpdate()
    {
        gametime += Time.deltaTime;

        if(gametime > maxGameTime){
            gametime = maxGameTime;
        }
    }
}
