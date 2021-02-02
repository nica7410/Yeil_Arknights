using System.Collections;
using System.Collections.Generic;
//using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;
using UnityEngine.UI;
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager spawnManager = null;  //싱글톤

    public float time =0; //스테이지 시간


    //초기세팅
    //따로 구별하는 이유는 플레이어 넘버링과 몬스터의 넘버링이 다르게 돌아가게 구조를 짜서 그렇게됨
    //public List<Operator> operatorPrefab; //캐릭터 객체 생성할 프리팹들
    public List<Enemys> enemyPrefab = null; //몬스터 객체 생성할 프리팹들

    public List<GameObject> spawnPoint = null;//스포너 시작지점

    public List<Operator> operators = new List<Operator>();//오브젝트 풀링용 찾아서 넣어야되기때문에 큐보다 리스트
    public List<Enemys> enemys = new List<Enemys>();//오브젝트 풀링용 찾아서 넣어야되기때문에 큐보다 리스트

    //스폰순서관련
    public int count=0; //생성되는 배열값
    public List<int> spawnTimeArry = new List<int>(); //순서들의 시간을 담을 리스트
    public FloorTileMap tileMap = null;

    /*DB에 올라갈변수들
     Time / 생성될시간
    enemy type / 어떤 몬스터가 나오는지
     */
    public UnityEngine.UI.Image cutoutmask = null;//마스크 리버스용
    public RectTransform CanvasRect = null;    //마스크 리버스용 캔버스
    private void Awake()
    {
        if(spawnManager == null)
        {
            spawnManager = this;
        }
        //cutoutmask = GameObject.Find("CutOutSmall").GetComponent<UnityEngine.UI.Image>();
        //CanvasRect = GameObject.Find("MaskCanvas").GetComponent<RectTransform>();
    }

    /// <summary>
    ///다량으로 생성시키는 
    ///함수매개변수  
    ///count = 몇개를 소환 할것인지 , value = 어느걸 소환할것인지
    ///외부에서 호출해줘야됨
    ///</summary>
    public void InitCreateEnemey(EnemyInfo info ,int count)
    {
        for (int i = 0; i < count; i++)
        {
            enemys.Add(CreateEnemey(info));
        }
    }
    /// <summary>
    ///캐릭터풀링  
    /// </summary>
    public void InitCreateOperator(OperatorInfo info)
    {
        operators.Add(CreateOperators(info));
    }
    /// <summary>
    ///오브젝트 풀링할때 새로 생성하는 함수
    ///여기다가 추가옵션을 계속 넣어줘야된다.
    ///1. 생존여부
    ///2. uv 애니메이션 
    ///3. 스텟
    ///4.애니메이션
    /// </summary>
    private Enemys CreateEnemey(EnemyInfo info)
    {
        Enemys newUnit = Instantiate(Resources.Load<GameObject>("Prefap/Unit/Enemy"), transform).GetComponent<Enemys>();
        newUnit.EnemyInfo = info;
        newUnit.isDead = true;
        newUnit.UVManager = newUnit.GetComponentInChildren<UVManager>();
        newUnit.UVManager.Unit = newUnit;
        newUnit.unitType = Unit.EUnitType.Enemy;
        newUnit.name = ""+info.NameNumber;

        newUnit.Attack = info.Attack;
        newUnit.AttackTime = info.AttackTime;
        newUnit.Defence = info.Defence;
        newUnit.Resistance = info.Resistance;
        newUnit.MaxHp = info.MaxHp;
        newUnit.NowHp = info.NowHp;

        return newUnit;
    }
    /// <summary>
    ///오브젝트 풀링할때 새로 생성하는 함수
    ///Todo : 오퍼레이터와 에너미 생성할때 겹치는 부분이 많다 추후
    ///리팩토링 작업 때 묶을 수 있으면 묶도록 하자.
    /// </summary>
    private Operator CreateOperators(OperatorInfo info)
    {
        Operator newUnit = Instantiate(
            Resources.Load<GameObject>("Prefap/Unit/Character"), transform).GetComponent<Operator>();
        newUnit.OperatorInfo = info;
        newUnit.isDead = true;
        newUnit.UVManager = newUnit.GetComponentInChildren<UVManager>();
        newUnit.UVManager.Unit = newUnit;
        newUnit.unitType = Unit.EUnitType.Operator;
        newUnit.UVManager.ChangeMotion(UVManager.eMotoin.idle);
        newUnit.IsAttack = false;

        //Todo : 완벽한 공식이 정해져있지 않앗기때문에 임시적으로 약식으로 계산 공식 적용
        //유저인포의 정보를 받은 값을 계산해서 정하는 공식으로 둘예정
        foreach (UserOperator item in Stage.instance.UserInfo.userOperator)
        {
            if(item.indexName == info.NameNumber)
            {
                newUnit.Level = item.lv;
                
                newUnit.Attack = StatCalculation(item, info.Attack);
                newUnit.Defence = StatCalculation(item, info.Defence);
                newUnit.Resistance = StatCalculation(item, info.Resistance);
                newUnit.MaxHp = StatCalculation(item, info.MaxHp);
            }
        }
        newUnit.AttackTime = info.AttackTime;
        newUnit.name = info.EgName;
        newUnit.NowHp = newUnit.MaxHp;
        newUnit.BlockNum = info.Block;
        newUnit.Cost = info.Cost;
        newUnit.ReDeployTime = info.ReDeployTime;

    //공격범위 타일 생성
    GameObject goRangTile = Instantiate
            (info.RangePrefap,newUnit.transform);
        goRangTile.transform.position = newUnit.transform.position + Vector3.up *0.1f;
        newUnit.TileRange = goRangTile;

        //공격범위 타일의 방위각을 잡아주는 쿼드 생성
        GameObject azimuth = Instantiate
            (Resources.Load<GameObject>("Prefap/Range/azimuth"), newUnit.transform);//공격방향쿼드
        newUnit.Azimuth = azimuth.GetComponentInChildren<AttackDirection>(); //방위 계산 쿼드 변수에 넣어둠
        
        azimuth.GetComponentInChildren<AttackDirection>().goAtkRangeTile = goRangTile;
        
        azimuth.transform.position = newUnit.transform.position + Vector3.up;
        azimuth.transform.rotation = Quaternion.Euler(90, 0, 45);
        azimuth.gameObject.SetActive(false);
     
        for (int i = 0; i < InStageUI.instance.InStageToggleGroup.Slot.Length; i++)
        {
            if (InStageUI.instance.InStageToggleGroup.Slot[i].operatorInfo == info)
            {
                newUnit.OperatorToggle = InStageUI.instance.InStageToggleGroup.Slot[i];
            }
        }
        
        return newUnit;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="resources"></param>
    /// <returns></returns>
    private GameObject ResourcesObject(string resources)
    {
        return Resources.Load<GameObject>(resources);
    }

    /// <summary>
    ///죽으면 다시 리스트로 넣어주는 함수
    /// </summary>
    public void InsertPool(Unit p_obj)
    {
        p_obj.transform.position = this.transform.position;
        p_obj.transform.parent = this.transform;
        p_obj.isDead = true;

        
        //열거형으로했을 때 swich 문이 더좋긴하지만
        //열거형 구분 변수가 2가지 밖에 안되기때문에 가독성과 복잡도를위해 if로 로직 구성
        if(p_obj.unitType == Unit.EUnitType.Operator)
        {
            operators.Add(p_obj.GetComponent<Operator>());
            p_obj.GetComponent<Operator>().IsAttack = false; //죽었을때 어택 다시꺼놓음
            p_obj.NowHp = p_obj.MaxHp;

            p_obj.GetComponent<Operator>().OperatorToggle.gameObject.SetActive(true);
            p_obj.GetComponent<Operator>().OperatorToggle.DieImage.gameObject.SetActive(true);
            p_obj.GetComponent<Operator>().OperatorToggle.LoadingDtNow = p_obj.GetComponent<Operator>().ReDeployTime;
            p_obj.GetComponent<Operator>().OperatorToggle.IsDelayChk = true;

            p_obj.UVManager.ClipOn();
            p_obj.UVManager.ChangeMotion(UVManager.eMotoin.idle);// 모션 초기 지정 생성할때 지정
        }
        else // 에너미
        {
            enemys.Add(p_obj.GetComponent<Enemys>());
            p_obj.GetComponent<Enemys>().Patten = null; //죽엇을때 플래그 클리어
        }
    }
    //Todo : 풀링 함수
    //2020-11-23 객체를 생성하는데 있어 uv 클래스가 초기에 값들이 정상적으로 세팅 되기전까지 
    //동작이 제대로 이루어 지지 않는 현상이 있어서 임시적으로 동작이 될 수 있도록 코루틴함수로 변경
    //uv 클래스 내부적인 설정 자체를 전체적으로 다 뜯어 고쳐야 될 수 있으므로 결정
    //구체적으로 오류가 발생하는 지점이 객체 생성이 되었을때 uv 클래스에있는 ChangeMotion 함수에 있는 로직상
    //uv 모션이 설정되지가 않고 풀링되는 시점에는 적용이되는데 한프레임 안에 풀링되는 여러 기능들이 합쳐지다보니 대기 시간없이
    //동작이 되었을 때 시각적으로 버그 현상이 일어나는 것처럼 보임

    /// <summary>
    /// 리스트에 있는 객체들 들고오는 함수 객체 타입 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="vector"></param>
    /// <param name="dt"></param>
    /// <param name="route"></param>
    public IEnumerator GetPoolEnemy(int type, Vector3 vector , Transform[] route , float time)
    {
        foreach (Enemys item in enemys)
        {
            if (item.EnemyInfo.NameNumber == type)
            {
                if(item.isDead == true)
                {
                    //Debug.Log("실행됨");
                    item.UVManager.ChangeMotion(UVManager.eMotoin.idle);// 모션 초기 지정 생성할때 지정
                    yield return new WaitForSeconds(0.1f);
                    item.isDead = false;
                    item.transform.position = vector; 
                    item.transform.parent = null;
                    item.GetComponent<Enemys>().FlagCount = 0;

                    item.GetComponent<Enemys>().Patten = route;//초기 경로지정
                    item.indexNumber = count; //카운터 지정
                    ++count; //추가후 카운트 올림
                    //item.EnemyInfo.MoveDelayTime = time;
                    
                    //어쩔수없는 그것
                    //item.EnemyInfo.NowHp = item.EnemyInfo.MaxHp;
                    item.NowHp = item.MaxHp;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 리스트에 있는 객체들 들고오는 함수 객체 타입 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="vector3"></param>
    public void GetPoolOperator(OperatorInfo obj)
    {
        foreach (Operator item in operators)
        {
            if(item.OperatorInfo == obj)
            {
                if (item.isDead == true)
                {
                    item.isDead = false;
                    //item.transform.position = vector3;
                    item.transform.parent = null;
                    item.IsAttack = true;

                    item.OperatorInfo.NowHp = item.OperatorInfo.MaxHp;
                    item.OperatorToggle.gameObject.SetActive(false);
                    Stage.instance.Cost -= obj.Cost;

                }
            

                break;
            }
        }
    }

    /// <summary>
    /// 오퍼레이터를 가지고있는 클래스에서 OperatorInfo 비교하여 해당되는 객체의 트렌스폼을 들고오는 함수
    /// </summary>
    /// <param name="info">받아오는 오퍼레이터 인포</param>
    /// <returns></returns>
    public Operator FindInputOperator(OperatorInfo info)
    {
        Operator temp = null;
        foreach (Operator item in operators)
        {
            if(item.OperatorInfo == info)
            {
                temp = item;
                break;
                
            }
        }
        return temp;
    }

    /// <summary>
    /// 스탯 유저인포에따른 증가값을 연산하는 함수. 플롯값도 있을수 있기때문에 오버로딩
    /// </summary>
    /// <param name="info"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public int StatCalculation(UserOperator info , int data)
    {
        return data+(int)(info.lv * (data * 0.2f));
    }
    public int StatCalculation(OperatorInfo info , int data)
    {
        UserOperator userOperator = Stage.instance.UserInfo.userOperator[info.NameNumber];
        return data+(int)(userOperator.lv * (data * 0.2f));
    }
    public int StatCalculation(OperatorInfo info, float data)
    {
        UserOperator userOperator = Stage.instance.UserInfo.userOperator[info.NameNumber];
        return (int)(data+(userOperator.lv * (data * 0.2f)));
    }
}
