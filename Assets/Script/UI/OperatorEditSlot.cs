using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OperatorEditSlot : MonoBehaviour
{
    [SerializeField] private Image characterImage;

    //
    [SerializeField] private Image classImage = null;
    [SerializeField] private Text enName = null;
    [SerializeField] private Text krName = null;

    //스탯
    [SerializeField] private Text atkText = null;
    [SerializeField] private Text atkSpeedText = null;
    [SerializeField] private Text defText = null;
    [SerializeField] private Text resText = null;
    [SerializeField] private Text hpText = null;
    [SerializeField] private Text costText = null;
    [SerializeField] private Text blockText = null;
    [SerializeField] private Text reDeployTimeText = null;

    [SerializeField] private int starNumber = 0;
    public GameObject starsParent = null;
    [SerializeField] private Image[] stars = null;

    [SerializeField] private Text lv = null;

    [SerializeField] private Image eliteImage = null;
    [SerializeField] private Image potentialImage = null;

    [SerializeField] private Text traitsText = null;
    [SerializeField] private Text talentsTitleText = null;

    [SerializeField] private GameObject rangGroup = null;
    [SerializeField] private Image[] ranges = null;
    [SerializeField] private Text tagText = null;

    [SerializeField] private Image skillImge = null;
    [SerializeField] private Text skillName = null;
    [SerializeField] private Image skillStat1Image = null;
    [SerializeField] private Image skillStat2Image = null;
    [SerializeField] private Text skillStat1 = null;
    [SerializeField] private Text skillStat2 = null;
    [SerializeField] private Text skillTag = null;

    [SerializeField] private Image operatorIconimage = null;
    [SerializeField] private Text useOperatorText = null;

    private Item getItem = null;    //클릭한 버튼의 아이템을 받는다
    private OperatorInfo getOperatorInfo = null;    //클릭한 버튼의 오퍼레이터인포를 받는다

    private void Awake()
    {
        stars = starsParent.GetComponentsInChildren<Image>();
        StarsSetDeActive();
    }
    public void InputAllData(Item item, OperatorInfo info)
    {
        classImage.sprite = info.ClassImage;
        krName.text = info.KrName;  // TODO : 아이템, 오퍼레이터인포 둘다 한글이름을 가지고 있음
        enName.text = info.EgName;

        atkText.text = ""+info.Attack;
        atkSpeedText.text = "" + info.AttackTime;
        defText.text = "" + info.Defence;
        resText.text = "" + info.Resistance;
        hpText.text = "" + info.MaxHp;
        costText.text = "" + info.Cost;
        blockText.text = "" + info.Block;
        reDeployTimeText.text = "" + info.ReDeployTime;
        traitsText.text = info.Traits;
        lv.text = ""+info.Lv;   //TODO : 아이템, 오퍼레이터인포 둘다 레벨을 가지고 있음
        characterImage.sprite = info.FullImageSprite;
        talentsTitleText.text = info.TalentsTitle;
        operatorIconimage.sprite = info.CardImage;
        
        foreach (UserOperator items in LoadData.instance.UserInfo.userOperator)
        {
            if(info.NameNumber == items.indexName)
            {
                useOperatorText.text = "" + items.operatorUse;
            }
        }

        starNumber = item.starNumber;
        for (int i = 0; i < starNumber; ++i)
        {
            stars[i].gameObject.SetActive(true);
        }
        eliteImage.sprite = item.eliteImage;
        potentialImage.sprite = item.potenialImage;

        skillImge.sprite = item.skillIconImage;
        
    }
    public void InputLevelUpData(OperatorInfo info)
    {
        atkText.text = "" + info.Attack;
        atkSpeedText.text = "" + info.AttackTime;
        defText.text = "" + info.Defence;
        resText.text = "" + info.Resistance;
        hpText.text = "" + info.MaxHp;
        costText.text = "" + info.Cost;
        blockText.text = "" + info.Block;
        reDeployTimeText.text = "" + info.ReDeployTime;
        lv.text = "" + info.Lv;   //TODO : 아이템, 오퍼레이터인포 둘다 레벨을 가지고 있음
    }
    public void GetObjData(GameObject obj)
    {
        getItem = obj.GetComponent<InventorySlot>().Item;
        getOperatorInfo = obj.GetComponent<InventorySlot>().OperatorInfo;
        InputAllData(getItem, getOperatorInfo);
    }
    public void StarsSetDeActive()
    {
        for (int i = 0; i < 6; ++i)
        {
            stars[i].gameObject.SetActive(false);
        }
    }
    public void LevelUpData(LevelUpPanel panel)
    {
        panel.gameObject.SetActive(true);
        panel.OperatorInfo = getOperatorInfo;        

    }
}
