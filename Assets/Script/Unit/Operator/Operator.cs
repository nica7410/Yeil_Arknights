using System.Collections.Generic;
using UnityEngine;

public class Operator : Unit
{
    [SerializeField] private OperatorInfo operatorInfo = null;
    
    protected float pushForce;  // 미는힘
    protected int deceleration; //감속
    //Todo : 영철 : 내가 만든 변수인데 창현씨가 이쪽작업하면서 사용한지 안한지 확인안해봄
    //확인된후로 삭제 or 사용할 예정
    [SerializeField] private int curBlockNum = 0;  // 현재 저지 중인 수
    [SerializeField] private int blockNum = 0;     // 저지 가능수
    public int indexNumber = 0;
    private OperatorToggle operatorToggle = null;

    public Unit unit = null; //사용되지 않음1
    public Unit GetUnit { get; set; } //사용되지않음1
    public OperatorInfo OperatorInfo { get => operatorInfo; set => operatorInfo = value; }

    [SerializeField]private AttackDirection azimuth = null;
    public AttackDirection Azimuth { get => azimuth; set => azimuth = value; }

    private GameObject tileRange = null;
    public GameObject TileRange { get => tileRange; set => tileRange = value; }

    [SerializeField] private int cost; // 코스트
    [SerializeField] private float reDeployTime; //생성 지연시간nu

 
    public int Cost { get => cost; set => cost = value; }
    public float ReDeployTime { get => reDeployTime; set => reDeployTime = value; }


    //데미지 처리에대한 변수 
    private bool isEnemySame; // 중첩 방지 체크 변수(영철 : 주석)

    [SerializeField] private float timer;    // 공격속도 타이머
    [SerializeField] private int curAttackNum = 0;   //현재 공격 중인 수

    private List<Enemys> totalEnemies;// 전체 적 -> 풀링된 적 리스트로 교체 Todo: 싱글톤 바로 쓰는거로 수정해야함
    [SerializeField] private List<Enemys> inRangeEnemies;    // 범위 안에 든 적
    private List<Enemys> inRangeEnemiesTemp;// 범위 밖을 나간 적
    [SerializeField] private List<Enemys> blockEnemyList;    // 저지 중인 적
    private List<Enemys> blockEnemyListTemp;// 저지 범위 밖을 나간 적

    private Vector2 dstA, dstB; //Todo :주석 달아주세요
    private float dst; //Todo : 요것두요

    private Transform[] atkRangeTrs;         //자식이 모두 들어온 후 자식 정보를 받도록 변경해야함
    private Transform mainTrs; //사용되지 않음2
    
    private Vector3 enemyVec3;
    private Vector3  mainVec3; //Todo : 주석 필요함

    private bool noEnemy;        // 해당 적이 여전히 공격 범위 안에 있는지 판별    

    [SerializeField]private bool isAttack = false; //공격상태 유무
    public bool IsAttack { get => isAttack; set => isAttack = value; }
    public OperatorToggle OperatorToggle { get => operatorToggle; set => operatorToggle = value; }
    public int CurBlockNum { get => curBlockNum; set => curBlockNum = value; }
    public int BlockNum { get => blockNum; set => blockNum = value; }

