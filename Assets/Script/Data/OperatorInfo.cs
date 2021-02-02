using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum eClassInfo
{
    Vanguard, //0
    Guard, //1
    Sniper, //2
    Defender, //3
    Medic, //4
    Supporter, //5
    Caster, //6 
    Specialist //7
}
public enum eImageInfo
{
    Claas,
    Card,
    Full,
    Icon
}
//Todo : 이차원 배열이 안나와서 이따구로 대처 
//
[System.Serializable]
public class RangeInfo
{
    public bool[] active = new bool[5];
}

public class OperatorInfo : UnitInfo
{

    //Todo : 영철 : Dictionary<SystemLanguage,string>으로바꿀것을 약속합니다.
    [SerializeField] private string krName; //한글이름
    [SerializeField] private string egName; //영어이름
    
    [SerializeField] private int lv; //Todo : 리팩토링할때 level로 바꿔야됨

    [SerializeField] private Sprite classImage; //넘버링
    [SerializeField] private Sprite cardImage;
    [SerializeField] private Sprite fullImageSprite;
    [SerializeField] private Texture2D fullImage;
    [SerializeField] private Sprite iconImage;

    [SerializeField] private int exp;   //경험치
    [SerializeField] private int block; //저지
    [SerializeField] private int cost; // 코스트

    [SerializeField] private float reDeployTime; //생성 지연시간nu
    [SerializeField] private RangeInfo[] range; //범위 최대 크기는 25로 고정
    [SerializeField] private GameObject rangePrefap;

    [SerializeField] private string traits; //캐릭터 설명란
    [SerializeField] private SkillInfo skillInfo;
    [SerializeField] private string talentsTitle;   //재능 제목


    public string KrName { get => krName; set => krName = value; }
    public string EgName { get => egName; set => egName = value; }
    public Sprite ClassImage { get => classImage; set => classImage = value; }
    public Sprite CardImage { get => cardImage; set => cardImage = value; }
    public Sprite FullImageSprite { get => fullImageSprite; set => fullImageSprite = value; }
    public Texture2D FullImage { get => fullImage; set => fullImage = value; }
    public Sprite IconImage { get => iconImage; set => iconImage = value; }
    
   
    public int Block { get => block; set => block = value; }
    public RangeInfo[] Range { get => range; set => range = value; }
    public string Traits { get => traits; set => traits = value; }
    public string TalentsTitle { get => talentsTitle; set => talentsTitle = value; }
    public SkillInfo SkillInfo { get => skillInfo; set => skillInfo = value; }
    public int Cost { get => cost; set => cost = value; }
    public float ReDeployTime { get => reDeployTime; set => reDeployTime = value; }
    public int Lv { get => lv; set => lv = value; }
    public GameObject RangePrefap { get => rangePrefap; set => rangePrefap = value; }
}
