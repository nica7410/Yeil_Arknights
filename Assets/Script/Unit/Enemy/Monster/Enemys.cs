using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemys : Unit
{

    [SerializeField] private int flagCount = 0; //자기가 몇번 카운트인지
    public int indexNumber = 0;
    [SerializeField] private Transform[] patten = null; //몬스터가 받는 경로 리스트
    private float timer;
    [SerializeField] private EnemyInfo enemyInfo = null;

    public Transform[] Patten { get { return patten; } set { patten = value; } }
    //몬스터 이동속도
    [SerializeField] private bool isBlocked = false;
    [SerializeField] private bool isAttack = false;
    private bool isBlockedDead = false;
    public bool IsBlocked { get { return isBlocked; } set { isBlocked = value; } }
    [SerializeField]protected bool isMove = false;
    public bool IsMove { get { return isMove; }set { isMove = value; } }

    public EnemyInfo EnemyInfo { get => enemyInfo; set => enemyInfo = value; }
    public bool IsAttack { get => isAttack; set => isAttack = value; }
    public int FlagCount { get => flagCount; set => flagCount = value; }
    public bool IsBlockedDead { get => isBlockedDead; set => isBlockedDead = value; }

    [SerializeField]private bool flip = false; // 기능구현용
    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < enemyInfo.UVClip.Length; i++)
        {
            uVManager.SettingData
                (enemyInfo.UVClip[i].TexAnim, enemyInfo.UVClip[i].TotalCnt);
        }
        
    }
    protected override void Update()
    {
        
        //죽은 상태가 아닐때
        //Todo : 20-11-19 10:06
        //isDead 에따라 SetActive 끄고 켜주는 기능이 들어가있는데
        //재검토 필요
        if (!isDead)
        {
            if (FlagCount < patten.Length)
            {
                MovetoObj(patten[FlagCount]);
            }
            FindTarget();
            if(timer <= enemyInfo.AttackTime)
            {
                timer += Time.deltaTime;
            }
            
            enemyInfo.MoveDelayTime -= Time.deltaTime;

        }

        //임시로 만들었지만 문제가 있는거 같음
        if (enemyInfo.MoveDelayTime < 0 && !isMove)
        {
            uVManager.ChangeMotion(UVManager.eMotoin.move);
            isMove = true;
        }
    }

    public void Arrival()
    {
        enemyInfo.NowHp = enemyInfo.MaxHp; //체력 최대값으로 증가

        //죽은대상이 에너미 일경우 엔트리 값 증가
        if (unitType == EUnitType.Enemy)
        {
            ++Stage.instance.NowEntry;
            --Stage.instance.Life;
            SpawnManager.spawnManager.InsertPool(this.GetComponent<Enemys>());
        }

    }


    void MovetoObj(Transform target)
    {
        //타겟을 지정해서 움직이는 로직
        if (transform != null && isMove)
        {
            if (isBlocked || IsAttack)
            {
                this.transform.position = Vector3.MoveTowards
                    (
                        Vector3.forward * this.transform.position.z + Vector3.right * this.transform.position.x,
                        Vector3.forward * this.transform.position.z + Vector3.right * this.transform.position.x,
                        EnemyInfo.Speed * Time.deltaTime * 0
                    );
            }
            else
            {
                this.transform.position = Vector3.MoveTowards
                    (
                        Vector3.forward * this.transform.position.z + Vector3.right * this.transform.position.x,
                        Vector3.forward * target.position.z + Vector3.right * target.position.x,
                        EnemyInfo.Speed * Time.deltaTime
                    );
                if (target.position.x == this.transform.position.x && target.position.z == this.transform.position.z)
                {
                    FlagCount++;
                    //Debug.Log("매칭완료2");

                    //목적지 도착 로직
                    if(FlagCount >= patten.Length)
                    {
                        Arrival();
                    }
                }
            }

            // Todo : 기능은 돌아가지만 리팩토링이 필요함
            // 1.컴퍼넌트를 2번이나 사용하게되어 퍼포먼스적으로 불리
            // 2.인라인 함수를 사용하지 않고 동적할당한것도 이번 프로젝트 규칙에 어긋남
            //플립 뒤집는 로직
            if(!flip)
            {
                if (this.transform.position.x - target.position.x > 0)
                {
                    this.transform.GetChild(0).localScale = new Vector3(-2.5f, 2.5f, 1);
                }
                flip = true;
            }
            else
            {
                if (this.transform.position.x - target.position.x < 0)
                {
                    this.transform.GetChild(0).localScale = new Vector3(2.5f, 2.5f, 1);
                }
                flip = false;
            }
            /*
             * 이로직으로 바꿀 생각중
             int size = -2.5f;
             size * (flip?1:-1)
             */
        }
    }

    private void FindTarget()
    {
        
        float dst;
        foreach (Operator target in SpawnManager.spawnManager.operators)
        {
            Vector2 dstA, dstB;
            dstA.x = HitingPoint.position.x;
            dstA.y = HitingPoint.position.z;
            dstB.x = target.HitingPoint.position.x;
            dstB.y = target.HitingPoint.position.z;
            //Debug.Log("EnemyPosition : " + HitingPoint.position + ", TargetPosition : " + target.HitingPoint.position);            
            dst = Vector2.Distance(dstA, dstB);  //자신(메인)의 위치와 적의 위치의 거리를 구함
            //Debug.Log("dst : " + dst);
            if (target.isDead == false)
            {
                if (dst < enemyInfo.Range)
                {
                    if (uVManager.Motoins == UVManager.eMotoin.idle)
                    {
                        if (this.AttackTime <= timer)
                        {
                            uVManager.ChangeMotion(UVManager.eMotoin.attack);
                            target.DpsDeliver(this.Attack,this.effectUv);
                            IsAttack = true;
                            timer = 0;
                            if (target.transform.position.x > this.transform.position.x)
                            {
                                this.transform.GetChild(0).localScale = new Vector3(2.5f, 2.5f, 1);
                            }
                            else
                            {
                                this.transform.GetChild(0).localScale = new Vector3(-2.5f, 2.5f, 1);
                            }
                        }
                    }
                }

            }
        }
    }

    public override void DpsDeliver(float dps, EffectUv effectUv)
    {
        EffectSet(effectUv);

        this.NowHp -= (int)dps;
        if (this.NowHp < 0)
        {
            IsBlockedDead = true;
            IsDeath();
        }
    }
}