using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
public class InSelectBtn : MonoBehaviour
{
    public ToggleGroup selectedAddParent;    // 추가 버튼 슬롯들의 부모
    public GameObject squadSlotParent;  // 스쿼드 슬롯의 부모

    [SerializeField]
    private List<InventorySlot> squadsSlot = new List<InventorySlot>();     // 스쿼드 슬롯들의 정보를 가진다
    private InventorySlot[] operatorsSlot;  // 전체 오퍼레이터 정보 배열

    private Item isItemExist;   // 선택한 스쿼드 슬롯이 빈 슬롯인지 확인

    [SerializeField]
    private Item addItem;       // 선택된 오퍼레이터의 아이템
    [SerializeField]
    private OperatorInfo addOperatorInfo;   // 선택된 오퍼레이터의 오퍼레이터인포 스탯정보

    private int selectSlotNumber = 1;   //선택된 스쿼드 슬롯의 인덱스 번호

    private List<OperatorInfo> tempSquadOperatorInfo = new List<OperatorInfo>();
    private List<Item> tempSquadItem = new List<Item>();

    public List<InventorySlot> SquadsSlot { get => squadsSlot; set => squadsSlot = value; }

    private void Start()
    {
        SquadsSlot.AddRange(squadSlotParent.GetComponentsInChildren<InventorySlot>()); // 스쿼드 슬롯 12개의 정보를 가진다.
        operatorsSlot = selectedAddParent.GetComponentsInChildren<InventorySlot>(); // 선택 가능한 캐릭터들의 슬롯 정보를 넣는다

        changeSquadSlotInfoNumber(selectSlotNumber);// 스쿼드 슬롯 초기화
    }
    public void OnButtonClicked(Button clickedButton)   //스쿼드 창에서 선택된 슬롯의 정보를 기억한다.
    {
        Debug.Log(clickedButton.name + " was selected");
        isItemExist = null;
        isItemExist = clickedButton.GetComponent<InventorySlot>().Item; // 스쿼드에서 선택한 버튼의 아이템을 넣는다.

        for (int i = 0; i < operatorsSlot.Length; ++i)  // 모든 오퍼레이터 슬롯을 켜준다.
        {
            operatorsSlot[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < operatorsSlot.Length; ++i)  // 다른 스쿼드 슬롯에 등록된 오퍼레이터면 비 활성화
        {
            foreach (Item _squad in tempSquadItem) // 이미 선택된 오퍼레이터를 비 활성화
            {
                if (operatorsSlot[i].Item == _squad)
                {
                    operatorsSlot[i].gameObject.SetActive(false);
                }
            }
        }

        if (isItemExist != null)    // 선택된 슬롯에 아이템이 존재하면, 해당 아이템의 슬롯을 제일 앞으로
        {
            for (int i = 0; i < operatorsSlot.Length; ++i)
            {
                if (operatorsSlot[i].Item == isItemExist)
                {
                    operatorsSlot[i].transform.SetAsFirstSibling();
                    operatorsSlot[i].GetComponent<Toggle>().isOn = true;
                    operatorsSlot[i].gameObject.SetActive(true);
                    break;
                }
            }
        }

        foreach (InventorySlot _squad in SquadsSlot)   // 선택된 슬롯의 인덱스 번호를 찾아서 기록한다.
        {
            if (_squad.name == clickedButton.name)
            {
                Debug.Log("clicked name : " + clickedButton.name + "Index is " + SquadsSlot.IndexOf(_squad));
                selectSlotNumber = SquadsSlot.IndexOf(_squad);
                break;
            }
        }
    }
    public void OperatorAdded() // 추가 버튼을 누르면 선택된 슬롯에 선택된 캐릭터의 정보를 넣는다.
    {
        addItem = selectedAddParent.ActiveToggles().FirstOrDefault().GetComponent<InventorySlot>().Item;    //선택한 캐릭터의 정보를 넣는다.
        addOperatorInfo = selectedAddParent.ActiveToggles().FirstOrDefault().GetComponent<InventorySlot>().OperatorInfo;
        //스쿼드 창에서 선택한 슬롯의 오퍼레이터인포

        int c = 0;
        bool isEmptySlot = true;

        foreach (InventorySlot _squad in SquadsSlot)
        {
            if (_squad.Item == null)   // 선택된 슬롯의  이전 슬롯 중 아이템이 비어있으면 거기에 아이템 추가
            {
                _squad.ChangeItemData(addItem);
                tempSquadOperatorInfo.Add(addOperatorInfo);
                tempSquadItem.Add(addItem);
                LoadData.instance.UserInfo.organization[Inventory.instance.SelectSquadNumber - 1].squadSloat[c].operatorIndex = addOperatorInfo.NameNumber;
                break;
            }

            if (c >= selectSlotNumber)  //인덱스 보다 값이 커지면 루프 끝
            {
                isEmptySlot = false;
                break;
            }
            ++c;
        }

        if (isEmptySlot == false)   // 선택된 슬롯이 비어있지 않으면, 아이템을 바꾼다.
        {
            SquadsSlot[selectSlotNumber].ChangeItemData(addItem);
            tempSquadOperatorInfo[selectSlotNumber] = addOperatorInfo;
            tempSquadItem[selectSlotNumber] = addItem;
            LoadData.instance.UserInfo.organization[Inventory.instance.SelectSquadNumber - 1].squadSloat[c].operatorIndex = addOperatorInfo.NameNumber;
        }
        LoadData.instance._SaveDataUserInfo();
    }
    public void SettingSloat()
    {
        Debug.Log("SettingSloat Called"+", Length of squad1OperatorInfo : "+ Inventory.instance.squad1OperatorInfo.Count);
        for (int i = 0; i < Inventory.instance.squad1Item.Count; ++i)
        {
            Debug.Log("바뀌는 슬롯 명 : "+SquadsSlot[i].name+ ", 바뀌는 아이템 명 : "+Inventory.instance.squad1Item[i].name);
            SquadsSlot[i].ChangeItemData(Inventory.instance.squad1Item[i]);
        }
    }
    public void changeSquadSlotInfoNumber(int _selectSquadNumber)   // 선택된 스쿼드로 리스트 변경
    {
        switch (_selectSquadNumber)
        {
            case 1:
                tempSquadOperatorInfo = Inventory.instance.squad1OperatorInfo;
                tempSquadItem = Inventory.instance.squad1Item;
                break;
            case 2:
                tempSquadOperatorInfo = Inventory.instance.squad2OperatorInfo;
                tempSquadItem = Inventory.instance.squad2Item;
                break;
            case 3:
                tempSquadOperatorInfo = Inventory.instance.squad3OperatorInfo;
                tempSquadItem = Inventory.instance.squad3Item;
                break;
            case 4:
                tempSquadOperatorInfo = Inventory.instance.squad4OperatorInfo;
                tempSquadItem = Inventory.instance.squad4Item;
                break;
        }
        Inventory.instance.SelectSquadNumber = _selectSquadNumber;
        foreach (InventorySlot _squad in SquadsSlot) // 스쿼드가 바뀔때 마다 스쿼드 슬롯 정보 초기화
        {
            _squad.DeleteItemData();
        }

        for (int i = 0; i < tempSquadItem.Count; ++i)// 바뀐 스쿼드의 정보를 스쿼드 슬롯에 넣는다.
        {
            SquadsSlot[i].ChangeItemData(tempSquadItem[i]);
        }
    }
}