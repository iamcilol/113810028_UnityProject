using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageWord : MonoBehaviour
{
    Rigidbody2D rb;
    TextMeshProUGUI tMP;
    void Start()
    {
        Debug.Log(transform.position.y);
        Debug.Log(transform.position.x);
        tMP = GetComponent<TextMeshProUGUI>();
        rb=GetComponent<Rigidbody2D>();
        tMP.text=player.Damage.ToString(); 
        rb.gravityScale = 80;
        rb.AddForce(Vector2.up*20000);
        rb.AddForce(Vector2.right*Random.Range(-5000,5000));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {

        }
        if(transform.position.y<-100)
        {
            Destroy(gameObject);
        }
    }
}
