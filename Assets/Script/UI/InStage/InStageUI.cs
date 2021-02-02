using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InStageUI : MonoBehaviour
{

    public static InStageUI instance = null;

    [SerializeField]private InStageToggleGroup inStageToggleGroup = null;
    [SerializeField] private StatusPanel statusPanel =null;
    [SerializeField] private StagePanel stagePanel = null;

    
    [SerializeField] private Transform cursor; // 캐릭터 게임 오브젝트
    [SerializeField] private Operator targetOperator = null;        // 캐릭터 좌표 
    [SerializeField] private float mouseZ =0;

    //건설모드 
    [SerializeField] private bool isFactoryMode = false;
    [SerializeField] private bool isTileMatch = false;

    [SerializeField] private int toggleNumber = 0;

    [SerializeField] OperatorToggle nowToggle = null;
    public Operator TargetOperator { get => targetOperator; set => targetOperator = value; }
    public bool IsTileMatch { get => isTileMatch; set => isTileMatch = value; }
    public bool IsFactoryMode { get => isFactoryMode; set => isFactoryMode = value; }
    public OperatorToggle NowToggle { get => nowToggle; set => nowToggle = value; }
    private Button toggleButton = null;
    public int ToggleNumber { get => toggleNumber; set => toggleNumber = value; }
    public InStageToggleGroup InStageToggleGroup { get => inStageToggleGroup; set => inStageToggleGroup = value; }

    public delegate void delegates(BaseEventData data);
    public delegate void delegatesToggle(BaseEventData data , OperatorToggle toggle);

    private void Awake()
    {
        instance = this;
        InStageToggleGroup = FindObjectOfType<InStageToggleGroup>();
        statusPanel = FindObjectOfType<StatusPanel>();
        toggleButton = GameObject.Find("ToggleButton").GetComponent<Button>();
        stagePanel = FindObjectOfType<StagePanel>();

        toggleButton.gameObject.SetActive(false);
    }
    private void Start()
    {
        
        mouseZ = 6f;
    }
    private void Update()
    {
        ActivateStartPanel();
        if (nowToggle && !stagePanel.IsPause)
        {
            Time.timeScale = 0.1f;
        }
        else if(!nowToggle && !stagePanel.IsPause && stagePanel.isEnd != false)
        {
            Time.timeScale = Stage.instance.NowTime;
        }
        if(!isFactoryMode)
        {
            Camera.main.orthographic = false;
            Quaternion rotation = Quaternion.Euler(75, 0, 0);

            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, rotation, Time.deltaTime * 10);
        }
        else
        {
            Quaternion rotation = Quaternion.Euler(90, 0, 0);
            
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, rotation, Time.deltaTime*50);
            if (Camera.main.transform.eulerAngles.x >= 80)
            {
                Camera.main.orthographic = true;
            }
        }
    }


    /// <summary>
    /// 캐릭터 생성하는 함수
    /// 델리 게이트로 만들어 놓음 참조는 오픈토글 
    /// </summary>
    public void OnPointClick(BaseEventData data, OperatorToggle toggle)
    {
        if (Stage.instance.Cost > toggle.operatorInfo.Cost)
        { 
            isFactoryMode = true;
        
            nowToggle = toggle;
            toggle.isChoice = true;
            ToggleNumber = toggle.Count;
        
            targetOperator = SpawnManager.spawnManager.FindInputOperator(toggle.operatorInfo);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void DraggingMove(BaseEventData data , OperatorToggle toggle)
    {
        if (Stage.instance.Cost > toggle.operatorInfo.Cost)
        {
            if (isFactoryMode == false)
            {
                isFactoryMode = true;
                nowToggle = null;
            }
            toggle.isChoice = true;
            nowToggle = toggle;
            targetOperator = SpawnManager.spawnManager.FindInputOperator(toggle.operatorInfo);
            targetOperator.UVManager.ChangeMotion(UVManager.eMotoin.idle);
            MouseMove();
        }
        //toggle.InputToggle();
    }

    public void OnPointerUp(BaseEventData data, OperatorToggle toggle)//마우스 버튼을 땟을때, 불러오는 정보들
    {
        
            //팩토리 모드일때
        if (isFactoryMode)
        {
            //타켓이 있으면
            if (isTileMatch)
            {
                targetOperator.Azimuth.transform.parent.gameObject.SetActive(true);
                targetOperator.Azimuth.transform.parent.position = targetOperator.transform.position
                    + Vector3.up * 2f;
                targetOperator.Azimuth.transform.position = targetOperator.transform.position
                    + Vector3.up * 2.5f;
                targetOperator.Azimuth.OriginPos = targetOperator.Azimuth.transform.position;

                Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(targetOperator.Azimuth.transform.position);
                Vector2 WorldObject_ScreenPosition = new Vector2(
                ((ViewportPosition.x * SpawnManager.spawnManager.CanvasRect.sizeDelta.x) - (SpawnManager.spawnManager.CanvasRect.sizeDelta.x * 0.5f)),
                ((ViewportPosition.y * SpawnManager.spawnManager.CanvasRect.sizeDelta.y) - (SpawnManager.spawnManager.CanvasRect.sizeDelta.y * 0.5f)));
                SpawnManager.spawnManager.cutoutmask.gameObject.SetActive(true);
                SpawnManager.spawnManager.cutoutmask.rectTransform.anchoredPosition = WorldObject_ScreenPosition;
            }
            else
            {
                targetOperator.transform.position = new Vector3(1000, 1000, 1000);
                
                isFactoryMode = false;
                toggle.isChoice = false;
                nowToggle = null;

                Time.timeScale = Stage.instance.NowTime;
            }
            
        }
    }

    /// <summary>
    /// 마우스 무브 
    /// </summary>
    void MouseMove()
    {
        Vector3 mousePosition = 
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, mouseZ);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        

        cursor.transform.position = new Vector3(objPosition.x,1, objPosition.z);
        

        RaycastHit hit;
        Debug.DrawRay(cursor.position, -cursor.up, Color.red,1.0f);
        if (Physics.Raycast(cursor.position, -cursor.up, out hit))
        {
            if (hit.transform.tag == "PositionHolder")
            {
                Debug.Log("레이감지중" + hit.transform.parent.name);
                targetOperator.TileRange.gameObject.SetActive(true);
                //드래그 중인 물체의 포지션을 부딪힌 물체의 위치에 (0,1,0)만큼 더해준다.

                targetOperator.transform.position = 
                    new Vector3(
                        hit.transform.parent.position.x
                    , hit.transform.parent.position.y+0.5f
                    , hit.transform.parent.position.z
                    );

                isTileMatch = true;

            }
            else
            {
                targetOperator.TileRange.gameObject.SetActive(false);
                Debug.Log("레이감지 안함");
                targetOperator.transform.position = cursor.transform.position;
                isTileMatch = false;
            }
        }
    }
    ////Todo : 생성부 바꿀필요 있음
    ////바꿔야되는 부분 오브젝트 풀링에서 빼내올수있도록 만들어줘야됨
    ///// <summary>
    ///// 방위 잡아주는 마름보 생성하는 함수
    ///// </summary>
   

    /// <summary>
    /// 스텟 패널을 키고 끄는 함수
    /// </summary>
    private void ActivateStartPanel()
    {
        if(isFactoryMode)
        {
            if (statusPanel.gameObject.activeSelf == false)
            {
                statusPanel.gameObject.SetActive(true);
                toggleButton.gameObject.SetActive(true);
                statusPanel.InputData(nowToggle.operatorInfo);
            }   
        }
        else
        {
            if(statusPanel.gameObject.activeSelf == true)
            {
                statusPanel.gameObject.SetActive(false);
            }
            if(toggleButton.gameObject.activeSelf == true)
            {
                toggleButton.gameObject.SetActive(false);
            }
        }

    }

    public void ToggleClear()
    {
        toggleNumber = 0;
        IsFactoryMode = false;
        
        if(IsFactoryMode == false)
        {
            nowToggle = null;
        }
        Debug.Log("클릭됨");
        Time.timeScale = Stage.instance.NowTime;

    }



}
