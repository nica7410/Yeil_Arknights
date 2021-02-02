using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDirection : MonoBehaviour
{
    private Vector3 originPos;  //생성시 위치
    private Vector3 mOffset;
    private float mZCoord;      //z축 값

    //private TileManager tileM;

    [SerializeField]private Transform character;
    private GameObject thisParent;

    public GameObject goAtkRangeTile;   //공격 범위 타일
    [SerializeField]private Operator characterClass = null;

    public Vector3 OriginPos { get => originPos; set => originPos = value; }

    private void Awake()
    {
        //OriginPos = this.transform.position;
        character = this.transform.parent.parent;
        characterClass = this.transform.parent.parent.GetComponent<Operator>();
        thisParent = this.transform.parent.gameObject;
    }
    private void Start()
    {
        //goAtkRangeTile.SetActive(false);
        //tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();

        
    }

    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(transform.position).z;  //해당 물체의 좌표를 스크린 좌표로
        mOffset = transform.position - GetMouseWorldPos();   //물체의 현재 위치에서 마우스 월드 좌표를 뺀 값
    }
    /// <summary>
    /// 드래그 땟을때
    /// </summary>
    private void OnMouseUp()
    {
        if (transform.position.x > OriginPos.x + 1f) //오른쪽
        {
            goAtkRangeTile.transform.rotation = Quaternion.Euler(0, 90, 0);
            Debug.Log("오른쪽업");
            QuadRotation(true);
        }
        else if (transform.position.x < OriginPos.x - 1f) //왼쪽
        {
            goAtkRangeTile.transform.rotation = Quaternion.Euler(0, 270, 0);
            Debug.Log("왼쪽업");
            QuadRotation(false);
        }
        else if (transform.position.z > OriginPos.z + 1f) //위쪽
        {
            goAtkRangeTile.transform.rotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("위쪽업");
            QuadRotation(true);
        }
        else if (transform.position.z < OriginPos.z - 1f) //아래쪽
        {
            goAtkRangeTile.transform.rotation = Quaternion.Euler(0, 180, 0);
            Debug.Log("아래쪽업");
            QuadRotation(true);
        }
        this.transform.position = OriginPos;

        InStageUI.instance.ToggleClear();

        thisParent.gameObject.SetActive(false);
        SpawnManager.spawnManager.GetPoolOperator(characterClass.OperatorInfo);

        SpawnManager.spawnManager.cutoutmask.gameObject.SetActive(false);
        Time.timeScale = Stage.instance.NowTime;
    }
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;   //마우스 좌표 입력

        mousePoint.z = mZCoord; //마우스 좌표의 z축에 해당 물체의 스크린 좌표를 넣는다

        return Camera.main.ScreenToWorldPoint(mousePoint);  //바뀐 마우스 좌표를 월드 좌표로 바꿔서 돌려준다.
    }
    /// <summary>
    /// 드래그도중
    /// </summary>
    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + mOffset;  // 물체의 현재위치를 마우스의 월드 좌표와 물체위치에 마우스 월드 좌표를 뺀 값을 더한 값을 넣는다.

        if (transform.position.x > OriginPos.x + 1f) //오른쪽
        {
            goAtkRangeTile.transform.rotation = Quaternion.Euler(0, 90, 0);
            QuadRotation(true);
            goAtkRangeTile.SetActive(true);
            Debug.Log("오른쪽 공격범위 활성");
        }
        else if (transform.position.x < OriginPos.x - 1f) //왼쪽
        {
            goAtkRangeTile.transform.rotation = Quaternion.Euler(0, 270, 0);
            QuadRotation(false);
            goAtkRangeTile.SetActive(true);
            Debug.Log("왼쪽 공격범위 활성");
        }
        else if (transform.position.z > OriginPos.z + 1f) //위쪽
        {
            goAtkRangeTile.transform.rotation = Quaternion.Euler(0, 0, 0);
            QuadRotation(false);
            goAtkRangeTile.SetActive(true);
            Debug.Log("위쪽 공격범위 활성");
        }
        else if (transform.position.z < OriginPos.z - 1f) //아래쪽
        {
            goAtkRangeTile.transform.rotation = Quaternion.Euler(0, 180, 0);
            QuadRotation(false);
            goAtkRangeTile.SetActive(true);
            Debug.Log("아래쪽 공격범위 활성");
        }
        else
        {
            goAtkRangeTile.SetActive(false);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="temp"></param>
    void DirectionEnable(Transform temp)
    {
        thisParent.transform.position = temp.transform.position;
        this.transform.position = thisParent.transform.position;
    }

    /// <summary>
    /// 오퍼레이터의 자식객체인 uv애니메이션 쿼드가 별도로 쿼드를 들고있지않아 
    /// 방향에따라 Flip 기능함수를 별도로 만들어서 사용
    /// </summary>
    /// <param name="chk"></param>
    public void QuadRotation(bool chk)
    {
        
        if(chk) //오른쪽일때
        {
            character.GetChild(0).transform.localScale = new Vector3(2.5f, 2.5f, 1);
            character.GetChild(0).transform.localPosition = new Vector3(0.1f, 0f, 0.5f);
        }
        else //왼쪽일때
        {
            character.GetChild(0).transform.localScale = new Vector3(-2.5f, 2.5f, 1);
            character.GetChild(0).transform.localPosition = new Vector3(-0.1f, 0f, 0.5f);
        }
    }
}
