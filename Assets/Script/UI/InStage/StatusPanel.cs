using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatusPanel : MonoBehaviour
{
    
    [SerializeField] private Image characterImage;
    
    //
    [SerializeField] private Image classImage = null;
    [SerializeField] private Text enName = null;
    [SerializeField] private Text krName = null;

    //스탯
    [SerializeField] private Text lv = null;
    [SerializeField] private Text atkText = null;
    [SerializeField] private Text defText = null;
    [SerializeField] private Text resText = null;
    [SerializeField] private Text blockText = null;

    [SerializeField] private Text hpText = null;
    [SerializeField] private Slider hpFill = null;

    [SerializeField] private Transform rangGroup = null;
    [SerializeField] private Image[] ranges = null;
    [SerializeField] private Text tagText = null;

    [SerializeField] private Image skillImge = null;
    [SerializeField] private Text skillName = null;
    [SerializeField] private Image skillStat1Image =null;
    [SerializeField] private Image skillStat2Image =null;
    [SerializeField] private Text skillStat1 = null;
    [SerializeField] private Text skillStat2 = null;
    [SerializeField] private Text skillTag = null;



    private void Awake()
    {
        characterImage = GameObject.Find("OperatorImage").GetComponent<Image>();

        classImage = GameObject.Find("ClassImage").GetComponent<Image>();
        enName = GameObject.Find("ENName").GetComponent<Text>();
        krName = GameObject.Find("KRName").GetComponent<Text>();

        lv = GameObject.Find("LvData").GetComponent<Text>();
        atkText = GameObject.Find("atkData").GetComponent<Text>();
        defText = GameObject.Find("DefData").GetComponent<Text>();
        resText = GameObject.Find("ResData").GetComponent<Text>();
        blockText = GameObject.Find("BlockData").GetComponent<Text>();

        hpText = GameObject.Find("HpText").GetComponent<Text>();
        hpFill = GameObject.Find("HpBar").GetComponent<Slider>();

        rangGroup = GameObject.Find("Range").GetComponent<Transform>();
        ranges = rangGroup.GetComponentsInChildren<Image>();
        tagText = GameObject.Find("TagText").GetComponent<Text>();

        skillImge = GameObject.Find("SkillImage").GetComponent<Image>();
        skillName = GameObject.Find("SkillName").GetComponent<Text>();
        skillStat1Image = GameObject.Find("SkillStat1").GetComponent<Image>();
        skillStat2Image = GameObject.Find("SkillStat2").GetComponent<Image>();
        skillStat1 = GameObject.Find("StatText1").GetComponent<Text>();
        skillStat2 = GameObject.Find("StatText2").GetComponent<Text>();
        skillTag = GameObject.Find("SkillTag").GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InputData(OperatorInfo info)
    {
        //Todo : 메인텍스가 원활히 수정이 되지않아 껏다켜줌
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        characterImage.material.mainTexture = info.FullImage;

        classImage.sprite = info.ClassImage;
        enName.text = info.EgName;
        krName.text = info.KrName;

        lv.text = ""+info.Lv;
        atkText.text = ""+info.Attack;
        defText.text = ""+info.Defence;
        resText.text = ""+ info.Resistance;

        hpText.text = info.MaxHp+" / " +info.NowHp;

        hpFill.maxValue = info.MaxHp ;
        hpFill.value = info.NowHp ;

        rangGroup = null;
        ranges = null;
        tagText.text = info.Traits;

        skillImge = null;
        skillName = null;
        skillStat1Image = null;
        skillStat2Image = null;
        skillStat1 = null;
        skillStat2 = null;
        skillTag = null;
    }
    public void InputInfo(OperatorInfo operatorInfo)
    {
        //characterImage = operatorInfo.
        //classImage
    }
    // Todo : 20-11-27 일단 디폴트 함수부터 쓰되 나중에 변경사항이 있을시에 추가 함수 오버로딩을 통해서 바꿔줄 생각
    /// <summary>
    /// 문자열 색상바꾸는 함수
    /// </summary>
    /// <param name="label">바꿔줄 문자열</param>
    /// <param name="color">바꿀 색상</param>
    /// <returns></returns>
    private string ChangeColorText(string label , string color)
    {
        return "<color#"+color+">"+label+"</color>";
    }
    /// <summary>
    /// 문자열 색상바꾸는 함수 오버로딩 추가기능 : 문자의 위치를 찾아서 바꿔줌 단 중복 문자도 바뀔 우려 있음
    /// </summary>
    /// <param name="label">바꿔줄 문자열</param>
    /// <param name="color">바꿀 색상</param>
    /// <param name="indexof">바꿔줄 문자들</param>
    /// <returns></returns>
    private string ChangeColorText(string label, string color, string indexof)
    {
        return "<color#" + color + ">" + label.IndexOf(indexof) + "</color>";
    }
}
