using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserOperator
{
    public int indexName; //현재 가지고있는 오퍼레이터의 넘버
    public int lv;
    public float exp; //경험치
    public int operatorUse; //가지고있는 오퍼레이터의 가지고 있는 갯수
}
[CreateAssetMenu(fileName = "New User", menuName = "Inventory/UserStat")]
public class UserInfo : ScriptableObject
{
    public string id; //유저 아이디
    public bool chk;
    public List<UserOperator> userOperator = new List<UserOperator>(); //오퍼레이터 보유현황을 알리는 리스트 변수
    public List<Organization> organization = new List<Organization>(); //4개의 편성창

}
[System.Serializable]
public class Organization
{
    public List<SqudSloat> squadSloat = new List<SqudSloat>(); //12개의 슬롯
}
[System.Serializable]
public class SqudSloat
{
    public int operatorIndex; //받아올 오퍼레이터 넘버값
}


