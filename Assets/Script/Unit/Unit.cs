using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{

    [SerializeField] protected UVManager uVManager = null;

    public UVManager UVManager { get { return uVManager; } set { uVManager = value; } }
    [SerializeField]protected EffectUv effectUv =null;
    [SerializeField] private Transform hitingPoint = null;

    //public DissolveCtrl Dissolve { get { return dissolve; } set { dissolve = value; } }

    /// <summary>
    /// 다른곳에서 Unit을 사용시 구분이 불확실해서 열거형으로 처리
    /// bool 값을써도 좋지만 열거형이 가독성이 좋을꺼같아서 열거형 선택
    /// </summary>
    public enum EUnitType
    {
        Operator,
        Enemy
    }
    public EUnitType unitType;
    //Todo : 에너미의 구분을 하는 변수가 이위치에 있어야되는지 확실하게 판단이 안섬 
    //추후 리팩토링을 통해 옳바른 위치로 옮겨줄 예정
    public enum EMonster
    {
        goblin,
        defualt
    }
    public EMonster monsterType;

    public bool isDead;
    [SerializeField] protected int multiAttackNum = 0;    // 다중 공격 가능 수

    [SerializeField] private int attack; //공격력
    [SerializeField] private float attackTime; //공격 속도
    [SerializeField] private int defence; //방어력
    [SerializeField] private int resistance; // 마법저항력
    [SerializeField] private int maxHp; //체력
    [SerializeField] private int nowHp; //체력

    [SerializeField] private int level; //Todo : 리팩토링할때 level로 바꿔야됨

    public int Attack { get => attack; set => attack = value; }
    public float AttackTime { get => attackTime; set => attackTime = value; }
    public int Defence { get => defence; set => defence = value; }
    public int Resistance { get => resistance; set => resistance = value; }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public int NowHp { get => nowHp; set => nowHp = value; }
  
    public int Level { get => level; set => level = value; }
    public Transform HitingPoint { get => hitingPoint; set => hitingPoint = value; }

    //Todo : 20-10-20 버프 종류가 많고 지금은 약식으로 만들어놓음
    enum EBuff
    {
        IncreaseAttack, Dotrecovery
    }


    protected virtual void Awake()
    {
        
    }
    protected virtual void Start()
    {
        this.effectUv = this.GetComponentInChildren<EffectUv>();
    }

    protected virtual void Update()
    {
    }

    //약식 밀리계산 구조만잡음 (영철)
    //protected float DpsCalculation(float per)
    //{
    //    float meleeCal;
    //    float magicCal;

    //    meleeCal = attackType.meleeAtk * per % 100;
    //    //magicCal = attackType.magicAtk - defenceType.magicDef;
    //    return damegeMeleeCal;
    //}

    /// <summary>
    ///범위 만드는 타일
    /// </summary>
    /// <param name="prefap"></param>
    protected void RangeSet(int prefap)//int에서 벡터2로 수정
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefap/Range" + prefap),
            this.transform.position,
            Quaternion.identity);
    }
    //overriding + Vector2 pibot
    protected void RangeSet(int prefap, Vector2 pibot)//int에서 벡터2로 수정
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefap/Range" + prefap),
            this.transform.position + Vector3.right * pibot.x + Vector3.forward * pibot.y,
            Quaternion.identity);
    }
   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    /// <param name="info1"></param>
    public virtual void DpsDeliver(float dps , EffectUv effectUv)
    {
        
    }

    /// <summary>
    ///빈값을 재정의 해서 사용
    /// </summary>
    protected virtual void NormalAttack()
    {

    }

    protected virtual void Skill_00()
    {
        //Skill.skillinfo(EBuff.Dotrecovery);
    }

    protected virtual void Skill_01()
    {

    }

    protected virtual void Skill_02()
    {

    }
    public EMonster Get_MonsterType()
    {
        return monsterType;
    }

    public void SetData(int _value)
    {
        this.multiAttackNum = _value;
    }

    public void PrintData()
    {
        Debug.Log(this.multiAttackNum);
    }
    /// <summary>
    /// 죽었을때 호출하는 함수
    /// </summary>
    protected void IsDeath()
    {
        //curHitPoint = maxHitPoint; //체력 최대값으로 증가
        
        uVManager.SetDie();
        //죽은대상이 에너미 일경우 엔트리 값 증가
        if (unitType == EUnitType.Enemy)
        {
            Stage.instance.NowEntry++;
        }
        //오퍼레이터가 죽으면 배치인원수 다시 증가
        else
        {
            Stage.instance.Deployment++;
        }


    }
    public void EffectSet(EffectUv effectUv)
    {
        effectUv.gameObject.SetActive(true);
        effectUv.transform.position = RangeVector(this.transform.position);
    }

    Vector3 RangeVector(Vector3 vector3)
    {

        return new Vector3(vector3.x + Random.Range(-0.3f, 0.3f),vector3.y, vector3.z + Random.Range(-0.3f, 0.3f));
    }
}
