using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UVClip
{
    //인스펙터 속성창에서 넣어줄것은 최대 갯수 텍스처 나머지는 uv매니저에서 데이터 세팅할때 자동세팅
    //지금 초기값 선언해준게 안들어갓을경우 생성자를 통해 디폴트값 넣어줘야됨

    private string textuerName= null; //Todo : 필요한지 검토해봐야됨
    private int colCnt = 5;
    [SerializeField] private int rowCnt;
    [SerializeField] private int totalCnt = 25;
    private bool isLoop = false;

    private float duration = 0.5f;          //애니메이션 플레이 시간
    private float changeFrameTime = 1.0f;   // 프레임 변경시간 (간격)
    private float elapsedTime = 0.0f;       //누적시간
    [SerializeField]private int curFrame = 0;               // 현재 프레임
    [SerializeField] private Texture2D texAnim = null; //스프라이트 시트 텍스쳐

    public int ColCnt { get { return colCnt; } set { colCnt = value; } }
    public int RowCnt { get { return rowCnt; } set { rowCnt = value; } }
    public int TotalCnt { get { return totalCnt; } set { totalCnt = value; } }
    public bool IsLoop { get { return isLoop; } set { isLoop = value; } }
    public float Duration { get { return duration; } set { duration = value; } }
    public float ChangeFrameTime { get { return changeFrameTime; } set { changeFrameTime = value; } }
    public float ElapsedTime { get { return elapsedTime; } set { elapsedTime = value; } }
    public int CurFrame { get { return curFrame; } set { curFrame = value; } }
    public string TextuerName { get { return textuerName; } set { textuerName = value; }}
    public Texture2D TexAnim { get { return texAnim; } set { texAnim = value; } }
} //class End
