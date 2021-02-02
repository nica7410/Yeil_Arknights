using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InStageToggleGroup : MonoBehaviour
{
    [SerializeField] private OperatorToggle[] slot = null;
    [SerializeField] private OperatorToggle nowToggle = null;

    [SerializeField] private int childCount = 1;
    public OperatorToggle NowToggle { get { return nowToggle; } set { nowToggle = value; } }

    public OperatorToggle[] Slot { get => slot; set => slot = value; }
    public int ChildCount { get => childCount; set => childCount = value; }

    void Start()
    {
        slot = this.GetComponentsInChildren<OperatorToggle>();
        childCount = 1;
        for (int i = 0; i < slot.Length; i++)
        {
            slot[i].gameObject.SetActive(false);
        }
    }

    public OperatorToggle InstaceToggle(OperatorInfo info)
    {
        slot[childCount].gameObject.SetActive(true);
        slot[childCount].SettingsData(); //생성이후 자기 자신이 가지고 있는 데이터를 불러와야 세팅가능
        slot[childCount].operatorInfo = info;
        slot[childCount].ClassImage.sprite = info.ClassImage;
        slot[childCount].Bgi.sprite = info.IconImage;
        slot[childCount].DelayTime = info.ReDeployTime;
        slot[childCount].operatorInfo.ReDeployTime = 5f; //초기값 세팅
        slot[childCount].Count = ChildCount;
        slot[childCount].gameObject.name = "" + childCount;
        slot[childCount].CostText.text = "" + info.Cost;
        ++childCount;

        return slot[childCount-1];
    }
   
    public void ToggleChoice()
    {
        for(int i=0;i< slot.Length;i++)
        {
           if(slot[i] == nowToggle)
           {
                slot[i].isChoice = true;
           }
           else 
           {
                slot[i].isChoice = false;
           }
        }
    }  
}
