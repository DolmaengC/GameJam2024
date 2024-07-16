using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEngine;

public class Weaponmanager : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    TowerManager tower;

    void Awake(){
        tower = GetComponentInParent<TowerManager>();
    }

    void Start(){
        if(gameObject.name == "Weapon0"){
            id = 0;
            prefabId = 1;
        }else if(gameObject.name == "Weapon1"){
            id = 1;
            prefabId = 2;
        }

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        switch(id){
            case 0:
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if(timer > speed){
                    timer = 0f;
                    Fire();
                }
                break;
        }

        if(Input.GetButtonDown("Jump")){
            LevelUp(20, 5);
        }
    }

    void LevelUp(float damage, int count){
        this.damage = damage;
        this.count += count;

        if(id == 0){
            Batch();
        }
    }

    public void Init(){
        switch(id){
            case 0:
                speed = -150;
                damage = 11;
                Batch();
                break;
            case 1  :
                speed = 0.3f;
                damage = 3;
                break;
        }
    }

    void Batch(){
        for(int i = 0; i < count; i++){
            Transform bullet ;
            if(i < transform.childCount){
                bullet = transform.GetChild(i);
            }else{
                bullet = GameAdministorator.instance.pool.Get(prefabId).transform;
            }
            
            bullet.parent = transform;

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<BulletManager>().Init(damage, -1, Vector3.zero); // -1 is Infinity Per.
        }
    }

    void Fire(){
        if(!tower.scanner.nearestTarget){
            return;
        }

        Vector3 targetPos = tower.scanner.nearestTarget.position;
        Vector3 dir = (targetPos - transform.position).normalized;
        dir = dir.normalized;
        
        Transform bullet = GameAdministorator.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<BulletManager>().Init(damage, count, dir);
    }
}