    protected override void Awake()
    {
        totalEnemies = SpawnManager.spawnManager.enemys;    
        atkRangeTrs = this.GetComponentsInChildren<Transform>();
        azimuth = GetComponentInChildren<AttackDirection>();
        inRangeEnemies = new List<Enemys>();
        inRangeEnemiesTemp = new List<Enemys>();
        blockEnemyList = new List<Enemys>();
        blockEnemyListTemp = new List<Enemys>();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        timer = 0;
    

        SetData(2);

        //스프라이트 시트 넣어주기 작업
        for(int i=0; i<operatorInfo.UVClip.Length;i++)
        {
            uVManager.SettingData
                (operatorInfo.UVClip[i].TexAnim, operatorInfo.UVClip[i].TotalCnt);
        }

    }
    /// <summary>
    /// 
    /// </summary>
    protected override void Update()
    {
        base.Update();
        timer += Time.deltaTime;
        CurBlockNum = blockEnemyList.Count;
        if(isAttack == true)
        {
            EnemySearch();  //적 수색
            BlockEnemy();            
            BlockEnemyDeath();
            EnemyHit();     //판별된 적 공격
        }

        if (isDead == true)
        {
            OperatorDead();
        }
    }// end of Update
    /// <summary>
    /// 
    /// </summary>
    private void EnemySearch()
    {
        //공격 범위 내에 적이 있는지 판별
        if (this.curAttackNum < this.multiAttackNum)//최대 공격수보다 현재 공격 가능수가 작거나 같을때
        {
            //Debug.Log("현재공격" + this.curAttackNum + "최대공격" + this.multiAttackNum);
            if (operatorInfo.AttackTime <= timer)    //공격속도가 시간보다 작거나 같을때
            {
                //Debug.Log("적 탐색 공격시간");
                foreach (Enemys enemy in totalEnemies)  //전체 적 반복문 Todo: 싱글톤 바로 쓰는거로 수정해야함
                {
                    if (inRangeEnemies.Count > 0)   //리스트에 적이 추가되어 있으면   
                    {
                        if (SameEnemyDistinct(inRangeEnemies, enemy)) //적이 중복인지 검사하여 참이면 루프 건너 띔
                            continue;
                    }

                    //Debug.Log("중복탐지 판별");
                    enemyVec3 = enemy.transform.position;
                    dstA.x = enemyVec3.x;
                    dstA.y = enemyVec3.z;

                    mainVec3 = this.transform.position;
                    dstB.x = mainVec3.x;
                    dstB.y = mainVec3.z;

                    dst = Vector2.Distance(dstA, dstB);  //자신(메인)의 위치와 적의 위치의 거리를 구함

                    if (dst <= 3.5f)    //적이 큰 공격범위 안에 있으면
                    {
                        //Debug.Log("Enemy is in Wide Range");
                        foreach (Transform atkRangeTr in atkRangeTrs)   //공격 범위들의 반복문
                        {
                            dstB.x = atkRangeTr.position.x;
                            dstB.y = atkRangeTr.position.z;

                            dst = Vector2.Distance(dstA, dstB);

                            if (dst <= 0.5f)                    // 적이 실제 공격 범위안에 있으면
                            {
                                if (this.curAttackNum < this.multiAttackNum)
                                {
                                    if (SameEnemyDistinct(inRangeEnemies, enemy))
                                        return;
                                    else
                                    {
                                        inRangeEnemies.Add(enemy);// 공격 중인 적 리스트에 추가
                                        ++this.curAttackNum;        // 현재 공격중인 적의 수 추가
                                    }
                                    //Debug.Log("Enemy is in Attack List");
                                    //Debug.Log("리스트 추가 직후" + inRangeEnemies.Count);
                                }
                                else      //현재 공격 수가 최대 공격 가능수와 같아지면 반복문 종료                               
                                    break;

                                //Debug.Log("Enemy is in Small Range");
                            }
                        }//end of trs foreach
                    }//end of big dst if
                }//end of Enmey foreach
            }//end of timer if
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void EnemyHit()
    {
        if (inRangeEnemies.Count > 0)   // 판별된 적 리스트에 값이 있으면
        {
            inRangeEnemiesTemp.Clear(); // 임시 판별 적 리스트 초기화
            noEnemy = true;
            if (operatorInfo.AttackTime<= timer)
            {
                foreach (Enemys enemyy in inRangeEnemies)
                {
                    dstA.x = enemyy.transform.position.x;
                    dstA.y = enemyy.transform.position.z;
                    foreach (Transform atkRangeT in atkRangeTrs)
                    {
                        dstB.x = atkRangeT.position.x;
                        dstB.y = atkRangeT.position.z;
                        dst = Vector2.Distance(dstA, dstB);

                        if (dst <= 0.5f)    // 실제 공격 범위 안에 적이 있으면
                        {
                            uVManager.ChangeMotion(UVManager.eMotoin.attack);

                            enemyy.DpsDeliver(this.Attack, this.effectUv);    //데미지 전달

                            if(enemyy.transform.position.x > this.transform.position.x)
                            {
                                azimuth.QuadRotation(true);
                            }
                            else
                            {
                                azimuth.QuadRotation(false);
                            }
                            
                            timer = 0;
                            noEnemy = false;    // 적이 있음
                            break;
                        }
                    }
                    if (noEnemy)    // 적이 없으면
                    {
                        --curAttackNum; // 공격 중인 적의 수 감소
                        inRangeEnemiesTemp.Add(enemyy); // 임시 적 리스트 추가
                        // Todo : 임시 리스트를 사용하지 않고 foreach 안의 인덱스를 바로 삭제하면 에러 발생
                        // 그래서 임시 리스트를 생성하여 값을 추가한뒤 추후에 일괄삭제를 한다.
                    }
                }

                if (inRangeEnemiesTemp.Count > 0)   // 임시 리스트에 값이 있으면, 해당 하는 적 리스트에서 제거
                {
                    inRangeEnemies.RemoveAll(inRangeEnemiesTemp.Contains);
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void BlockEnemy()  //적이 메인의 위치 안에 있고, 현재 저지수가 저지 가능 수 보다 작을때
    {
        if (CurBlockNum <= BlockNum)
        {
            foreach (Enemys enemy in totalEnemies)
            {
                if (blockEnemyList.Count > 0)
                {
                    if (SameEnemyDistinct(blockEnemyList, enemy))// 이미 저지된 적이면 루프 건너 띔
                        continue;
                }

                enemyVec3 = enemy.transform.position;
                dstA.x = enemyVec3.x;
                dstA.y = enemyVec3.z;

                mainVec3 = this.transform.position;
                dstB.x = mainVec3.x;
                dstB.y = mainVec3.z;

                dst = Vector2.Distance(dstA, dstB);  //자신(메인)의 위치와 적의 위치의 거리를 구함
                if (enemy.IsBlocked == false)
                {
                    if (dst <= 0.5f)
                    {

                        enemy.IsBlocked = true;
                        CurBlockNum += enemy.EnemyInfo.BlockCost;
                        blockEnemyList.Add(enemy);
                        Debug.Log("최대 저지 가능 수 : " + BlockNum);
                        Debug.Log("현재 저지 가능 수 : " + CurBlockNum);
                        if (CurBlockNum > BlockNum)
                        {
                            Debug.Log("초가된 저지 수 : " + CurBlockNum);
                            enemy.IsBlocked = false;
                            CurBlockNum -= enemy.EnemyInfo.BlockCost;
                            blockEnemyList.Remove(enemy);
                        }

                    }

                }
                
            }
        }
    }
    private void BlockEnemyDeath()
    {
        if (blockEnemyList.Count > 0)
        {
            blockEnemyListTemp.Clear();
            foreach (Enemys enemys in blockEnemyList)
            {
                if(enemys.IsBlockedDead)
                {
                    CurBlockNum -= enemys.EnemyInfo.BlockCost;//현재 저지중인 수 감소
                    blockEnemyListTemp.Add(enemys);
                }
            }

            if (blockEnemyListTemp.Count > 0)
            {
                blockEnemyList.RemoveAll(blockEnemyListTemp.Contains);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_Enemys"></param>
    /// <param name="_sameEnemy"></param>
    /// <returns></returns>
    private bool SameEnemyDistinct(List<Enemys> _Enemys, Enemys _sameEnemy)    //리스트에 들어가 있는 적인지 판별
    {
        isEnemySame = false;
        foreach (Enemys _enemy in _Enemys)  //범위 내 적과 전체 적을 비교해 중복시 루프 탈출
        {
            if (_sameEnemy.indexNumber == _enemy.EnemyInfo.NameNumber)
            {
                isEnemySame = true;
                return isEnemySame;    //적이 동일하면 참 반환
            }
        }
        return isEnemySame;    //적이 다르면 거짓 반환
    }
    /// <summary>
    /// 죽으면 발동되는 함수인가보다.
    /// </summary>
    private void OperatorDead()
    {
        foreach (Enemys blockedEnemy in blockEnemyList)
        {
            blockedEnemy.IsBlocked = false;
            CurBlockNum -= blockedEnemy.EnemyInfo.BlockCost;
        }
        blockEnemyList.Clear();
    }
    /// <summary>
    /// 
    /// </summary>
    public void SetChildTrs()
    {
        atkRangeTrs = this.GetComponentsInChildren<Transform>();
    }

    public override void DpsDeliver(float dps, EffectUv effectUv)
    {
        EffectSet(effectUv);

        this.NowHp -= (int)dps;
        
        if (this.NowHp < 0)
        {
            IsDeath();
        }
    }

}
