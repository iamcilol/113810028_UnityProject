using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    void Start()
    {
        
    }


    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(player.WeaponAttackType==0)
        {
            if (other.gameObject.tag=="floor" ||other.gameObject.tag=="fmob" || other.gameObject.tag=="Smob" || other.gameObject.tag=="wall")
            {
                Invoke("WeaponBack", 0.01f);
            }
        }

    }
    void WeaponBack()
    {
            player.WeaponShot=0;
            player.nowattack=0;
    }
}
