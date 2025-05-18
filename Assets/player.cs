using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class player : MonoBehaviour
{
    Rigidbody2D rb;
    static public float spd ;
    public float Jumpforce;
    public float Boostforce;
    public float toBoostCD;
    public float BoostCD;
    static public int onfloor;
    public int Canboost;
    public int skill;
    static public float playerhp;
    

    static public int playeralive;



    public int Weaponfaceon;
    [SerializeField] GameObject Weapon;
    static public int WeaponAttackType;
    public int WeaponPointOn;
    public float WeaponSpeed;
    static public int WeaponShot;
    public float ShootCD;

    [SerializeField] GameObject attackeffect;
    Rigidbody2D wprb;
    static public int playerfaceon;
    
    public int nowskill;
    static public int nowattack;
    public Animator boostskillLoad;
    public SpriteRenderer sr;
    


    [SerializeField] GameObject Fmob;

    

    public Slider hpslider;
    public Animator hpword;

    [SerializeField] GameObject PlayerChange;
    public Animator PlayerChangeAni;
    static public int PlayerType;
    public float AttackTime;
    static public int Damage;
    public Animator CrazySkillLoad;
    public float toCrazyCD;
    public float CrazyCD;
    public int CanCrazy;
    public bool CrazyHealth;
    public bool BaseHealth;

    static public int Exp;
    static public int Lv;
    static public int UpgradePoint;
    public float UpgradeExp;

    
    public Slider ExpSlider;
    public float MP;
    public Slider MPSlider;
    public bool MPHealth;

    [SerializeField] Text basewarn;

    void Start()
    {

    }

    
    void Update()
    {
        NowStart();
        if(GManager.PlayerSystemSetComplete==true)
        {
            MPController();
            ExpController();
            Hpbarcontroller();
            if (playeralive==1)
            {
            Move();
            Jump();
            WeaponManage();
            }

            BoostSkillCD();
            CrazySkillCD();
            diemanager();
        }
        change();
        if (WeaponAttackType==1 && PlayerType==1)
        {
            attackeffect.transform.position = new Vector3( transform.position.x+(0.5f*Weaponfaceon), transform.position.y-0.4f, transform.position.z );
        }
        else
        {
            attackeffect.transform.position = new Vector3( transform.position.x+(0.5f*Weaponfaceon), transform.position.y, transform.position.z );
        }
        
        if (PlayerType==2)
        {
            CrazyEffect();
        }
        if (-8.0f<transform.position.x && transform.position.x<-6.3f && mobmanager.wave%10!=0)
        {
            if (playerhp<10 && BaseHealth==true)
            {
                Invoke("Health", 0.5f);
                BaseHealth=false;   
            }
        }
    }

    void NowStart()
    {
        if (GManager.GameStart==true && GManager.PlayerSystemSetComplete==false)
        {
            playeralive=1;
            playerhp=10f;

            BoostCD=2.0f;
            toBoostCD=0f;

            CrazyCD=1.5f;
            toCrazyCD=0f;
            CanCrazy=0;

            skill=1;
            spd = 5.0f+0.5f*GManager.SpdPlus;
            Jumpforce = 300.0f;
            Boostforce = 300.0f;
            onfloor=0;
            Canboost=0;
            rb=GetComponent<Rigidbody2D>();
            sr=GetComponent<SpriteRenderer>();
            attackeffect.SetActive(false);

            playerfaceon=-1;
            wprb=GameObject.Find("weapon").GetComponent<Rigidbody2D>();
            nowskill=0;
            nowattack=0;

            WeaponAttackType=1;
            WeaponPointOn=-1;
            WeaponSpeed=50.0f;
            WeaponShot=0;
            
            hpslider= GameObject.Find("HP").GetComponent<Slider>();
            hpword=GameObject.Find("UI word").GetComponent<Animator>();
            boostskillLoad=GameObject.Find("SkillLoad").GetComponent<Animator>();

            
            PlayerChange=GameObject.Find("PlayerChange");
            PlayerChangeAni=GameObject.Find("PlayerChange").GetComponent<Animator>();
            PlayerType=1;
            Damage=3+GManager.DmgPlus;
            CrazySkillLoad=GameObject.Find("CrazyLoad").GetComponent<Animator>();
            CrazyHealth=true;

            ExpSlider= GameObject.Find("EXP").GetComponent<Slider>();

            Lv=GManager.StartLv;
            BaseHealth=true;
            Debug.Log("start");
            GManager.PlayerSystemSetComplete=true;
            MPSlider= GameObject.Find("MP").GetComponent<Slider>();

            MP=20;
            MPHealth=true;
        }
    }
    void change()
    {
        PlayerChange.transform.position=transform.position;
        
        if (Input.GetKeyDown(KeyCode.E) && CanCrazy==1 && WeaponShot==0 && GManager.CracySkillIsLock==false && playeralive==1)
        {
            MP-=6;
            PlayerChange.SetActive(true);
            rb.AddForce(Vector2.up*50);
            if (PlayerType==1)
            {
                GetComponent<Animator>().SetBool("IsCrazy",true);
                PlayerChangeAni.SetBool("BtoR",true);
                PlayerType=2;
                if(WeaponAttackType==0)
                {
                    WeaponAttackType=1;
                    spd=spd+3.0f;
                }
                Weapon.transform.up = Vector2.right;
                spd=spd+1.0f;
                Weapon.SetActive(false);
                Invoke("StopChange", 0.5f);  
                attackeffect.GetComponent<Animator>().SetBool("firepunch",true);
                Damage=Damage*2;
            }
            else
            {
                spd=spd-1.0f;
                GetComponent<Animator>().SetBool("IsCrazy",false);
                PlayerChangeAni.SetBool("RtoB",true);
                PlayerType=1;
                Weapon.SetActive(true);
                Invoke("StopChange", 0.5f);  
                attackeffect.GetComponent<Animator>().SetBool("firepunch",false);
                Damage=Damage/2;
            }
            toCrazyCD=0f;
            CanCrazy=0;
        }
    }
    void CrazyEffect()
    {
        if (playerhp<10 && CrazyHealth==true)
        {
            Invoke("Health", 1.0f);
            CrazyHealth=false;   
        }

    }
    void Health()
    {
        playerhp+=1;
        CrazyHealth=true;
        BaseHealth=true;
    }
    void StopChange()
    {
        PlayerChangeAni.SetBool("RtoB",false);
        PlayerChangeAni.SetBool("BtoR",false);

    }

    void Move()
    {
        if (Input.GetKey(KeyCode.D) && nowskill==0)
        {
            playerfaceon=1;
            if (Input.GetKey(KeyCode.S) && Canboost==1)
            {
                MP-=4;
                rb.velocity= Vector3.zero;
                wprb.velocity= Vector3.zero;
                nowskill=1;
                Canboost=0;
                GetComponent<BoxCollider2D>().enabled = false;
                rb.gravityScale = 0;
                rb.AddForce(Vector2.right*Boostforce);
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
                skilltimmer(0.3f);
                toBoostCD=0f;
            }
            transform.Translate(spd*Time.deltaTime,0,0);
            if (WeaponAttackType==1)
            {
                GetComponent<SpriteRenderer>().flipX=true;
                WeaponPointOn=1;
            }
            
            GetComponent<Animator>().SetBool("run",true);

        }
        else if (Input.GetKey(KeyCode.A) && nowskill==0)
        {
            playerfaceon=-1;
           if (Input.GetKey(KeyCode.S) && Canboost==1)
            {
                MP-=4;
                rb.velocity= Vector3.zero;
                wprb.velocity= Vector3.zero;
                nowskill=1;
                Canboost=0;
                GetComponent<BoxCollider2D>().enabled = false;
                rb.gravityScale = 0;
                rb.AddForce(Vector2.left*Boostforce);
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
                skilltimmer(0.3f);
                toBoostCD=0f;
            }
            transform.Translate(-spd*Time.deltaTime,0,0); 
            if (WeaponAttackType==1)
            {
            GetComponent<SpriteRenderer>().flipX=false;
            WeaponPointOn=-1;
            }
            GetComponent<Animator>().SetBool("run",true);
        }
        else
        {
            GetComponent<Animator>().SetBool("run",false);
        }
    }
    void Jump()
    {
        
        if (Input.GetKey(KeyCode.W) && onfloor==1 && nowskill==0)
        {

            rb.AddForce(Vector2.up*Jumpforce);
            onfloor=0;
            
        }
    }
    void BoostSkillCD()
    {
        if (MP>=4)
        {
            boostskillLoad.SetBool("load",false);
            Canboost=1;
        }
        else
        {
            boostskillLoad.SetBool("load",true);
            Canboost=0;
        }
    }
    void CrazySkillCD()
    {
        if (MP>=6)
        {
            CrazySkillLoad.SetBool("load",false);
            CanCrazy=1;
        }
        else
        {
            CrazySkillLoad.SetBool("load",true);
            CanCrazy=0;
        }
    } 
    void WeaponManage()
    {
        if (WeaponAttackType==1)
        {
            if(playerfaceon==1)
            {
                Weapon.transform.up = Vector2.right;
            }
            else
            {
                Weapon.transform.up = Vector2.left;
            }
            Debug.Log("poke");
        }
        if (nowattack==0)
        {
            WeaponMove();
        }
        if ( Input.GetMouseButtonDown(0) && nowattack==0 && nowskill==0 && WeaponAttackType==1)
        {
            attack();
            if (MP<20)
            {
                MP+=1;
            }
        }
        if ( Input.GetMouseButtonDown(0) && nowattack==0 && nowskill==0 && WeaponAttackType==0 && ShootCD>=0)
        {
            WeaponShoot();
            if (MP<20)
            {
                MP+=1;
            }  
        }
        if(WeaponShot==1)
        {
            Weapon.transform.position += Weapon.transform.up * Time.deltaTime * WeaponSpeed;
        }
        if(Input.GetMouseButtonDown(1) && nowattack==0 && nowskill==0 && PlayerType==1)
        {
            if (WeaponAttackType==1)
            {
                WeaponAttackType=0;
                spd=spd-3.0f;
            }
            else
            {
                WeaponAttackType=1;
                spd=spd+3.0f;
            }
        }
        if(WeaponAttackType==0)
        {
            if (WeaponShot==0)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Quaternion rot = Quaternion.LookRotation(Weapon.transform.position - mousePos, Vector3.forward);
                Weapon.transform.rotation = rot;
                Weapon.transform.eulerAngles = new Vector3(0, 0, Weapon.transform.eulerAngles.z);
            }
        }
        if(ShootCD<=0)
        {
            ShootCD+=Time.deltaTime;
        }
        else
        {
            ShootCD=0;
        }

    }
    void WeaponMove()
    {
        if (WeaponAttackType==1)
        {
            Weapon.transform.position = new Vector3( transform.position.x, transform.position.y-0.3f, transform.position.z );
        }
        else
        {
            Weapon.transform.position = new Vector3( transform.position.x-(0.5f*WeaponPointOn), transform.position.y, transform.position.z );
        }
        
 
    }
    void WeaponShoot()
    {
        ShootCD=-1;
        nowattack=1;
        WeaponShot=1;
    }
    void attack()
    {

        Weaponfaceon=playerfaceon;
        
        nowattack=1;
        Weapon.transform.Rotate(0,0,10*Weaponfaceon);
        if (Weaponfaceon==1)
        {
            attackeffect.GetComponent<SpriteRenderer>().flipX=true;
            wprb.AddForce(Vector2.right*100);
        }
        else
        {
            attackeffect.GetComponent<SpriteRenderer>().flipX=false;
            wprb.AddForce(Vector2.left*100);
        }
            
        
        attackeffect.SetActive(true);
        if (PlayerType==2)
        {
            playerhp-=2f;
            attackeffect.GetComponent<Animator>().SetBool("firepunch",true);
            AttackTime=0.5f;
            
        }
        else
        {

            attackeffect.GetComponent<Animator>().SetBool("firepunch",false);
            AttackTime=0.2f;
            
        }
        attackeffect.GetComponent<Animator>().SetBool("normalattack",true);
        normalattacktime(AttackTime);
        attacktimmer(0.2f);
        Weapon.transform.Rotate(0,0,-10*Weaponfaceon);
        Weapon.transform.Rotate(0,0,-10*Weaponfaceon);
        
        
    }
    void diemanager()
    {
        if(playeralive==1)
        {
            if(playerhp<=0f)
            {
                playerhp=0f;
                GetComponent<Animator>().SetBool("IsCrazy",false);
                PlayerType=1;
                Weapon.SetActive(false);
                playeralive=0;
                GetComponent<Animator>().SetBool("die",true);
                if(mobmanager.wave%10!=0)
                {
                    hpword.SetBool("pressh",true);
                    Invoke("waitalive", 0.2f);   
                }
                else
                {
                    Invoke("BossFall", 0.2f);
                    Invoke("BossWaveAlive", 5f);   
                }
            }
        }

        if(playeralive==0 && Input.GetKeyDown("h") )
        {
            
            GetComponent<Animator>().SetBool("alive",false);
            playerhp+=1f;
            if(playerhp>=10f)
            {
                playerhp=10f;
                hpword.SetBool("pressh",false);
                GetComponent<Animator>().SetBool("alive",true);
                playeralive=1;
                GetComponent<BoxCollider2D>().enabled = true;
                rb.gravityScale = 3;
                GetComponent<Animator>().SetBool("waitlive",false);
                Weapon.SetActive(true);
            }
        }
    }
    void waitalive()
    {
        rb.velocity= Vector3.zero;
        GetComponent<Animator>().SetBool("alive",false);
        GetComponent<Animator>().SetBool("die",false);
        GetComponent<Animator>().SetBool("waitlive",true);
        GetComponent<BoxCollider2D>().enabled = false;
        rb.gravityScale = 0;
    }
    void BossFall()
    {
        GetComponent<Animator>().SetBool("alive",false);
        GetComponent<Animator>().SetBool("die",false);
        GetComponent<Animator>().SetBool("waitlive",true);
    }
    void BossWaveAlive()
    {
        mobmanager.basehp-=4;
        playerhp=10f;
        GetComponent<Animator>().SetBool("alive",true);
        playeralive=1;
        GetComponent<BoxCollider2D>().enabled = true;
        rb.gravityScale = 3;
        GetComponent<Animator>().SetBool("waitlive",false);
        Weapon.SetActive(true);
        basewarn.text="石碑似乎消耗了能量幫你回復元氣，一口氣擊敗魔王吧";
        Invoke("endwarn", 3.0f);       
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag=="floor" || other.gameObject.tag=="wall")
        {
            if (other.contacts[0].normal == new Vector2(0f,1f))
            {
                Debug.Log("on floor");
                onfloor=1;
            }

        }
        if (other.gameObject.tag=="fmob")
        {
            if (other.contacts[0].normal == new Vector2(0f,1f))
            {
                rb.AddForce(Vector2.up*100);
                Debug.Log("on floor");
                onfloor=1;
            }
            if (fmob.fmobattack==1 && other.contacts[0].normal != new Vector2(0f,1f))
            {
                sr.color = new Color(1f, 0f, 0f);
                Invoke("ColorBack",0.2f);
                playerhp-=1f;
                fmob.fmobattack=0;
                rb.AddForce(Vector2.up*50);
                if(fmob.fmobfaceon==-1)
                {
                    rb.AddForce(Vector2.left*10);
                }
                else if (fmob.fmobfaceon==1)
                {
                    rb.AddForce(Vector2.right*10); 
                }
                
            }

        }
        if (other.gameObject.tag=="Boss" && Boss.NowSmash==true)
        {
            if (other.contacts[0].normal == new Vector2(0f,-1f))
            {
                playerhp-=3f;
            }
            
        }
        if (other.gameObject.tag=="Boss" && Boss.BossShoot==false && Boss.BossFall==true)
        {
            if (other.contacts[0].normal == new Vector2(1f,0f) && Boss.BossFaceOn==1)
            {
                playerhp-=3f;
            }
            if (other.contacts[0].normal == new Vector2(-1f,0f) && Boss.BossFaceOn==0)
            {
                playerhp-=3f;
            }
            if (other.contacts[0].normal == new Vector2(0f,-1f))
            {
                playerhp-=3f;
            }
            
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag=="BossBeam" && Boss.BossShoot==true)
        {
            playerhp-=4f;
        }
    }


    void skilltimmer (float sec)
    {
        Invoke("SkillTimmerEnd", sec);
    }
    void SkillTimmerEnd()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        GetComponent<BoxCollider2D>().enabled = true;
        rb.gravityScale = 3;
        nowskill=0;
        rb.velocity= Vector3.zero;
        wprb.velocity= Vector3.zero;
    }

    void attacktimmer (float sec)
    {
        Invoke("AttackTimmerEnd", sec);
    }
    void AttackTimmerEnd()
    {
        wprb.velocity= Vector3.zero;
    }  



    void normalattacktime (float sec)
    {
        Invoke("normalattacktimeend", sec);
    }
    void normalattacktimeend()
    {
        attackeffect.GetComponent<Animator>().SetBool("normalattack",false);
        attackeffect.SetActive(false);
        Weapon.transform.Rotate(0,0,10*Weaponfaceon);
        nowattack=0;
    }  


    void Hpbarcontroller()
    {
        hpslider.value=playerhp;
        
    }

    void ExpController()
    {
        UpgradeExp= Lv ;
        ExpSlider.maxValue = UpgradeExp;
        ExpSlider.value=Exp;
        if (Exp>=UpgradeExp)
        {
            Lv+=1;
            Exp=0;
            UpgradePoint+=2;
        }
        
    }
    void MPController()
    {
        MPSlider.value=MP;
        if (MP<20 && MPHealth==true)
        {
            Invoke("HealthMP", 1.0f);
            MPHealth=false;   
        }
    }
    void HealthMP()
    {
        MP+=1;
        MPHealth=true;  
    }
    void endwarn()
    {
        basewarn.text="     ";
    }
    void ColorBack()
    {
        sr.color = new Color(1f, 1f, 1f);
    }

}
