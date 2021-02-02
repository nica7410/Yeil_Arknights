using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

//using System.Linq;

public class LoadData : MonoBehaviour
{

    public static LoadData instance = null;

    [SerializeField] private Sprite[] classIcon = null;
    [SerializeField] private Sprite[] card = null;
    [SerializeField] private Sprite[] full = null;
    [SerializeField] private Sprite[] icon = null;
    [SerializeField] private Texture2D[] operatorSheet = null;
    [SerializeField] private Texture2D[] enemySheet = null;
    [SerializeField] private OperatorInfo[] operatorInfos = null;
    [SerializeField] private Item[] items = null;

    [SerializeField]private UserInfo userInfo = null;
    [SerializeField] private EnemyInfo[] enemyInfos = null;
    [SerializeField] private StageInfo[] stageInfo = null;
    [SerializeField] private InSelectBtn inSelectBtn = null;
    public GameObject slotParent = null;
    [SerializeField] private InventorySlot[] inventorySlots = null;

    public int stageNumber = 0;

    private int itmp = 0;
    private float ftmp = 0;
    public OperatorInfo[] OperatorInfos { get { return operatorInfos; } set { operatorInfos = value; } }

    public EnemyInfo[] EnemyInfos { get => enemyInfos; set => enemyInfos = value; }
    public StageInfo[] StageInfo { get => stageInfo; set => stageInfo = value; }
    public UserInfo UserInfo { get => userInfo; set => userInfo = value; }
    public Item[] Items { get => items; set => items = value; }

    //Todo : 나중에 데이터 베이스에서 데이터 받아올수있도록 할 예정

    private void Awake()
    {
        instance = this;
        operatorInfos = Resources.LoadAll<OperatorInfo>("Data/OperatorStat");
        //enemyInfos = Resources.LoadAll<EnemyInfo>("Data/EnemyStat");
        StageInfo = Resources.LoadAll<StageInfo>("Data/StageStat");
        Items = Resources.LoadAll<Item>("Data/Operator");

        if (UserInfo==null)
        {
            UserInfo = Resources.Load<UserInfo>("Data/UserStat/cyc");
        }

        inventorySlots = slotParent.GetComponentsInChildren<InventorySlot>();

        if(userInfo.organization.Count == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                Organization organization = new Organization();
                userInfo.organization.Add(organization);
            }
        }
        LoadFromJSON();
    }

    private void Start()
    {
        StartCoroutine(InitOrganization());

        LoadSquadData(userInfo.organization[0], Inventory.instance.squad1OperatorInfo, Inventory.instance.squad1Item);
        LoadSquadData(userInfo.organization[1], Inventory.instance.squad2OperatorInfo, Inventory.instance.squad2Item);
        LoadSquadData(userInfo.organization[2], Inventory.instance.squad3OperatorInfo, Inventory.instance.squad3Item);
        LoadSquadData(userInfo.organization[3], Inventory.instance.squad4OperatorInfo, Inventory.instance.squad4Item);        
    }

    IEnumerator InitOrganization()
    {
        yield return new WaitForSeconds(1.0f);
        if(!userInfo.chk)
        {
            foreach (Organization item in userInfo.organization)
            {
                for (int i = 0; i < 12; i++)
                {
                    SqudSloat squdSloat = new SqudSloat();
                    item.squadSloat.Add(squdSloat);
                }
            }
        }
        userInfo.chk = true;
        inSelectBtn.SettingSloat();
    }
    /// <summary>
    /// 데이터 넣는 함수 
    /// 리소스 로드를 줄이기위해 넘버링된 인덱스번호대로 순차적으로 데이터 집어 넣음
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    OperatorInfo InputInfo(OperatorInfo info)
    {

        return info;
    }
    /// <summary>
    ///
    /// </summary>
    /// 
    OperatorInfo OutputData(OperatorInfo operatorInfo)
    {
        return operatorInfo;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public OperatorInfo OutputData(int index)
    {
        return operatorInfos[index];
    }
    public void _StageChange(int num)
    {
        Inventory.instance.stageInfo = stageInfo[num];
    }

    public void _SaveDataUserInfo()
    {
        string json = JsonUtility.ToJson(userInfo);
        File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "AvatarData.txt", json);
        
        Debug.Log("파일저장 경로");
        Debug.Log(Application.persistentDataPath + Path.DirectorySeparatorChar + "AvatarData.txt");

        string json1 = JsonUtility.ToJson(userInfo);
        File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "AvatarData.txt", json);
    }    

    public void LoadFromJSON()
    {
        JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "AvatarData.txt"), userInfo);
        userInfo.hideFlags = HideFlags.HideAndDontSave;
    }
    public void LoadSquadData(Organization loadSquadData, List<OperatorInfo> sloatOperatorInfo,List<Item> sloatOperatorItem)
    {
        for (int j = 0; j < loadSquadData.squadSloat.Count; ++j)//12개
        {
            for (int k = 0; k < operatorInfos.Length; ++k)
            {
                if (operatorInfos[k].NameNumber == loadSquadData.squadSloat[j].operatorIndex)
                {
                    sloatOperatorInfo.Add(operatorInfos[k]);
                    sloatOperatorItem.Add(Items[k]);
                }
            }
        }
    }
}
