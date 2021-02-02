using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemySetting
{
    [SerializeField] private EnemyInfo enemyInfo;
    [SerializeField] private int count;

    public EnemyInfo EnemyInfo { get => enemyInfo; set => enemyInfo = value; }
    public int Count { get => count; set => count = value; }
}
[System.Serializable]
public class EnemySpawn
{
    [SerializeField] private int enemyIndex;
    
    //경로값
    [SerializeField] private int point; //시작위치
    [SerializeField] private int route; //경로
    [SerializeField] private float moveDelayTime; //이동하기전 대기시간
    [SerializeField] private float nextTime; //대기시간

    public int EnemyIndex { get => enemyIndex; set => enemyIndex = value; }
    public int Point { get => point; set => point = value; }
    public int Route { get => route; set => route = value; }
    public float MoveDelayTime { get => moveDelayTime; set => moveDelayTime = value; }
    public float NextTime { get => nextTime; set => nextTime = value; }
    
}

[System.Serializable]
public class PattenFlag
{
    public int[] transforms;
}

public class StageInfo : MonoBehaviour
{
    [SerializeField] private EnemySetting[] enemySettings;
    [SerializeField] private EnemySpawn[] enemySpawn;

    [SerializeField] private float startWaitTime;
    [SerializeField] private int deployment; //배치가능인원
    [SerializeField] private int life;
    [SerializeField] private int nowEntry;
    [SerializeField] private int totalEntry;
    [SerializeField] private int cost;
    [SerializeField] private PattenFlag[] pattenFlags;

    

    public EnemySpawn[] EnemySpawn { get => enemySpawn; set => enemySpawn = value; }
    public float StartWaitTime { get => startWaitTime; set => startWaitTime = value; }
    public int Deployment { get => deployment; set => deployment = value; }
    public int Life { get => life; set => life = value; }
    public int NowEntry { get => nowEntry; set => nowEntry = value; }
    public int TotalEntry { get => totalEntry; set => totalEntry = value; }
    public int Cost { get => cost; set => cost = value; }
    public PattenFlag[] PattenFlags { get => pattenFlags; set => pattenFlags = value; }
    public EnemySetting[] EnemySettings { get => enemySettings; set => enemySettings = value; }


}
