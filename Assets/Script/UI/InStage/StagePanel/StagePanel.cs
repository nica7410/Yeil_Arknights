using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StagePanel : MonoBehaviour
{
    //인게임 내 관련
    [SerializeField] private Image pause = null;
    [SerializeField] private Image exitLayer = null;
    [SerializeField] private Text speedText = null;
    [SerializeField] private Text puasetext = null;
    [SerializeField] private Text deploymentText = null;


    //게임 종료관련
    [SerializeField] private Button gameEndLayer = null;
    [SerializeField] private Button gameDataSet = null;
    [SerializeField] private TerminationUI terminationUI = null;



    delegate void FunctionCall(bool chk);
    [SerializeField]private bool isPause = false;
    public bool isEnd = false;

    public bool IsPause { get => isPause; set => isPause = value; }

    //일시정지  ' 버튼 리스너
    //배속  ' 버튼리스너 + 스테이지 데이터
    //나가기 ' 패널 레이어
    //코스트 타이머 바 '코스트 타이머 셋 역활 
    //배치 가능인원 ' 스테이지 겟 셋


    private void Awake()
    {
        pause = GameObject.Find("PAUSE").GetComponent<Image>();
        exitLayer = GameObject.Find("ExitLayer").GetComponent<Image>();
        puasetext = GameObject.Find("PauseText").GetComponent<Text>();
        speedText = GameObject.Find("SpeedText").GetComponent<Text>();
        deploymentText = GameObject.Find("DeploymentText").GetComponent<Text>();


        gameEndLayer = GameObject.Find("GameEndLayer").GetComponent<Button>();
        gameDataSet = GameObject.Find("GameDataSetLayer").GetComponent<Button>();
        terminationUI = GameObject.Find("GameEndLayer").GetComponent<TerminationUI>();

        //시작과 동시에 엑티브 꺼놓음
        pause.gameObject.SetActive(false);
        exitLayer.gameObject.SetActive(false);

    
        gameDataSet.gameObject.SetActive(false);
    }
    private void Update()
    {
        deploymentText.text = "배치 가능 인원: "+Stage.instance.Deployment;

        //라이프가 다까질경우 게임 종료 전광판 띄움
        if(Stage.instance.Life <=0)
        {

            StartCoroutine(delayFuntioCall(1.5f, false, GameEnd));
            
            Stage.instance.Life = 1000; //로직을 끊기위해 값 100으로 줫음
        }
        else if(Stage.instance.NowEntry == Stage.instance.TotalEntry 
            && Stage.instance.Life > 0
            && Stage.instance.Life <100)
        {
            StartCoroutine(delayFuntioCall(3f, true, GameEnd));
            
            Stage.instance.NowEntry = 0;
            isEnd = true;

        }
    }
    /// <summary>
    /// 타임스케일 속도값 조절
    /// </summary>
    public void SpeedBtnClick()
    {
        if(!isPause)
        {
            if (Stage.instance.NowTime == 1)
            {
                Stage.instance.NowTime = 2;
            }
            else
            {
                Stage.instance.NowTime = 1;
            }
            Time.timeScale = Stage.instance.NowTime;
            speedText.text = Stage.instance.NowTime + "X\n▶";
        }
        
        
    }

    /// <summary>
    /// 일시정지 버튼
    /// </summary>
    public void PauseBtnClick()
    {
        //켜져있을때
        if(IsPause)
        {
            IsPause = false;
            Time.timeScale = Stage.instance.NowTime; //일시정지 꺼둠
            puasetext.text = "▶";
        }
        else //꺼져있을때
        {
            IsPause = true;
            Time.timeScale = 0; //일시정지 켜둠
            puasetext.text = "∥";
        }
        pause.gameObject.SetActive(IsPause);
    }

    /// <summary>
    /// 되겠지?
    /// </summary>
    public void ExitBtnClick()
    {


        IsPause = true;
        Time.timeScale = 0; //일시정지 켜둠
        
        exitLayer.gameObject.SetActive(IsPause);
        exitLayer.transform.parent = this.transform.parent;
    }

    /// <summary>
    /// ExitLayer 작전포기버튼
    /// </summary>
    public void GivingUp()
    {
        Time.timeScale = Stage.instance.NowTime;
        SceneManager.LoadScene("MenuScene");
    }
    /// <summary>
    /// EXitLayer의 돌아가기버튼
    /// </summary>
    public void ReStart()
    {
        Time.timeScale = Stage.instance.NowTime;
        exitLayer.transform.parent = this.transform;
        exitLayer.gameObject.SetActive(false);
        PauseBtnClick();
    }

    public void GameEnd(bool chk)
    {
        Time.timeScale = 0;

        terminationUI.MissonFailed(chk);
        gameEndLayer.gameObject.SetActive(true);
    }

    public void GameLayerClick()
    {
        gameDataSet.gameObject.SetActive(true);
        gameEndLayer.gameObject.SetActive(false);
    }

    public void GameDataClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }

    /// <summary>
    /// 델리게이트로 매개변수 bool값타입을 불러오는 스타트 코루틴 
    /// </summary>
    /// <param name="dt">지연시간</param>
    /// <param name="chk">전달해줄 bool 값</param>
    /// <param name="funtionCall">불러올 함수</param>
    /// <returns></returns>
    IEnumerator delayFuntioCall(float dt ,bool chk , FunctionCall funtionCall)
    {
        yield return new WaitForSeconds (dt);
        funtionCall(chk);

    }


}
