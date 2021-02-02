using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectUv : MonoBehaviour
{
    private MeshFilter mf = null;
    private MeshRenderer mr = null;

    public Texture2D texAnim = null;
    public Color color = new Color();

    public int colCnt = 5;
    public int rowCnt = 5;
    public int totalCnt = 25;
    public bool isLoop = true;              //

    private float duration = 0.5f;          //애니메이션 플레이 시간
    [SerializeField]private float changeFrameTime = 1.0f;   // 프레임 변경시간 (간격)
    private float elapsedTime = 0.0f;       //누적시간
    private int curFrame = 0;               // 현재 프레임

    private void OnEnable()
    {
        mf= this.GetComponent<MeshFilter>();
        if(!mf)
        {
            Debug.LogError("MeshFilter is missing!!");
        }
        mr = this.GetComponent<MeshRenderer>();
        if (!mr)
        {
            Debug.LogError("MeshRenderer is missing!!");
        }
        mr.material.mainTexture = texAnim;
        mr.material.SetColor("_Color", color);
        changeFrameTime = duration / totalCnt;
        UpdateUVWithIndex(curFrame);
    }
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<mf.mesh.vertexCount;i++)
        {
            Debug.Log(i + " : " + mf.mesh.vertices[i]);
            Debug.Log(i + " : " + mf.mesh.uv[i]);
        }
        




    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        

        if (elapsedTime >= changeFrameTime)
        {
            ++curFrame;
            if(curFrame >= totalCnt)
            {
                if (isLoop) curFrame = 0;
                this.transform.position = this.transform.parent.position;
                this.gameObject.SetActive(false);
            }
            UpdateUVWithIndex(curFrame);
            elapsedTime = 0.0f;
        }
    }
    //4각형 간격에 맞게 계산해주기
    private void UpdateUVWithIndex(int idx)
    {

        //0,0.2------0.2,0.2
        //  |          |
        //  |          |
        //  0,0----0.2,0
        
        Vector2 startUV = new Vector2(0.0f,1.0f); //좌측상단
        float offsetU = 1.0f / colCnt;
        float offsetV = 1.0f / rowCnt;

        //많이쓰임
        float calcU = (idx % colCnt) * offsetU;
        float calcV = (idx / colCnt) * offsetV;
        Vector2[] newUV = new Vector2[]
       {
            startUV + new Vector2(calcU,-(calcV+offsetV)),
            startUV + new Vector2(calcU+offsetU,-(calcV+offsetV)),
            startUV + new Vector2(calcU,-calcV),
            startUV + new Vector2(calcU+offsetU ,-calcV)
       };
        mf.mesh.uv = newUV;
    }
} //class End
