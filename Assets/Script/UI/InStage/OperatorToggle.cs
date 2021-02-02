using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OperatorToggle : MonoBehaviour
{
    [SerializeField]private int count =0;
    
    public int Count { get { return count; } set { count = value; } }
    public bool isChoice = false;

    /*
     * 1.캐릭터 이미지 
     * 2.죽엇을때 나오는 빨간 이미지 
     * 3.클래스(직업) 이미지
     * 4.로딩바 금형
     * 5.로딩바 움직이는거
     * 6.코스트 텍스트
     * 7.대기 지연시간 텍스트
     */

    //런타임중 데이터를 받아와야되는것들
    [SerializeField] private InStageToggleGroup group = null;
    [SerializeField] private Toggle toggle = null;
    [SerializeField] private Image bgi = null; //캐릭터 이미지
    [SerializeField] private Image dieImage = null; //죽엇을 때 나오는 빨간 이미지
    [SerializeField] private Image loadingState = null; // 대기시간 로딩바 움직이는거 
    [SerializeField] private Image classImage = null; //클래스(직업) 이미지
    [SerializeField] private Text costText = null; // 코스트 숫자 텍스트 
    [SerializeField] private Text delayTimeText = null; // 지연시간 텍스트  
    [SerializeField] private Image possibility = null;
    [SerializeField] private OperatorToggle[] toggles = null;
    [SerializeField] private bool isDelayChk = false;
    

    [SerializeField] private float loadingDtMax = 0; //로딩시간 소숫점 변수
    [SerializeField] private float loadingDtNow = 0; //로딩시간 소숫점 변수

    //전체 받아는 변수 4/6빼고 다받아오면된다.
    [SerializeField] private Image[] images = null; 
    [SerializeField] private Text[] texts = null;

    public Toggle Toggle { get { return toggle; } set { toggle = value; } }
    public Image Bgi { get { return bgi; } set { bgi = value; } }
    public Image ClassImage { get { return classImage; } set { classImage = value; } }
    public float DelayTime { set { loadingDtMax = value; } }

    public Image DieImage { get => dieImage; set => dieImage = value; }
    public bool IsDelayChk { get => isDelayChk; set => isDelayChk = value; }
    public float LoadingDtNow { get => loadingDtNow; set => loadingDtNow = value; }
    public Text CostText { get => costText; set => costText = value; }

    /*
     * 
     * Down is Triger Event Data 
     *
     */
    [SerializeField] private bool ischk;

    [SerializeField]EventTrigger eventTrigger = null;

    //Todo : 나중에 검토이후 실재로 사용되지 않을경우 그냥 프리팹화 시키고 변수는 뺄예정
    //데이터를 안받아와도 되는것들 이미지가 고정됨
    [SerializeField] private Image loadingMold = null; //대기시간 로딩바 금형 이미지 
    public OperatorInfo operatorInfo = null;

    public void Start()
    {
        //inStageUI = FindObjectOfType<InStageUI>();
        eventTrigger = this.GetComponent<EventTrigger>();
        
        SettingTriggerEvent
        (EventTriggerType.PointerClick, new InStageUI.delegatesToggle(InStageUI.instance.OnPointClick));

        SettingTriggerEvent
        (EventTriggerType.Drag, new InStageUI.delegatesToggle(InStageUI.instance.DraggingMove));

        SettingTriggerEvent
        (EventTriggerType.PointerUp,new InStageUI.delegatesToggle(InStageUI.instance.OnPointerUp));

        IsDelayChk = true;
        LoadingDtNow = loadingDtMax;
        toggles = this.GetComponentsInParent<OperatorToggle>();

        
    }

    /*
     예시 : onPointerClick.eventID = EventTriggerType.PointerClick;
           onPointerClick.callback.AddListener((data) =>
           { inStageUI.CreateCharacter((PointerEventData)data); });
           eventTrigger.triggers.Add(onPointerClick);
     */
    /// <summary>
    /// 이벤트 트리거에 
    /// </summary>
    /// <param name="eventTriggers"></param>
    /// <param name="funtion"></param>
    public void SettingTriggerEvent(EventTriggerType eventTriggers, InStageUI.delegates funtion)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry(); //
        InStageUI.delegates refer = funtion; //델리게이트 함수를 지역변수로 생성해서 전달해줌

        entry.eventID = eventTriggers; //이벤트 타입 설정(열거형)

        //람다식으로 함수를 추가하는데 델리게이트로 매개변수 받아와서 사용
        entry.callback.AddListener((data) =>
        { refer(data); });

        eventTrigger.triggers.Add(entry);
    }
    public void SettingTriggerEvent(EventTriggerType eventTriggers, InStageUI.delegatesToggle funtion)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry(); //
        InStageUI.delegatesToggle refer = funtion; //델리게이트 함수를 지역변수로 생성해서 전달해줌
        entry.eventID = eventTriggers; //이벤트 타입 설정(열거형)

        //람다식으로 함수를 추가하는데 델리게이트로 매개변수 받아와서 사용
        entry.callback.AddListener((data) =>
        { refer(data,this); });

        eventTrigger.triggers.Add(entry);
    }
    void Update()
    {
        FillDecrease(loadingState, delayTimeText);
        
        if (InStageUI.instance.ToggleNumber == count)
        {
            if (bgi.transform.parent.transform.localPosition.y <= 30)
            {
                bgi.transform.parent.transform.localPosition +=
                    Vector3.up * Time.deltaTime * 150f;
            }
        }
        else
        {
            if (bgi.transform.parent.transform.localPosition.y >= 0)
            {
                bgi.transform.parent.transform.localPosition -=
                    Vector3.up * Time.deltaTime * 150f;
            }
        }

        if(possibility)
        {
            if (Stage.instance.Cost >= operatorInfo.Cost)
            {
                possibility.gameObject.SetActive(false);
            }
            else
            {
                possibility.gameObject.SetActive(true);
            }
        }
        
    }
    /// <summary>
    /// 이미지 넣는 함수
    /// </summary>
    /// <param name="image">받아올 이미지</param>
    /// <param name="index">인덱스넘버</param>
    /// <returns></returns>
    //이미지 넣기
    public Image InputImge(Image image,int index)
    {
        if(image)
        {
            return image;
        }
        else
        {
            return images[index];
        }
    }
    /// <summary>
    /// 텍스트 넣는 함수
    /// </summary>
    /// <param name="text">받아올 텍스트</param>
    /// <param name="index">인덱스 넘버</param>
    /// <returns></returns>
    private Text InputText(Text text , int index)
    {
        if (text)
        {
            return text;
        }
        else
        {
            return texts[index];
        }
    }
    ///<summary>
    ///토글 이벤트 발생시
    ///</summary>
    public void InputToggle()
    {
        //if(operatorInfo != null) //예외처리
        //{
            if (LoadingDtNow <= 0)
            {
                InStageUI.instance.NowToggle = this;
                
            }
        //}
        
    }
    public void SettingsData()
    {
        //부모정보를 받아옴
        group = this.transform.parent.GetComponent<InStageToggleGroup>();
        //토글들의 자식들중 전체 이미지와 텍스트 받아옴
        toggle = this.GetComponent<Toggle>();
        images = this.GetComponentsInChildren<Image>();
        texts = this.GetComponentsInChildren<Text>();
        //이미지 위치 찾아줌
        for (int i = 0; i < images.Length; i++)
        {
            switch (i)
            {
                case 0:
                    bgi = InputImge(bgi, i);
                    break;
                case 1:
                    DieImage = InputImge(DieImage, i);
                    break;
                case 2:
                    loadingMold = InputImge(loadingMold, i);
                    break;
                case 3:
                    loadingState = InputImge(loadingState, i);
                    break;
                case 4:
                    //4번은 넣지않는다.
                    break;
                case 5:
                    classImage = InputImge(classImage, i);
                    break;
                case 6:
                    break;
                //6번도 넣지않는다.
                case 7:
                    possibility = InputImge(possibility, i);
                    break;
            }
        }
        //텍스트 배열로 위치 찾아줌
        for (int i = 0; i < texts.Length; i++)
        {
            switch (i)
            {
                case 0:
                    delayTimeText = InputText(delayTimeText, i);
                    break;
                case 1:
                    CostText = InputText(CostText, i);
                    
                    break;
            }
        }
    }
    /// <summary>
    /// 필 감소하는 함수
    /// 재배치 대기시간에 사용 용도
    /// </summary>
    private void FillDecrease(Image img ,Text text)
    {
        if(IsDelayChk == true)
        {
            if (LoadingDtNow >= 0)
            {
                LoadingDtNow -= Time.deltaTime * 1f;
            }
            //대기시간 다되었을 때 
            else
            {
                //loadingDtNow = loadingDtMax;
                if (img.transform.parent)
                {
                    img.transform.parent.gameObject.SetActive(false); //자기 자신의 부모를 꺼준다
                }
                else
                {
                    img.gameObject.SetActive(false);//부모가 없을땐 자기 자신을 꺼준다.
                }
                IsDelayChk = false;
            }
            text.text = "" + string.Format("{0:0.0}", LoadingDtNow);
            img.fillAmount -= Time.deltaTime *(1 / loadingDtMax);
        }
    }

    void FillAmount(Image img)
    {
        if (img.fillAmount <= 0.0f)
        {
            ischk = false;
            //image.fillClockwise = true;
        }
        else if (img.fillAmount >= 1.0f)
        {
            ischk = true;
            //image.fillClockwise = false;
        }
        if (ischk == false)
        {
            img.fillAmount += Time.deltaTime * 0.3f;
        }
        else if (ischk == true)
        {
            //img.fillAmount = 0;
            img.fillAmount -= Time.deltaTime * 0.3f;
        }

    }
    void FillAmount_01(Image Tepimg)
    {
        if (Tepimg.fillClockwise == true)
        {
            Tepimg.fillAmount += Time.deltaTime * 1f;
            //tx.transform.rotation = Quaternion.Euler(0, 0, Tepimg.fillAmount * -360);
            //tx.transform.localScale = new Vector3(tx.transform.localScale.x - 0.1f, tx.transform.localScale.y - 0.1f, tx.transform.localScale.z - 0.1f);
        }
        else if (Tepimg.fillClockwise == false)
        {
            Tepimg.fillAmount -= Time.deltaTime * 1f;
            //tx.transform.rotation = Quaternion.Euler(0, 0, Tepimg.fillAmount * 360);
            //tx.transform.localScale =new Vector3(tx.transform.localScale.x+0.1f, tx.transform.localScale.y+0.1f, tx.transform.localScale.z+0.1f);
        }
        if (Tepimg.fillAmount >= 1.0f)
        {
            Tepimg.fillClockwise = false;
        }
        else if (Tepimg.fillAmount <= 0.0f)
        {
            Tepimg.fillClockwise = true;
        }

    }
}
