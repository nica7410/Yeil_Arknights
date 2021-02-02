using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class UVManager : MonoBehaviour
{
    /// <summary>
    /// 모션
    /// </summary>
    public enum eMotoin
    {
        idle,   //기본 대기상태
        attack, //공격상태
        move,   //이동상태
        die,    //죽었을 때
        skill  // 엑티브 스킬 상태
    }
    private eMotoin motoins;
    

    [SerializeField] private List<UVClip> uVClips = new List<UVClip>();

    [SerializeField] private Unit unit = null;

    public MeshFilter mf = null;
    public MeshRenderer mr = null;
    public Material mat = null;
    public Unit Unit { get { return unit; } set { unit = value; } }

    public eMotoin Motoins { get => motoins; set => motoins = value; }

    public bool isDissolve = false;
    [SerializeField]private float dissolve; //죽은후 디졸브로 사라지는 처리

    

    [SerializeField]private UVClip currClip; //현재 실행중인 uv 클립

    void Start()
    {
        mf = gameObject.GetComponent<MeshFilter>();
        if (!mf)
        {
            Debug.LogError("MeshFilter is missing!!");
        }
        mr = gameObject.GetComponent<MeshRenderer>();
        if (!mr)
        {
            Debug.LogError("MeshRenderer is missing!!");
        }
        mat = mr.material;
        SettingMesh();
    }
  
    void Update()
    {
        SettingMesh(); //Todo : 취약성이 존재하지만 지금은 수정 시간이 생기전까지 업데이트에 자동 갱신으로 할예정

        NormalLogic();
    }
    /// <summary>
    /// 초기 세팅함수
    /// </summary>
    public void SettingMesh()
    {
        mr.material.mainTexture = currClip.TexAnim; //메터리얼의 메인 텍스쳐를 바르는 코드
        //currClip.ChangeFrameTime = currClip.Duration / currClip.TotalCnt;
        currClip.ChangeFrameTime = FrameAvg();
        mat = mr.material;
        UpdateUVWithIndex(currClip.CurFrame);
    }

    public float FrameAvg()
    {
        return currClip.Duration / currClip.TotalCnt;
    }

    /// <summary>
    /// 디폴트
    /// name 들어갈 리소스 : 경로/이름 , row 세로길이 , maxSize 담겨져있는 사진갯수
    /// </summary>
    public void SettingData(string name, int row, int maxSize)
    {
        UVClip uV = new UVClip();

        uV.ColCnt = 5;
        uV.TexAnim = Resources.Load<Texture2D>("SpriteSheet/" + name);
        uV.RowCnt = row;
        uV.TotalCnt = maxSize;
        uVClips.Add(uV);
    }
    /// <summary>
    /// 오버로딩을 통해 텍스트 2D에 리소스로드 과정없이 
    /// OpertorInfo에 넣은값 Textuer2D 형태로 받아온것을 넣을수 있도록 만들어줌
    /// Row(종)갯수는 전체 시트크기 / 횡값
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="row"></param>
    /// <param name="maxSize"></param>
    public void SettingData(Texture2D tex, int maxSize)
    {
        UVClip uV = new UVClip();
        uV.ColCnt = 5;
        uV.TexAnim = tex;
        uV.TotalCnt = maxSize;
        uV.RowCnt = Mathf.CeilToInt((float)maxSize / uV.ColCnt);
        uVClips.Add(uV);

    }
    /// <summary>
    /// 클립 인덱스 번호로 변환 하는 함수
    /// </summary>
    public void ChangeMotion(eMotoin index)
    {
        Motoins = index;
        ClipOff();

        if (uVClips.Count > (int)index)
        currClip = uVClips[(int)index];
        Motoins = index;
        currClip.CurFrame = 0;
        
        ClipOn();
    }

    public void UpdateUVWithIndex(int idx)
    {
        //0,0.2------0.2,0.2
        //  |          |
        //  |          |
        //  0,0----0.2,0

        Vector2 startUV = new Vector2(0.0f, 1.0f); //좌측상단
        float offsetU = 1.0f / currClip.ColCnt;
        float offsetV = 1.0f / currClip.RowCnt;

        //많이쓰임
        float calcU = (idx % currClip.ColCnt) * offsetU;
        float calcV = (idx / currClip.ColCnt) * offsetV;
        Vector2[] newUV = new Vector2[]
        {
            startUV + new Vector2(calcU,-(calcV+offsetV)),
            startUV + new Vector2(calcU+offsetU,-(calcV+offsetV)),
            startUV + new Vector2(calcU,-calcV),
            startUV + new Vector2(calcU+offsetU ,-calcV)
        };
        if (mf != null)
        {
            mf.mesh.uv = newUV;
        }
    }

    /// <summary>
    /// 일반적인 
    /// </summary>
    public void NormalLogic()
    {
        if (currClip.IsLoop == false)
        {
            currClip.ElapsedTime = 0;
            return;
        }
        else
        {
            currClip.ElapsedTime += Time.deltaTime;
            if (currClip.ElapsedTime >= currClip.ChangeFrameTime)
            {
                currClip.CurFrame++;
                if (currClip.CurFrame >= currClip.TotalCnt)
                {
                    //일반적인 공격이 끝난후 아이들로 돌아가는 로직
                    if (Motoins == eMotoin.attack)
                    {
                        if(unit.unitType == Unit.EUnitType.Enemy)
                        {
                            unit.GetComponent<Enemys>().IsAttack = false;
                        }
                        ChangeMotion(eMotoin.idle);
                    }

                    if (currClip.IsLoop) currClip.CurFrame = 0;
                }
                UpdateUVWithIndex(currClip.CurFrame);
                currClip.ElapsedTime = 0.0f;
            }
        }
        if (isDissolve == true && currClip.IsLoop == true)
        {
            getDissolve();
        }
    }

    public void SetDie()
    {
        ChangeMotion(eMotoin.die);
        isDissolve = true;
    }

    public void getDissolve()
    {
        if (dissolve <1)
        {
            dissolve += Time.deltaTime;
            mat.SetFloat("_Cut", dissolve);
        }
        else
        {
            isDissolve = false;
            //죽엇을때 재풀링
            SpawnManager.spawnManager.InsertPool(unit);
            dissolve =0;
            mat.SetFloat("_Cut", dissolve);
            ClipOff();
            return;
        }
    }
    public void ClipOn()
    {
        currClip.IsLoop = true;
    }
    public void ClipOff()
    {
        currClip.IsLoop = false;
    }
}
