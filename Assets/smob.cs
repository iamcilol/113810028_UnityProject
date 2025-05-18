using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class smob : MonoBehaviour
{
    public double SmobPlayerDistance;
    public double SmobBaseDistance;
    public int FlySpeed;
    public float UDFly;
    public int SmobFaceOn;
    public int StartAttack;
    static public int AttackTarget;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Base;
    public GameObject SmobBullet;

    public string SmobName; 
    
    public int MobNumber; 

    public int SmobAlive;
    public GameObject DamageWord;
    public Canvas canvas;
    public SpriteRenderer sr;

    void Start()
    {
        AttackTarget=0;
        SmobBaseDistance=0;
        SmobPlayerDistance=0;
        FlySpeed=2;
        SmobFaceOn=1;
        StartAttack=0;
        Player=GameObject.Find("player");
        Base=GameObject.Find("base");
        SmobBullet=Resources.Load<GameObject>("SBullet");
        GetMobNumber();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        DamageWord=Resources.Load<GameObject>("DamageWord");
        sr=GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        DistanceCount();
        move();
        if(GManager.GameStart==false)
        {
            Destroy(gameObject);
        }

    }

    void GetMobNumber()
    {
        SmobName= gameObject.name;
        MobNumber=int.Parse(SmobName);
        
    }

    void DistanceCount()
    {
        SmobBaseDistance=Math.Sqrt((Math.Pow(Base.transform.position.x-transform.position.x,2)
                                  +Math.Pow(Base.transform.position.y-transform.position.y,2)));
        
        SmobPlayerDistance=Math.Sqrt((Math.Pow(Player.transform.position.x-transform.position.x,2)
                                  +Math.Pow(Player.transform.position.y-transform.position.y,2)));
    }
    void move()
    {
        if((SmobPlayerDistance<5 && player.playeralive==1)||SmobBaseDistance<5 )
        {
            wait();
            if(player.playeralive==1)
            {
                if (Player.transform.position.x<transform.position.x)
                {
                    SmobFaceOn=-1;
                    GetComponent<SpriteRenderer>().flipX=false;
                }
                else if (Player.transform.position.x>transform.position.x)
                {
                    SmobFaceOn=1;
                    GetComponent<SpriteRenderer>().flipX=true;
                }                
            }
            else
            {
                if (Base.transform.position.x<transform.position.x)
                {
                    SmobFaceOn=-1;
                    GetComponent<SpriteRenderer>().flipX=false;
                }
                else if (Base.transform.position.x>transform.position.x)
                {
                    SmobFaceOn=1;
                    GetComponent<SpriteRenderer>().flipX=true;
                }                  
            }
                
        }
        else if(SmobBaseDistance>=5)
        {
            if(Base.transform.position.x<transform.position.x)
            {
                transform.Translate(-FlySpeed*Time.deltaTime,0,0);
                SmobFaceOn=-1;
                GetComponent<SpriteRenderer>().flipX=false;
            }
            else 
            {
                transform.Translate(FlySpeed*Time.deltaTime,0,0);
                SmobFaceOn=1;
                GetComponent<SpriteRenderer>().flipX=true;
            }
        }
        if(transform.position.y>2.6f)
        {
            UDFly=-0.5f;
        }
        else if(transform.position.y<2.5f)
        {
            UDFly=0.5f;
        }
        transform.Translate(0,UDFly*Time.deltaTime,0);

    }



    void wait()
    {
        if(StartAttack==0)
        {
            Invoke("attackwait", 1.0f);
            StartAttack=1;  
        }
        
    }


    void attackwait()
    {
        if (SmobPlayerDistance<=5 && player.playeralive==1)
        {
            Invoke("attack", 0.05f);
            AttackTarget=1;
        }
        else if (SmobBaseDistance<=5)
        {
            Invoke("attack", 0.05f);
            AttackTarget=2;
        }
        else
        {
            StartAttack=0;
        }
    }
    void attack()
    {
        GameObject SBullet =Instantiate(SmobBullet);
        SBullet.transform.position = transform.position;
        Invoke("endattack", 0.2f);
        
    }
    void endattack()
    {
        StartAttack=0;
    }


    void OnTriggerEnter2D(Collider2D other) 
    {
        if((other.gameObject.tag=="attackeffect"|| other.gameObject.tag=="Weapon") && player.nowattack==1 )
        {
            sr.color = new Color(1f, 0f, 0f);
            Invoke("ColorBack",0.2f);
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            GameObject DMGWord =Instantiate(DamageWord,canvasRect);
            DMGWord.transform.position = new Vector3( 1920*transform.position.x/18+960, 850, 0f); 
            mobmanager.MobHp[MobNumber]-=player.Damage;

        }
        if(mobmanager.MobHp[MobNumber]<=0)
        {
            GetComponent<Animator>().SetBool("die",true);    
            Invoke("die", 1.0f);
        }
    }
    void die() 
    {
        mobmanager.WaveEndCount+=1;
        GetComponent<Animator>().SetBool("die",false); 
        player.Exp+=1; 
        Destroy(gameObject);
    }
    void ColorBack()
    {
        sr.color = new Color(1f, 1f, 1f);
    }
}
