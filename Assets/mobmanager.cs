using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mobmanager : MonoBehaviour
{
    static public int wave;
    public int MobCount;
    static public int WaveEndCount;
    public int WEC;
    public int W;
    float MobLoc;

    [SerializeField] GameObject[] mobs;
    

    [SerializeField] GameObject Base;
    static public int basehp;
    [SerializeField] Text basewarn;
    public int basebrokewarn;

    [SerializeField] Text WaveWord;


    public int mobname;
    public GameObject themobs;

    public float CD;

    static public int[] MobHp;
    public int[] Mobhp;
    public int mk;
    public int[] MobType;
    
    public Slider BaseHpSlider;
    GameObject BaseHpBar;
    public int BaseNowHp;
    public float BHM;

    static public bool WaveIsCD;
    static public bool WaveCanCD;

    [SerializeField] GameObject BiggBoss;
    [SerializeField] GameObject BossHp;
    [SerializeField] GameObject BossShield;
    public Slider BossHpSlider;
    public Slider BossShieldSlider;
    void Start()
    {     
    }
    void Update()
    {
        NowStart();
        if(GManager.MobSystemSetComplete==true)
        {
            WaveUI();
            nextwave();
            WEC=WaveEndCount;
            W=wave;


            //basewarnning();
            if (basehp<=0)
            {
                Invoke("lose", 1.0f);
                Base.GetComponent<Animator>().SetBool("break",true);
            }
            Mobhp=MobHp;
            BaseHpManage();
        }
        BossHpController();
        
    }
    void nextwave()
    {
        if (wave<5)
        {
            if (WaveEndCount==wave )
            {
                WaveIsCD=true;
                if (WaveCanCD==true)
                {
                    CD+=Time.deltaTime;
                }
                
                if (CD>=3)
                {
                    WaveIsCD=false;
                    CD=0;
                    wave+=1;
                    WaveEndCount=0;
                    wavestart();
                }

            }
        }
        else
        {
            if (WaveEndCount>=5)
            {
                WaveIsCD=true;
                if (WaveCanCD==true)
                {
                    CD+=Time.deltaTime;
                }
                if (CD>=3)
                {
                    WaveIsCD=false;
                    CD=0;
                    wave+=1;
                    WaveEndCount=0;
                    if(wave%10!=0)
                    {
                        wavestart();
                    }
                    else
                    {
                        BossWave();
                    }
                }
            }
        }

    }

    void NowStart()
    {
        if (GManager.GameStart==true && GManager.MobSystemSetComplete==false)
        {
            BossHp.SetActive(true);
            BossShield.SetActive(true);
            BaseHpSlider= GameObject.Find("BaseHp").GetComponent<Slider>();
            BaseHpBar= GameObject.Find("BaseHp");
            BossHpSlider=BossHp.GetComponent<Slider>();
            BossShieldSlider=BossShield.GetComponent<Slider>();
            GManager.MobSystemSetComplete=true;
            CD=0;

            basewarn.text="     ";

            MobHp=new int [5];
            MobType=new int[5];

            MobLoc=0f;
            wave=GManager.BeginWave;
            WaveEndCount=0;

            mk=0;
            if(wave%10==0 && wave!=0)
            {
                BossWave();
            }
            else
            {   
                wavestart();
                BossHp.SetActive(false);
                BossShield.SetActive(false);
            }

            BaseNowHp=20;
            BaseHpBar.SetActive(false);
            WaveCanCD=true;


            if(GManager.IsLoadGame==true)
            {
                GManager.IsLoadGame=false;
            }
        }
    }
    void wavestart()
    {
        if(basehp<20)
        {
            basehp+=5;
        }
        if(basehp>20)
        {
            basehp=20;
        }
        basebrokewarn=20;

        MobCount=wave;

        if (wave>=5)
        {
            MobCount=5;
        }
        
        for (int i = 0; i < MobCount; i++)
        {
            MobLoc=i*1f;
            mk = Random.Range(0,mobs.Length);
            GameObject sfmob = Instantiate(mobs[mk],transform);
            if (mk==0)
            {
                sfmob.transform.position = new Vector3( 7.82f+MobLoc, -0.1f, 0f);   
            }
            else if (mk==1)
            {
                sfmob.transform.position = new Vector3( 7.82f+MobLoc, 2.49f, 0f);
            }
            MobType[i]=mk;
                     
        }  
        
        for (int i = 0; i < MobCount; i++)
        {
            Transform child = themobs.transform.GetChild(i);
            mobname = child.GetSiblingIndex();
            child.name = ""+(i);
            Debug.Log(mobname);
            if (MobType[i]==0)
            {
                MobHp[i]=10+wave*2;
            }
            else if (MobType[i]==1)
            {
                MobHp[i]=wave*2;
            }                

        }

    }

    void BaseHpManage()
    {
        BaseHpSlider.value=basehp;
        if (BaseNowHp!=basehp)
        {
            BaseNowHp=basehp;
            BHM-=1.5f;
        }
        if (BHM<2)
        {
            BHM+=Time.deltaTime;
        }
        
        if (BHM>=2)
        {
            BaseHpBar.SetActive(false);
        }
        else
        BaseHpBar.SetActive(true);
    }
    void CloseHpBar()
    {
        BaseHpBar.SetActive(false);
        
    }
    void basewarnning()
    {
        if (basehp<=5 && basebrokewarn>5)
        {
            basebrokewarn=5;
            basewarn.text="石碑搖搖欲墜，似乎快崩塌了!";
            Invoke("endwarn", 3.0f);
        }
        else if(basehp<=10 && basebrokewarn>10)
        {
            basebrokewarn=10;
            basewarn.text="更多碎裂聲出現了，石碑狀態好像並不樂觀。";
            Invoke("endwarn", 3.0f);
        }
        else if(basehp<=15 && basebrokewarn>15)
        {
            basebrokewarn=15;
            basewarn.text="啪嚓，好像從石碑那傳出了碎裂聲?";
            Invoke("endwarn", 3.0f);
        }
        

    }
    void endwarn()
    {
        basewarn.text="     ";
    }
    void lose()
    {
        Time.timeScale = 0;
    }
    void WaveUI()
    {
        if (wave%10==0)
        {
            WaveWord.text="";
        }
        else if (wave<10)
        {
            WaveWord.text="Wave:00"+wave.ToString();
        }
        else if(wave<100)
        {
            WaveWord.text="Wave:0"+wave.ToString();
        }
        
    }

    void BossWave()
    {
        basewarn.text="似乎因魔王的力量，石碑的回復魔法陣失效了";
        Invoke("endwarn", 3.0f);
        GameObject bigboss = Instantiate(BiggBoss,transform);
        bigboss.transform.position = new Vector3( 8f, 2.49f, 0f);
        BossHp.SetActive(true);
        BossShield.SetActive(true);
        basehp=20;
    }
    void BossHpController()
    {
        BossHpSlider.value=Boss.BossHp;
        BossShieldSlider.value=Boss.BossShield;
    }
    

}
