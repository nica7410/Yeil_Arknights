using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyInfo : UnitInfo
{
    

    [SerializeField] private int blockCost; //저지
    [SerializeField] private float range; //범위 최대 크기는 25로 고정
    [SerializeField] private float speed; //이동속도
    private float time; //인덱스

    //경로값
    private int point; //시작위치
    private int route; //경로
    private float moveDelayTime; //이동하기전 대기시간
    private float createTime; //대기시간

   
    public int BlockCost { get => blockCost; set => blockCost = value; }
    public float Range { get => range; set => range = value; }
    public float Speed { get => speed; set => speed = value; }
    public float Time { get => time; set => time = value; }
    public float MoveDelayTime { get => moveDelayTime; set => moveDelayTime = value; }
    public float CreateTime { get => createTime; set => createTime = value; }
    public int Route { get => route; set => route = value; }
    public int Point { get => point; set => point = value; }
}
