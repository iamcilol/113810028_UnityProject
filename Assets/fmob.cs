using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fmob : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Base;
    Rigidbody2D fmobrb;
    public float movespeed;
    public float playerx;
    public float basex;
    public int CanMove;
    public int CanAttack;


    static public int[] alreadyknock;
    static public int fmobfaceon;
    public int startattack;
    static public int fmobattack;
    public float attackboost;
    public float attackback;
    public int nowisstrike;

    public string FmobName; 
    
    public int MobNumber; 

    public int FmobAlive;
 
    public GameObject DamageWord;
    public Canvas canvas;
    public SpriteRenderer sr;
    void Start()
    {
        FmobAlive=1;
        GetMobNumber();
        Player=GameObject.Find("player");
        Base=GameObject.Find("base");

        movespeed=1.0f;
        CanMove=1;
        fmobrb=GameObject.Find(FmobName).GetComponent<Rigidbody2D>();
        
        fmobfaceon=-1;
        startattack=0;
        fmobattack=0;
        attackboost=200.0f;
        nowisstrike=0;
        DamageWord=Resources.Load<GameObject>("DamageWord");
        sr=GetComponent<SpriteRenderer>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }


    void Update()
    {
        
        if (nowisstrike==0)
        {
            move();
        }
        
        
        playerx=Player.transform.position.x;
        basex=Base.transform.position.x;
        if(GManager.GameStart==false)
        {
            Destroy(gameObject);
        }
    }
    void GetMobNumber()
    {
        FmobName= gameObject.name;
        MobNumber=int.Parse(FmobName);
        alreadyknock=new int[] {0,0,0,0,0};
        
    }


    void move()
    {
        if(startattack==0)
        {
            if (playerx-1.5f<transform.position.x && transform.position.x<playerx+1.5f && player.playeralive==1)
            {
                wait();
                CanMove=0;
                GetComponent<Animator>().SetBool("walk",false);
                if (playerx<transform.position.x)
                {
                    fmobfaceon=-1;
                    GetComponent<SpriteRenderer>().flipX=false;
                }
                else if (playerx>transform.position.x)
                {
                    fmobfaceon=1;
                    GetComponent<SpriteRenderer>().flipX=true;
                }
            }
            else if (basex-1.3f<transform.position.x && transform.position.x<basex+1.3f)
            {
                wait();
                CanMove=0;
                GetComponent<Animator>().SetBool("walk",false);
                if (playerx<transform.position.x)
                {
                    fmobfaceon=-1;
                    GetComponent<SpriteRenderer>().flipX=false;
                }
                else if (playerx>transform.position.x)
                {
                    fmobfaceon=1;
                    GetComponent<SpriteRenderer>().flipX=true;
                }
            }
            else
            {
                CanMove=1;
            }
        }
        


        if (FmobAlive==1)
        {
            if (basex<transform.position.x && CanMove==1)
            {
                GetComponent<SpriteRenderer>().flipX=false;
                transform.Translate(-movespeed*Time.deltaTime,0,0);
                GetComponent<Animator>().SetBool("walk",true);
            }
            else if(basex>transform.position.x && CanMove==1)
            {
                GetComponent<SpriteRenderer>().flipX=true;
                transform.Translate(movespeed*Time.deltaTime,0,0);
                GetComponent<Animator>().SetBool("walk",true);
            }
        }

    }
    void wait()
    {
        if(startattack==0)
        {
            Invoke("attackwait", 1.0f);
            startattack=1;
        }
        
    }


    void attackwait()
    {
        if (playerx-1.5f<transform.position.x && transform.position.x<playerx+1.5f)
        {
            GetComponent<Animator>().SetBool("attackwait",true);
            Invoke("attack", 0.05f);
            attackboost=300.0f;
            attackback=20.0f;
            fmobattack=1;
        }
        else if (basex-1.3f<transform.position.x && transform.position.x<basex+1.3f)
        {
            GetComponent<Animator>().SetBool("attackwait",true);
            Invoke("attack", 0.05f);
            attackboost=0f;
            attackback=0f;
            mobmanager.basehp-=1;
        }
        else
        {
            startattack=0;
        }
    }
    void attack()
    {   
        if (nowisstrike==0)
        {
            if(fmobfaceon==1)
            {
                transform.Translate(-attackback*Time.deltaTime,0,0);
                fmobrb.AddForce(Vector2.right*attackboost);
            }
            else if (fmobfaceon==-1)
            {
                transform.Translate(attackback*Time.deltaTime,0,0);
                fmobrb.AddForce(Vector2.left*attackboost); 
            }
        }
        
        GetComponent<Animator>().SetBool("attack",true);
        GetComponent<Animator>().SetBool("attackwait",false);
        Invoke("endattack", 0.2f);

    }
    void endattack()
    {
        fmobrb.velocity= Vector3.zero;
        GetComponent<Animator>().SetBool("attack",false);
        startattack=0;
        fmobattack=0;
    }


    void OnTriggerEnter2D(Collider2D other) 
    {
        if((other.gameObject.tag=="attackeffect"|| other.gameObject.tag=="Weapon") && player.nowattack==1 )
        {
            sr.color = new Color(1f, 0f, 0f);
            Invoke("ColorBack",0.2f);
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            GameObject DMGWord =Instantiate(DamageWord,canvasRect);
            DMGWord.transform.position = new Vector3( 1920*transform.position.x/18+960, 430, 0f); 
            mobmanager.MobHp[MobNumber]-=player.Damage;
            if(other.gameObject.tag=="attackeffect")
            {
                if(player.playerfaceon==1 && mobmanager.MobHp[MobNumber]<=5 && alreadyknock[MobNumber]==0)
                {
                nowisstrike=1;
                fmobrb.velocity= Vector3.zero;
                alreadyknock[MobNumber]=1;
                fmobrb.AddForce(Vector2.right*200);
                fmobrb.AddForce(Vector2.up*200);
                Invoke("endstrike", 1.8f);
                }
                else if(player.playerfaceon==-1 && mobmanager.MobHp[MobNumber]<=5 && alreadyknock[MobNumber]==0 )
                {
                nowisstrike=1;
                fmobrb.velocity= Vector3.zero;
                alreadyknock[MobNumber]=1;
                fmobrb.AddForce(Vector2.left*200);
                fmobrb.AddForce(Vector2.up*200);
                Invoke("endstrike", 1.8f);
                }
            }

            if(mobmanager.MobHp[MobNumber]<=0)
            {
                FmobAlive=0;
                GetComponent<Animator>().SetBool("walk",false);
                GetComponent<Animator>().SetBool("die",true);
                GetComponent<Animator>().SetBool("attackwait",false);
                GetComponent<Animator>().SetBool("attack",false);
                
                Invoke("die", 1.0f);
            }
            
            if(other.gameObject.tag=="attackeffect" )
            {
                if (fmobattack==1 )
                {
                    fmobrb.velocity= Vector3.zero;
                }
            }
            
        }
    }
    void endstrike()
    {
        nowisstrike=0;
    }
    void die() 
    {
        mobmanager.WaveEndCount+=1;
        player.Exp+=1;
        Destroy(gameObject);
        GetComponent<Animator>().SetBool("die",false);
    }
    void ColorBack()
    {
        sr.color = new Color(1f, 1f, 1f);
    }

        


    
}
