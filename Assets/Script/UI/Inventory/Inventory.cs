using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Inventory : MonoBehaviour
{
    public static Inventory instance/* = new Inventory()*/;

    //static Inventory()
    //{

    //}
    //private Inventory()
    //{

    //}
    //public static Inventory Instance
    //{
    //    get { return instance; }
    //}

    public List<Item> Operator = new List<Item>();  // 오퍼레이터 슬롯 아이템 리스트
    public List<OperatorInfo> operatorInfo = new List<OperatorInfo>();  //오퍼레이터 인포 스탯 리스트

    public List<OperatorInfo> squad1OperatorInfo = new List<OperatorInfo>();    // 1스쿼드의 오퍼레이터인포 정보
    public List<OperatorInfo> squad2OperatorInfo = new List<OperatorInfo>();    // 2스쿼드의 오퍼레이터인포 정보
    public List<OperatorInfo> squad3OperatorInfo = new List<OperatorInfo>();    // 3스쿼드의 오퍼레이터인포 정보
    public List<OperatorInfo> squad4OperatorInfo = new List<OperatorInfo>();    // 4스쿼드의 오퍼레이터인포 정보


    public List<Item> squad1Item = new List<Item>();  // 1스쿼드의 인벤토리 슬롯 정보
    public List<Item> squad2Item = new List<Item>();  // 2스쿼드의 인벤토리 슬롯 정보
    public List<Item> squad3Item = new List<Item>();  // 3스쿼드의 인벤토리 슬롯 정보
    public List<Item> squad4Item = new List<Item>();  // 4스쿼드의 인벤토리 슬롯 정보

    [SerializeField] public StageInfo stageInfo;

    private int selectSquadNumber = 1;  //현재 선택 되있는 스쿼드의 번호
    public int SelectSquadNumber { get { return selectSquadNumber; } set { selectSquadNumber = value; } }

    public GameObject inventorySlotPrefab;  //인벤토리 슬롯 프리팹
    [SerializeField]
    private GameObject newInventorySlot;     //새로 생성된 프리팹
    public GameObject inventorySlotParent;  //인벤토리 슬롯이 들어갈 부모

    public GameObject selectInSquadSlotPrefab;
    [SerializeField]
    private GameObject selectInSquadSlot;
    public GameObject selectInSquadSlotParent;

    public GameObject operatorEditPanel;    //오퍼레이터 정보 패널

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        if (instance == null)               //인벤토리 파괴 금지
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        instance = this;
        
        


        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public bool Add(Item item)
    {   //오퍼레이터스에 있는 오퍼레이터 목록 생성용
        newInventorySlot = null;
        Operator.Add(item);
        newInventorySlot = Instantiate(inventorySlotPrefab, transform.position, transform.rotation) as GameObject;
        newInventorySlot.transform.SetParent(inventorySlotParent.transform);
        newInventorySlot.GetComponent<InventorySlot>().AddItem(item);
        newInventorySlot.GetComponent<Button>().onClick.AddListener(() => ObjSetActive(operatorEditPanel));

        GameObject tmpInventorySlot = newInventorySlot; // AddListener의 Closure Problem을 위한 임시슬롯 게임오브젝트;
        newInventorySlot.GetComponent<Button>().onClick.AddListener(() =>
            operatorEditPanel.GetComponentInChildren<OperatorEditSlot>().GetObjData(tmpInventorySlot));

        //스쿼드스에 있는 오퍼레이터 목록 생성용
        selectInSquadSlot = Instantiate(selectInSquadSlotPrefab, transform.position, transform.rotation) as GameObject;
        selectInSquadSlot.transform.SetParent(selectInSquadSlotParent.transform);
        selectInSquadSlot.GetComponent<InventorySlot>().AddItem(item);
        selectInSquadSlot.GetComponent<Toggle>().group = selectInSquadSlotParent.GetComponent<ToggleGroup>();

        return true;
    }
    public bool Add(OperatorInfo _operatorInfo)//인벤토리 슬롯에 오퍼레이터인포 스탯 정보 추가
    {
        operatorInfo.Add(_operatorInfo);
        newInventorySlot.GetComponent<InventorySlot>().AddOperatorInfo(_operatorInfo);
        selectInSquadSlot.GetComponent<InventorySlot>().AddOperatorInfo(_operatorInfo);
        return true;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) //다음씬이 호출되면 실행됨
    {
        switch (SelectSquadNumber)
        {
            case 1:
                Stage.instance.InputInfo(Inventory.instance.squad1OperatorInfo);
                break;
            case 2:
                Stage.instance.InputInfo(Inventory.instance.squad2OperatorInfo);
                break;
            case 3:
                Stage.instance.InputInfo(Inventory.instance.squad3OperatorInfo);
                break;
            case 4:
                Stage.instance.InputInfo(Inventory.instance.squad4OperatorInfo);
                break;
        }
        Stage.instance.InputStage(stageInfo);
        Stage.instance.UserInfo = LoadData.instance.UserInfo;

        Destroy(gameObject);
        // Debug.Log("씬 교체됨, 현재 씬: " + scene.name);
    }

    void OnDisable()
    {
        // 델리게이트 체인 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void ObjSetActive(GameObject obj)
    {
        obj.SetActive(true);
    }
}
