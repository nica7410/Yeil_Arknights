using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemGet : MonoBehaviour
{
    [System.Serializable]
    public struct weightedValue
    {
        public string name; //이름
        public int weight;  //가중치
    }
    enum Rank { star4,star5,star6}  //랭크 종류

    public List<OperatorInfo> OperatorInfo6Star = new List<OperatorInfo>();   //6성 오퍼레이터 정보
    public List<Item> OperatorItem6Star = new List<Item>();   //6성 오퍼레이터 슬롯용 아이템

    public List<OperatorInfo> OperatorInfo5Star = new List<OperatorInfo>();   //5성 오퍼레이터 정보
    public List<Item> OperatorItem5Star = new List<Item>();   //5성 오퍼레이터 슬롯용 아이템

    public List<OperatorInfo> OperatorInfo4Star = new List<OperatorInfo>();   //4성 오퍼레이터 정보
    public List<Item> OperatorItem4Star = new List<Item>();   //4성 오퍼레이터 슬롯용 아이템

    private Dictionary<Rank, int> rankTable = new Dictionary<Rank, int>();  //랭크테이블 딕셔너리

    private Dictionary<string, int> rank4 = new Dictionary<string, int>();  //4성 딕셔너리
    private Dictionary<string, int> rank5 = new Dictionary<string, int>();  //5성 딕셔너리
    private Dictionary<string, int> rank6 = new Dictionary<string, int>();  //6성 딕셔너리

    public List<weightedValue> rank4List = new List<weightedValue>();   //4성들의 이름과 가중치
    public List<weightedValue> rank5List = new List<weightedValue>();   //5성들의 이름과 가중치
    public List<weightedValue> rank6List = new List<weightedValue>();   //6성들의 이름과 가중치

    private Rank selected;  // 랜덤 결과로 나온 등급
    private string resultName;  // 랜덤 결과로 나온 이름
    private int total;  //가중치 총합
    private int randomNumber;   //난수값

    void Start()
    {
        rankTable.Add(Rank.star4, 2);
        rankTable.Add(Rank.star5, 8);
        rankTable.Add(Rank.star6, 90);

        foreach(weightedValue weighted in rank4List)
        {
            rank4.Add(weighted.name, weighted.weight);
        }
        foreach (weightedValue weighted in rank5List)
        {
            rank5.Add(weighted.name, weighted.weight);
        }
        foreach (weightedValue weighted in rank6List)
        {
            rank6.Add(weighted.name, weighted.weight);
        }
    }
    public void RandomNumSet()
    {
        resultName = null;
        resetTotal(rankTable);
        selected = CalculateTable(rankTable);
        switch (selected)
        {
            case Rank.star4:
                resetTotal(rank4);
                resultName = CalculateTable(rank4);
                giveOperator(OperatorInfo4Star, OperatorItem4Star);
                break;
            case Rank.star5:
                resetTotal(rank5);
                resultName = CalculateTable(rank5);
                giveOperator(OperatorInfo5Star, OperatorItem5Star);
                break;
            case Rank.star6:
                resetTotal(rank6);
                resultName = CalculateTable(rank6);
                giveOperator(OperatorInfo6Star, OperatorItem6Star);
                break;
        }
    }
    public void RandomTenTimes()
    {
        for (int i = 0; i < 10; ++i)
        {

            RandomNumSet();
        }
    }
    private T CalculateTable<T>(Dictionary<T, int> _Table)
    {
        T tmp = default(T);
        foreach (var _table in _Table)
        {
            if (randomNumber < _table.Value)
            {
                tmp=_table.Key;
                break;
            }
            else
            {
                randomNumber -= _table.Value;
            }
        }
        return tmp;
    }

    private void resetTotal<T>(Dictionary<T, int> _Table)
    {
        total = 0;
        foreach (var item in _Table)
        {
            total += item.Value;
        }
        randomNumber = Random.Range(0, total);
    }
    private void giveOperator(List<OperatorInfo> operatorInfos,List<Item> items)
    {
        bool tmpbool = true;
        for (int i = 0; i < operatorInfos.Count; ++i)
        {
            int tmp = i;
            //Debug.Log("들어간 값 : " + resultName + "/ 디비 값 : " + operatorInfos[tmp].EgName);
            if (resultName == operatorInfos[tmp].EgName)
            {
                Debug.Log("너어는 진짜");

                if(LoadData.instance.UserInfo.userOperator.Count <=0)
                {
                    Inventory.instance.Add(items[tmp]);
                    Inventory.instance.Add(operatorInfos[tmp]);

                    UserOperator userOperator = new UserOperator();
                    userOperator.indexName = operatorInfos[tmp].NameNumber;
                    LoadData.instance.UserInfo.userOperator.Add(userOperator);
                }
                else
                {
                    foreach (UserOperator item in LoadData.instance.UserInfo.userOperator)
                    {
                        if (item.indexName == operatorInfos[tmp].NameNumber)
                        {
                            item.operatorUse++;
                            tmpbool = false;
                            break;
                        }
                    }

                    if(tmpbool==true)
                    {
                        Inventory.instance.Add(items[tmp]);
                        Inventory.instance.Add(operatorInfos[tmp]);
                        UserOperator userOperator = new UserOperator();
                        userOperator.indexName = operatorInfos[tmp].NameNumber;
                        LoadData.instance.UserInfo.userOperator.Add(userOperator);
                    }
                }                
                break;
            }
        }
        LoadData.instance._SaveDataUserInfo();
    }
}
