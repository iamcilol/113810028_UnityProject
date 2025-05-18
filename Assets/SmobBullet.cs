using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmobBullet : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Base;
    public float BulletSpeed;
    void Start()
    {
        BulletSpeed=3.0f;
        Player=GameObject.Find("player");
        Base=GameObject.Find("base");
        if (smob.AttackTarget==1)
        {
            Vector3 direction = Player.transform.position - transform.position;
            transform.up = direction;
        }
        else if(smob.AttackTarget==2)
        {
            Vector3 direction = Base.transform.position - transform.position;
            transform.up = direction;
        }
    }

    void Update()
    {
        transform.position += transform.up * Time.deltaTime * BulletSpeed;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag=="player")
        {
            player.playerhp-=1;
            Destroy(gameObject);
        }
        if (other.gameObject.tag=="base")
        {
            mobmanager.basehp-=1;
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
 
    }
}
