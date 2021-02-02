using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelUpPanel : MonoBehaviour
{
    [SerializeField]private OperatorInfo operatorInfo;
    //private UserInfo userinfo;

    [System.Serializable]
    class TextUIClip
    {
        public Text hpText;
        public Text atkText;
        public Text defText;
        public Text resText;
    }
    private Text levelText;
    private Text expText;

    [SerializeField]private Image image;
    private int sumExp = 0;
    private int sumLv = 0;

    private int itemCount = 0;

    private Transform nowText;
    private Transform NextText;

    private Transform expSelectRing;
    private Transform expSelectedNum;
    private Transform expSelectedBtn;

    [SerializeField] private TextUIClip[] textUIClips = null;

    public OperatorInfo OperatorInfo { get => operatorInfo; set => operatorInfo = value; }

    
    
    private void OnEnable()
    {
        if(!nowText)
            nowText = GameObject.Find("ExpStatTextParent").GetComponent<Transform>();
        if(!NextText)
            NextText = GameObject.Find("ModifiedStatTextParent").GetComponent<Transform>();
        if(!levelText)
            levelText = GameObject.Find("ExpLevelNumber").GetComponent<Text>();
        if(!expText)
            expText = GameObject.Find("expNumber").GetComponent<Text>();
        if(!image)
            image = GameObject.Find("ExpPanel").GetComponent<Image>();
        if(!expSelectRing)
            expSelectRing = GameObject.Find("expSelectRing").GetComponent<Transform>();
        if(!expSelectedNum)
            expSelectedNum = GameObject.Find("expSelectedNum").GetComponent<Transform>();
        if(!expSelectedBtn)
            expSelectedBtn = GameObject.Find("expSelectedBtn").GetComponent<Transform>();

        Setting(0 , nowText);
        Setting(1 , NextText);

        

        StartCoroutine(UISettings());
    }
    private void Update()
    {
        expSelectRing.gameObject.SetActive(ItemIcon());
        expSelectedNum.gameObject.SetActive(ItemIcon());
        expSelectedBtn.gameObject.SetActive(ItemIcon());
    }
    private bool ItemIcon()
    {
        if (itemCount <= 0)
        {
            return false;
        }
        else
        {
            return true; 
        }

    }
   
    private void Setting(int index , Transform parent)
    {
        textUIClips[index].hpText = parent.GetChild(0).GetComponent<Text>();
        textUIClips[index].atkText = parent.GetChild(1).GetComponent<Text>();
        textUIClips[index].defText = parent.GetChild(2).GetComponent<Text>();
        textUIClips[index].resText = parent.GetChild(3).GetComponent<Text>();
    }
    public IEnumerator UISettings()
    {
        yield return new WaitForSeconds(0.01f);
        foreach (UserOperator item in LoadData.instance.UserInfo.userOperator)
        {
            if(item.indexName == OperatorInfo.NameNumber)
            {

                textUIClips[0].hpText.text = "" + StatCalculationNow(OperatorInfo, OperatorInfo.MaxHp);
                textUIClips[0].atkText.text = "" + StatCalculationNow(OperatorInfo, OperatorInfo.Attack);
                textUIClips[0].defText.text = "" + StatCalculationNow(OperatorInfo, OperatorInfo.Defence);
                textUIClips[0].resText.text = "" + StatCalculationNow(OperatorInfo, OperatorInfo.Resistance);
                
                textUIClips[1].hpText.text = "" + StatCalculationSum(OperatorInfo, OperatorInfo.MaxHp);
                textUIClips[1].atkText.text = "" + StatCalculationSum(OperatorInfo, OperatorInfo.Attack);
                textUIClips[1].defText.text = "" + StatCalculationSum(OperatorInfo, OperatorInfo.Defence);
                textUIClips[1].resText.text = "" + StatCalculationSum(OperatorInfo, OperatorInfo.Resistance);
                Debug.Log("오퍼레이터인포레벨 : "+OperatorInfo.Lv+"아이템레벨 : " + item.lv + ", 섬레벨 : " + sumLv);

                levelText.text = "" + (item.lv + sumLv);
                expText.text = "" + sumExp;
            }
        }
        image.sprite = operatorInfo.FullImageSprite;
    }

    public void _ItemSum()
    {
        ++itemCount;
        if(itemCount >0)
        {
            expSelectedNum.GetComponentInChildren<Text>().text = "" + itemCount;
        }
        sumExp += 300;
        sumLv = (sumExp/1000);
        StartCoroutine(UISettings());
    }
    public void _ItemSub()
    {
        --itemCount;
        expSelectedNum.GetComponentInChildren<Text>().text = "" + itemCount;
        sumExp -= 300;
        sumLv = (sumExp / 1000);
        StartCoroutine(UISettings());
    }

    public void _ExitPanel()
    {
        sumExp = 0;
        sumLv = 0;
        itemCount = 0;

        this.gameObject.SetActive(false);


    }
    public void _UsingExpItem()
    {
        foreach (UserOperator item in LoadData.instance.UserInfo.userOperator)
        {
            if(item.indexName == OperatorInfo.NameNumber)
            {
                item.lv = item.lv + sumLv;
                if(sumExp >=1000)
                {
                    item.exp = (sumExp %1000);
                    sumExp = (sumExp % 1000);
                }
                else
                {
                    item.exp = sumExp;
                }
                OperatorInfo.Lv = item.lv;
                sumLv = 0;
                itemCount = 0;
            }
        }
        LoadData.instance._SaveDataUserInfo();
        this.gameObject.SetActive(false);
    }
    public int StatCalculationNow(OperatorInfo info, int data)
    {
        UserOperator userOperator = LoadData.instance.UserInfo.userOperator[info.NameNumber];
        return data + (int)((userOperator.lv) * (data * 0.2f));
    }
    public int StatCalculationSum(OperatorInfo info, int data)
    {
        UserOperator userOperator = LoadData.instance.UserInfo.userOperator[info.NameNumber];
        
        return data + (int)((userOperator.lv+ sumLv) * (data * 0.2f));
        
    }
    public int StatCalculation(OperatorInfo info, float data)
    {
        UserOperator userOperator = LoadData.instance.UserInfo.userOperator[info.NameNumber];
        return (int)(data + ((userOperator.lv + sumLv) * (data * 0.2f)));
    }

    public void UpdateOperatorEditSlot(OperatorEditSlot editSlot)
    {
        editSlot.InputLevelUpData(operatorInfo);
    }
}
