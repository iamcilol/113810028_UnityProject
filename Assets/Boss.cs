using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Boss : MonoBehaviour
{
    Rigidbody2D BossRb;
    static public int BossHp;
    static public int BossShield;
    static public int BossDmg;
    static public int BossFaceOn;

    [SerializeField] GameObject Player;
    [SerializeField] GameObject BH;
    [SerializeField] GameObject BS;
    [SerializeField] GameObject Beam;
    public double BossPlayerDistance;
    public float UDFly;
    public int StartAttack;
    public bool SmashBack;
    static public bool NowSmash;
    static public bool BossFall;
    public bool BossEscape;
    public bool NowEscape;
    static public bool BossShoot;
    public bool OnFloor;

    public GameObject DamageWord;
    public Canvas canvas;
    public SpriteRenderer sr;
    void Start()
    {
        BossRb=GetComponent<Rigidbody2D>();
        BossHp=500;
        BossShield=250;
        BossDmg=4;
        BossFaceOn=1;
        Player=GameObject.Find("player");
        Player=GameObject.Find("player");
        BossRb.gravityScale = 0;
        StartAttack=0;
        SmashBack=false;
        NowSmash=false;
        BH=GameObject.Find("BossHp");
        BS=GameObject.Find("BossShield");
        Beam=GameObject.Find("BossBeam");
        BossFall=false;
        BossEscape=false;
        NowEscape=false;
        BossShoot=false;
        OnFloor=false;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        DamageWord=Resources.Load<GameObject>("DamageWord");
        sr=GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        Locate();
        if(StartAttack==0)
        {
            Move();
        }
        if(SmashBack==true)
        {
            SmaBack();
        }   
    }
    void Locate()
    {
        BossPlayerDistance=Math.Sqrt((Math.Pow(Player.transform.position.x-transform.position.x,2)
                                     +Math.Pow(Player.transform.position.y-transform.position.y,2)));
    }
    void Move()
    {
        if(BossShield>0)
        {
            if (SmashBack==false)
            {
                if(Player.transform.position.x-1f<transform.position.x && transform.position.x<Player.transform.position.x+1f)
                {
                    wait();
                }
                else
                {
                    if(Player.transform.position.x<transform.position.x)
                    {
                        transform.Translate(-5*Time.deltaTime,0,0);
                        GetComponent<SpriteRenderer>().flipX=true;
                    }
                    else if(Player.transform.position.x>transform.position.x)
                    {
                        transform.Translate(5*Time.deltaTime,0,0);
                        GetComponent<SpriteRenderer>().flipX=false;
                    } 
                }
                if(transform.position.y>2.6f)
                {
                    UDFly=-0.5f;
                    Debug.Log("up");
                }
                else if(transform.position.y<2.5f)
                {
                    UDFly=0.5f;
                    Debug.Log("down");
                }
                transform.Translate(0,UDFly*Time.deltaTime,0);   
            }

        }
        else
        {
            GetComponent<Animator>().SetBool("fall",true);
            BossFall=true;
            BossRb.gravityScale = 3;
            Debug.Log("fall");
            if(BossPlayerDistance<8f)
            {
                if(Player.transform.position.x<transform.position.x && BossEscape==false)
                {
                    transform.Translate(8*Time.deltaTime,0,0);
                    GetComponent<SpriteRenderer>().flipX=true;
                    BossFaceOn=0;
                }
                else if(Player.transform.position.x>transform.position.x && BossEscape==false)
                {
                    transform.Translate(-8*Time.deltaTime,0,0);
                    GetComponent<SpriteRenderer>().flipX=false;
                    BossFaceOn=1;
                }
            }
            else
            {
                if(OnFloor==true)
                {
                    wait();
                }
            }
            if (transform.position.x>=10 || transform.position.x<=-8)
            {
                BossEscape=true;
                Escape();
            }
        }
    }
    void wait()
    {
        if(StartAttack==0)
        {
            if (BossFall==false)
            {
                Invoke("attackwait", 0.1f);
                StartAttack=1;  
            }
            else
            {
                if(player.playeralive==1)
                {
                    Beam.GetComponent<BoxCollider2D>().enabled = false;
                    if(Player.transform.position.x<transform.position.x)
                    {
                        Beam.transform.position = new Vector3(transform.position.x-12,transform.position.y,transform.position.z);  
                        GetComponent<SpriteRenderer>().flipX=true;
                        Beam.GetComponent<SpriteRenderer>().flipX=true;
                    }
                    else if(Player.transform.position.x>transform.position.x)
                    {
                        Beam.transform.position = new Vector3(transform.position.x+12,transform.position.y,transform.position.z);
                        GetComponent<SpriteRenderer>().flipX=false;
                        Beam.GetComponent<SpriteRenderer>().flipX=false;
                    }
                    Beam.GetComponent<Animator>().SetBool("load",true);
                    GetComponent<Animator>().SetBool("PrepareShoot",true);
                    Invoke("shoot", 3f);
                    StartAttack=1;    
                }

            }
        }
        
    }


    void attackwait()
    {
        if (Player.transform.position.x-1f<transform.position.x && transform.position.x<Player.transform.position.x+1f && player.playeralive==1 )
        {
            Invoke("attack", 0.05f);
            GetComponent<Animator>().SetBool("Smash",true);
        }
        else
        {
            StartAttack=0;
        }
    }
    void attack()
    {
        NowSmash=true;
        BossRb.gravityScale = 3;
    }
    void SmaBack()
    {
        
        NowSmash=false;
        GetComponent<Animator>().SetBool("Smash",false);
        BossRb.gravityScale = 0;
        if(transform.position.y<2.55f && BossFall==false)
        {
            Debug.Log("smaback");
            transform.Translate(0,3*Time.deltaTime,0);
        }
        else
        {
            BossRb.velocity= Vector3.zero;
            StartAttack=0;
            SmashBack=false;
        }
        
    }
    void Escape()
    {
        OnFloor=false;
        Debug.Log("escape");
        if(transform.position.x>=10 && NowEscape==false)
        {
            NowEscape=true;
            BossRb.AddForce(Vector2.left*60000);
            BossRb.AddForce(Vector2.up*80000);
        }
        if(transform.position.x<=-8 && NowEscape==false)
        {
            NowEscape=true;
            BossRb.AddForce(Vector2.right*60000);
            BossRb.AddForce(Vector2.up*80000);
        }
    }
    void shoot()
    {
        BossShoot=true;
        Beam.GetComponent<BoxCollider2D>().enabled = true;
        Beam.GetComponent<Animator>().SetBool("shoot",true);
        GetComponent<Animator>().SetBool("Shoot",true);
        Invoke("Endshoot", 1f);
    }
    void Endshoot()
    {
        BossShoot=false;
        Beam.GetComponent<Animator>().SetBool("shoot",false);
        Beam.GetComponent<Animator>().SetBool("load",false);
        GetComponent<Animator>().SetBool("Shoot",false);
        GetComponent<Animator>().SetBool("PrepareShoot",false);
        Beam.transform.position = new Vector3(-24.3f,-13.8f,0);
        StartAttack=0;
    }
    void die()
    {
        GetComponent<Animator>().SetBool("die",false);
        Beam.transform.position = new Vector3(-24.3f,-13.8f,0);
        Destroy(gameObject);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag=="floor" && BossFall==false)
        {
            SmashBack=true;
        }
        if (other.gameObject.tag=="floor" && BossFall==true)
        {
            BossRb.velocity= Vector3.zero;
            NowEscape=false;
            BossEscape=false;
            OnFloor=true;
        }
        if (other.gameObject.tag=="player" && BossFall==false)
        {
            BossRb.velocity= Vector3.zero;
            SmashBack=true;
        }
    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        if((other.gameObject.tag=="attackeffect"|| other.gameObject.tag=="Weapon") && player.nowattack==1 )
        {
            if(BossShield>=1)
            {
                sr.color = new Color(1f, 0f, 0f);
                Invoke("ColorBack",0.2f);
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                GameObject DMGWord =Instantiate(DamageWord,canvasRect);
                DMGWord.transform.position = new Vector3( 1920*transform.position.x/18+960, 850, 0f); 
                BossShield-=player.Damage;
            }
            else
            {
                sr.color = new Color(1f, 0f, 0f);
                Invoke("ColorBack",0.2f);
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                GameObject DMGWord =Instantiate(DamageWord,canvasRect);
                DMGWord.transform.position = new Vector3( 1920*transform.position.x/18+960, 850, 0f); 
                BossHp-=player.Damage;
            }
            

        }
        if(BossHp<=0)
        {
            GetComponent<Animator>().SetBool("die",true);    
            Invoke("die", 1.0f);
            Beam.transform.position = new Vector3(-24.3f,-13.8f,0);
            mobmanager.WaveEndCount+=5;
            player.UpgradePoint+=10; 

            BH.SetActive(false);
            BS.SetActive(false);
        }
    }
    void ColorBack()
    {
        sr.color = new Color(1f, 1f, 1f);
    }
}
