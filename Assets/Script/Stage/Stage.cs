using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public static Stage instance = null;
    //Todo : 데이터를 전달해줘야될경우도 있어서 어쩔수없이 싱글톤
    //중복 생성방지는 나중에 리팩토링할때 만들 예정

    //스폰 패턴 구조체 
    [System.Serializable]
    public struct EnemySpawn
    {
        public Unit.EMonster value;
        public float waitTime; //대기시간
        public int point; //시작위치
        public int route; //경로
        public float moveDelayTime; //움직임 대기시간
    }


    //스테이지에 세팅해주는 오퍼레이터들
    //Flag 관련
   
    //스폰관련
    public EnemySpawn[] enmySpawns = null;
    public List<EnemyInfo> enemyInfo = null;
    public List<OperatorInfo> operatorInfo = null;
    public StageInfo stageInfo = null;
    [SerializeField] private UserInfo userInfo = null;

    //경로
    public FloorTileMap tileMap;
  
    [SerializeField] private float stageWaitTime = 0;

    //UI관련
    [SerializeField] private InGameUImanager inGameUImanager = null;
    [SerializeField] private InStageToggleGroup inStageToggleGroup = null;

    [SerializeField] private float nowTime = 0;
    [SerializeField] private int deployment = 0; //배치가능인원

    [SerializeField] private int life = 0;
    [SerializeField] private int nowEntry = 0;
    [SerializeField] private int totalEntry = 0;
    [SerializeField] private int cost = 0;

    public float NowTime { get => nowTime; set => nowTime = value; }
    public int Deployment { get => deployment; set => deployment = value; }
    public int Life { get => life; set => life = value; }
    public int NowEntry { get => nowEntry; set => nowEntry = value; }
    public int TotalEntry { get => totalEntry; set => totalEntry = value; }
    public int Cost { get => cost; set => cost = value; }
    public InStageToggleGroup InStageToggleGroup { get => inStageToggleGroup; set => inStageToggleGroup = value; }
    public UserInfo UserInfo { get => userInfo; set => userInfo = value; }

    private void Awake()
    {
        instance = this;
        NowTime = 1f; //초기 타임값세팅
    }
    void Start()
    {
        Time.timeScale = 1f; //초기 속도 지정
        stageWaitTime = 2f;
        //UI들 정보 받아오기
        inGameUImanager = Transform.FindObjectOfType<InGameUImanager>();
        InStageToggleGroup = Transform.FindObjectOfType<InStageToggleGroup>();
        cost = stageInfo.Cost;
        Life = stageInfo.Life;

        //토글 세팅
        foreach (OperatorInfo info in operatorInfo)
        {
            InStageToggleGroup.Slot[InStageToggleGroup.ChildCount - 1] = InStageToggleGroup.InstaceToggle(info);

        }
        StageSetting();
        StartCoroutine(WaitStarEnemy()); //대기 시간 이후에 몬스터 풀링된것을 빼옴

        totalEntry = stageInfo.EnemySpawn.Length; //목표치세팅값은 몬스터 스폰될 수량 만큼


    }
    public void InputInfo(List<OperatorInfo> info)
    {
        foreach (OperatorInfo item in info)
        {
            operatorInfo.Add(item);
        }
    }
    public void InputStage(StageInfo info)
    {
        stageInfo = info;
    }
    private void Update()
    {

    }
    /// <summary>
    ///풀링 세팅 함수 
    /// </summary>
    private void StageSetting()
    {

        for (int i = 0; i < stageInfo.EnemySettings.Length; i++)
        {
            SpawnManager.spawnManager.InitCreateEnemey
                (stageInfo.EnemySettings[i].EnemyInfo, stageInfo.EnemySettings[i].Count);
        }

        foreach (OperatorInfo item in operatorInfo)
        {
            SpawnManager.spawnManager.InitCreateOperator(item);
        }
    }
    IEnumerator WaitStarEnemy()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(SpawnCallEnmey());
    }
    /// <summary>
    /// 시간마다 풀링에서 빼내오는 함수
    /// </summary>
    /// <returns></returns>

    IEnumerator SpawnCallEnmey()
    {
        for (int i = 0; i < stageInfo.EnemySpawn.Length; i++)
        {
            StartCoroutine
                (
                    SpawnManager.spawnManager.GetPoolEnemy
                    (
                    stageInfo.EnemySpawn[i].EnemyIndex,
                    tileMap.GetTile_Transform(stageInfo.EnemySpawn[i].Point),
                    tileMap.GetPathWithPatten(stageInfo.PattenFlags[stageInfo.EnemySpawn[i].Route].transforms),
                    stageInfo.EnemySpawn[i].MoveDelayTime
                    )
                );
            //
            Debug.Log(i);
            yield return new WaitForSeconds(stageInfo.EnemySpawn[i].NextTime);
        }
    }
    /// <summary>
    /// 왜넣엇는지 모르겟슴
    /// </summary>
    void InputInfoData()
    {

        foreach (OperatorInfo item in LoadData.instance.OperatorInfos)
        {


        }
    }


}
