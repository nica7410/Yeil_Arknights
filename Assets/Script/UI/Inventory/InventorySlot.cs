using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image CharacterImage;
    public Text nameText;
    public Text levelText;
    //작업 우선 순위에 따라 정렬 함.

    public Image skillIconImage;

    public Image expImage;
    public Image starImage;
    public int starNumber;
    public Image starColorGradiantImage;
    public Image classImage;
    public Image eliteImage;
    public Image potenialImage;

    public GameObject inclineBaseGray;
    public GameObject inclineBaseBlack;
    public GameObject expBaseRing;
    public GameObject LV;
    private Item item = null;
    public Item Item { get { return item; } set { item = value; } }

    [SerializeField]private OperatorInfo operatorInfo;
    public OperatorInfo OperatorInfo { get { return operatorInfo; } set { operatorInfo = value; } }

    private Vector3 starWidth;
    public void AddItem(Item newItem)
    {
        Item = newItem;
        starWidth = new Vector3(starImage.rectTransform.rect.width, 0, 0);
        AddItemData();

        for (int i = 1; i < starNumber; ++i)
        {
            Instantiate(starImage, starImage.transform.position + starWidth * i * 0.5f, Quaternion.identity,this.transform);
        }
    }
    
    private void AddItemData()
    {
        CharacterImage.sprite = item.CharacterImage;
        nameText.text = item.nameText;
        levelText.text = item.levelText;
        skillIconImage.sprite = item.skillIconImage;
        expImage.sprite = item.expImage;
        starImage.sprite = item.starImage;
        starNumber = item.starNumber;
        starColorGradiantImage.sprite = item.starColorGradiantImage;
        classImage.sprite = item.classImage;
        eliteImage.sprite = item.eliteImage;
        potenialImage.sprite = item.potenialImage;
    }
    public void ChangeItemData(Item _item)
    {
        Item = _item;
        CharacterImage.sprite = _item.CharacterImage;
        nameText.text = _item.nameText;
        levelText.text = _item.levelText;
        skillIconImage.sprite = _item.skillIconImage;
        expImage.sprite = _item.expImage;
        starImage.sprite = _item.starImage;
        starNumber = _item.starNumber;
        starColorGradiantImage.sprite = _item.starColorGradiantImage;
        classImage.sprite = _item.classImage;
        eliteImage.sprite = _item.eliteImage;
        potenialImage.sprite = _item.potenialImage;
        setActiveAddedSquadSlotItem();
    }
    public void DeleteItemData()
    {
        Item = null;
        CharacterImage.sprite = null;
        nameText.text = null;
        levelText.text = null;
        skillIconImage.sprite = null;
        expImage.sprite = null;
        starImage.sprite = null;
        starNumber = 0;
        starColorGradiantImage.sprite = null;
        classImage.sprite = null;
        eliteImage.sprite = null;
        potenialImage.sprite = null;
        setDeActiveAddedSquadSlotItem();
    }
    private void setActiveAddedSquadSlotItem()
    {
        CharacterImage.gameObject.SetActive(true);
        nameText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);
        skillIconImage.gameObject.SetActive(true);
        expImage.gameObject.SetActive(true);
        starImage.gameObject.SetActive(true);
        starColorGradiantImage.gameObject.SetActive(true);
        classImage.gameObject.SetActive(true);
        inclineBaseGray.gameObject.SetActive(true);
        inclineBaseBlack.gameObject.SetActive(true);
        expBaseRing.gameObject.SetActive(true);
        LV.gameObject.SetActive(true);
    }
    private void setDeActiveAddedSquadSlotItem()
    {
        CharacterImage.gameObject.SetActive(false);
        nameText.gameObject.SetActive(false);
        levelText.gameObject.SetActive(false);
        skillIconImage.gameObject.SetActive(false);
        expImage.gameObject.SetActive(false);
        starImage.gameObject.SetActive(false);
        starColorGradiantImage.gameObject.SetActive(false);
        classImage.gameObject.SetActive(false);
        inclineBaseGray.gameObject.SetActive(false);
        inclineBaseBlack.gameObject.SetActive(false);
        expBaseRing.gameObject.SetActive(false);
        LV.gameObject.SetActive(false);
    }
    public void AddOperatorInfo(OperatorInfo _operatorInfo)
    {
        OperatorInfo = _operatorInfo;
    }
}
