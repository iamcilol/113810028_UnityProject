using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GManager : MonoBehaviour
{
    [SerializeField] GameObject ReplayBottom;
    [SerializeField] GameObject SaveBottom;
    [SerializeField] GameObject PlayBottom;
    [SerializeField] GameObject NewGameBottom;
    [SerializeField] GameObject LoadGameBottom;

    [SerializeField] GameObject Player;

    [SerializeField] GameObject WaveWord;
    [SerializeField] GameObject PlayerHpWord;
    [SerializeField] GameObject PlayerHpSlider;
    [SerializeField] GameObject BaseHpSlider;
    [SerializeField] GameObject BaseWord;
    [SerializeField] GameObject SkillLoad;
    [SerializeField] GameObject CrazyLoad;
    [SerializeField] GameObject MobSystem;
    [SerializeField] GameObject Weapon;
    [SerializeField] GameObject ClearButtom;
    [SerializeField] GameObject UpgradeButtomDmg;
    [SerializeField] GameObject UpgradeButtomSpd;
    [SerializeField] GameObject Exp;
    [SerializeField] GameObject LockSkill;
    [SerializeField] GameObject BossHp;
    [SerializeField] GameObject BossShield;
    [SerializeField] GameObject MP;
    [SerializeField] Text UploadWords;
    [SerializeField] Text LoadWaveWords;
    [SerializeField] Text LvWords;
    [SerializeField] Text SpdLvWords;
    [SerializeField] Text DmgLvWords;
    
    public bool NowIsStop;
    static public bool GameStart;
    static public bool MobSystemSetComplete;
    static public bool PlayerSystemSetComplete;
    static public bool IsLoadGame;
    static public bool CracySkillIsLock;
    static public int BeginWave;
    static public int SpdPlus;
    static public int DmgPlus;
    static public int StartLv;

    void Start()
    {
        StartLv=0;
        BeginWave=0;
        MobSystemSetComplete = false;
        PlayerSystemSetComplete = false;
        GameStart = false;
        NowIsStop = false;
        IsLoadGame=false;
        transform.position=new Vector3(0,-20,-10f);
        Player.SetActive(false);
        MobSystem.SetActive(false);
        WaveWord.SetActive(false);
        PlayerHpWord.SetActive(false);
        PlayerHpSlider.SetActive(false);
        BaseHpSlider.SetActive(false);
        SkillLoad.SetActive(false);
        CrazyLoad.SetActive(false);
        LockSkill.SetActive(false);
        //CracySkillIsLock=true;
        CracySkillIsLock=false;
        Exp.SetActive(false);
        MP.SetActive(false);
        BaseWord.SetActive(false);
        UploadWords.text="";
        LoadWaveWords.text="Wave:"+PlayerPrefs.GetInt("Wave").ToString();
        SpdLvWords.text="";
        DmgLvWords.text="";
        
    }

    void Update()
    {
        NowStop();
        if (mobmanager.basehp<=0 && GameStart == true && MobSystemSetComplete==true)
        {
            ReplayBottom.SetActive(true);
        }
        SaveDebug();
        Upgrade();
        if (GameStart == true && mobmanager.WaveCanCD==true)
        {
            LvWords.text="Lv:"+player.Lv.ToString();
        }
        else
        {
            LvWords.text="";
        }

        
    }
    void SaveDebug()
    {
        if (Input.GetKey(KeyCode.M))
        {
            Debug.Log(BeginWave);
        }
    }
    void NowStop()
    {
        if (Input.GetKey(KeyCode.Escape) && NowIsStop==false && mobmanager.WaveCanCD==true)
        {
            NowIsStop=true;
            SaveBottom.SetActive(true);
            PlayBottom.SetActive(true);
            Time.timeScale = 0;
        }
    }
    void CleanMobs()
    {
        Debug.Log("Mobs are cleaned");
        MobSystem.SetActive(false);
    }
    public void Continue ()
    {
        NowIsStop=false;
        SaveBottom.SetActive(false);
        PlayBottom.SetActive(false);
        Time.timeScale = 1;
    }
    public void SaveData()
    {
        player.PlayerType=1;    
        SaveBottom.SetActive(false);
        PlayBottom.SetActive(false);
        PlayerPrefs.SetInt("Wave", mobmanager.wave);
        PlayerPrefs.SetInt("Lv", player.Lv);
        PlayerPrefs.SetInt("UpgradePoint", player.UpgradePoint);
        PlayerPrefs.SetInt("SpdPlus", SpdPlus);
        PlayerPrefs.SetInt("DmgPlus", DmgPlus);
        BeginWave=PlayerPrefs.GetInt("Wave");
        NewGameBottom.SetActive(true);
        LoadGameBottom.SetActive(true);
        ClearButtom.SetActive(true);

        Exp.SetActive(false);
        MP.SetActive(false);
        MobSystemSetComplete = false;
        PlayerSystemSetComplete = false;
        GameStart = false;
        NowIsStop = false;
        transform.position=new Vector3(0,-20,-10f);
        Player.SetActive(false);
        //Invoke("CleanMobs", 1.0f);
        foreach (Transform child in MobSystem.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        MobSystem.SetActive(false);
        WaveWord.SetActive(false);
        PlayerHpWord.SetActive(false);
        PlayerHpSlider.SetActive(false);
        BaseHpSlider.SetActive(false);
        SkillLoad.SetActive(false);
        CrazyLoad.SetActive(false);
        LockSkill.SetActive(false);
        BaseWord.SetActive(false);
        BossHp.SetActive(false);
        BossShield.SetActive(false);
        UploadWords.text="";
        LoadWaveWords.text="Wave:"+PlayerPrefs.GetInt("Wave").ToString();
    }
    public void replay ()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("ver1");
    } 
    public void NewGame()
    {
        Time.timeScale = 1; 
        BeginWave=0;
        GameStart=true;
        transform.position=new Vector3(0,0,-10f);
        WaveWord.SetActive(true);
        PlayerHpWord.SetActive(true);
        PlayerHpSlider.SetActive(true);
        BaseHpSlider.SetActive(true);
        SkillLoad.SetActive(true);
        CrazyLoad.SetActive(true);
        //LockSkill.SetActive(true);
        //CracySkillIsLock=true;
        LockSkill.SetActive(false);
        CracySkillIsLock=false;
        Exp.SetActive(true);
        MP.SetActive(true);
        BaseWord.SetActive(true);
            
        Invoke("PActive", 0.5f);
        Invoke("MActive", 1.0f);
        NewGameBottom.SetActive(false);
        LoadGameBottom.SetActive(false);
        ClearButtom.SetActive(false);
        
        player.Lv=1;
        player.Exp=0;
        player.UpgradePoint=0;
        LoadWaveWords.text="";
        Player.transform.Translate(0,0.2f,0);

        SpdPlus=0;
        DmgPlus=0;
        StartLv=1;
    }
    void MActive()
    {
        MobSystem.SetActive(true);
    }
    void PActive()
    {
        Player.SetActive(true);
    }
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("Wave")) 
        {
            IsLoadGame=true;
            Debug.Log("HaveData");
            Time.timeScale = 1; 
            BeginWave=PlayerPrefs.GetInt("Wave");
            GameStart=true;
            transform.position=new Vector3(0,0,-10f);
            WaveWord.SetActive(true);
            PlayerHpWord.SetActive(true);
            PlayerHpSlider.SetActive(true);
            BaseHpSlider.SetActive(true);
            SkillLoad.SetActive(true);
            CrazyLoad.SetActive(true);
            Weapon.SetActive(true);
            BaseWord.SetActive(true);
            Invoke("PActive", 0.5f);
            Invoke("MActive", 1.0f);
            NewGameBottom.SetActive(false);
            LoadGameBottom.SetActive(false);
            ClearButtom.SetActive(false);
            Exp.SetActive(true);
            MP.SetActive(true);
            LoadWaveWords.text="";
            Player.transform.Translate(0,0.2f,0);
            SpdPlus=PlayerPrefs.GetInt("SpdPlus");
            DmgPlus=PlayerPrefs.GetInt("DmgPlus");
            StartLv=PlayerPrefs.GetInt("Lv");
            if(StartLv<=5)
            {
                //LockSkill.SetActive(true);
                //CracySkillIsLock=true;
                LockSkill.SetActive(false);
                CracySkillIsLock=false;
            }
            else
            {
                CracySkillIsLock=false;
            }

        } 
        else
        {
            Debug.Log("NoData");
 

        }
    }

    public void UpgradeDmg()
    {
        Debug.Log("Now can upgrade"+ player.UpgradePoint +"times");
        if(player.UpgradePoint>=1)
        {
            DmgPlus+=1;
            player.Damage+=1;
            player.UpgradePoint-=1;
            Debug.Log("Now Dmg is"+ player.Damage);
        }
        UploadWords.text="Point:"+ player.UpgradePoint.ToString();
        DmgLvWords.text="LV:"+DmgPlus.ToString();
    }
    public void UpgradeSpd()
    {
        Debug.Log("Now can upgrade"+ player.UpgradePoint +"times");
        if(player.UpgradePoint>=1)
        {
            SpdPlus+=1;
            player.spd+=0.5f;
            player.UpgradePoint-=1;
            Debug.Log("Now Spd is"+ player.spd);
        } 
        UploadWords.text="Point:"+ player.UpgradePoint.ToString();
        SpdLvWords.text="LV:"+SpdPlus.ToString();
    }
    void Upgrade()
    {
        if (Input.GetKeyDown(KeyCode.C) && NowIsStop==false && mobmanager.WaveCanCD==true && mobmanager.WaveIsCD==true)
        {
        
            transform.position=new Vector3(0,-20,-20);
            PlayerHpWord.SetActive(false);
            WaveWord.SetActive(false);
            PlayerHpSlider.SetActive(false);
            BaseHpSlider.SetActive(false);
            SkillLoad.SetActive(false);
            CrazyLoad.SetActive(false);
            LockSkill.SetActive(false);
            Exp.SetActive(false);
            MP.SetActive(false);
            BaseWord.SetActive(false);
            Invoke("WaveCantCD",0.1f);
            UpgradeButtomDmg.SetActive(true);
            UpgradeButtomSpd.SetActive(true);
            UploadWords.text="Point:"+ player.UpgradePoint.ToString();
            SpdLvWords.text="LV:"+SpdPlus.ToString();
            DmgLvWords.text="LV:"+DmgPlus.ToString();
            
        }
        if (Input.GetKeyDown(KeyCode.C) && mobmanager.WaveCanCD==false)
        {
            transform.position=new Vector3(0,0,-10f);
            PlayerHpWord.SetActive(true);
            WaveWord.SetActive(true);
            PlayerHpSlider.SetActive(true);
            BaseHpSlider.SetActive(true);
            SkillLoad.SetActive(true);
            CrazyLoad.SetActive(true);
            if(player.Lv<=5)
            {
                //LockSkill.SetActive(true);
                LockSkill.SetActive(false);
            }
            Exp.SetActive(true);
            MP.SetActive(true);
            BaseWord.SetActive(true);
            UploadWords.text="";
            mobmanager.WaveCanCD=true;
            UpgradeButtomDmg.SetActive(false);
            UpgradeButtomSpd.SetActive(false);
            SpdLvWords.text="";
            DmgLvWords.text="";
        }

        if(player.Lv>=5)
        {
            LockSkill.GetComponent<Animator>().SetBool("Unlock",true);
            Invoke("Unlock",0.12f);
            CracySkillIsLock=false;
        }
    }
    void Unlock()
    {
        LockSkill.SetActive(false);
    }
    void WaveCantCD()
    {
        mobmanager.WaveCanCD=false;
    }
    public void ClearWave()
    {
        PlayerPrefs.SetInt("Wave", 0);
        PlayerPrefs.SetInt("Lv", 1);
        PlayerPrefs.SetInt("UpgradePoint", 0);
        PlayerPrefs.SetInt("SpdPlus", 0);
        PlayerPrefs.SetInt("DmgPlus", 0);
        LoadWaveWords.text="Wave:"+PlayerPrefs.GetInt("Wave").ToString();
    }
}
