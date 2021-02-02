using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    //클래스 설명
    /*
     이클래스는 타일들을 생성 , 수정 , 삭제 시켜주는 에디터용 클래스 이다.
    생성 방식은 기존 타일들을 배열값으로 지정해 기본 4각형 형식으로 전체적인 맵을 그린뒤
    위에 벽 ,콜라이더 없는 지형  (함정 or 투명벽) ,  시작지점 , 도착지점을 수정할수있게 만들수있도록 
    클래스 구조를 잡아놓음
     */

    public int MAX; //타일의 총크기를 구하는 변수
    public int change;
    public int arryCount=0;
    public int chk_MouseORKey = 0;

    public int pickPos =0; // 선택된 타일의 현재 배열값

    Vector3 newVector3; //타일을 만들때 위치 잡아주는 벡터
    public GameObject temp;


    public Transform pickingTileTr; //선택된 오브젝트
    

    public Tiled tiled; //선택된타일의 클래스
    public Material[] materials = null;

    //타일들의 부모
    public GameObject p_Map;

    public GameObject p_Sheet;
    public GameObject p_Wall;
    public GameObject p_End;
    public GameObject p_Start;
    public GameObject p_Flag;


    public List<GameObject> cubePrefab = null; //어떤 객체를 만들어줄것인지 리스트로 담고 생성해주기위한 리스트]

    public FloorTileMap floorTileMap = null;

    public int isRight;       //isRight이 -1이면 오른쪽이 아닌 왼쪽으로 범위를 그려준다.
    public int isUp;           //isUp이 -1이면 위쪽이 아닌 아래쪽으로 범위를 그려준다.
    public int OnOff;  //OnOff가 0이면 위아래 계산, 1이면 좌우 계산

    //ScriptableObject.CreateInstance<MapInfo> mapInfos = ScriptableObject.CreateInstance<MapInfo>();

    private void Awake()
    {
        Vector3 startPoss = new Vector3(0, 0, 0); // 지역변수 스타트 포지션을 잡아주는 변수

        //배열의 총크기만큼 종횡을 더해줘서이중 포문으로 생성
        for (int height = 0; height < MAX; height++)
        {
            for (int with = 0; with < MAX; with++)
            {
                newVector3 = new Vector3(startPoss.x + with, startPoss.y, startPoss.z + height);
                CreateTile();
            }
        }
        //floorTileMap = GameObject.Find("Map").GetComponent<FloorTileMap>();
        floorTileMap.isFactoryMode = true;
    }
    //타일매니저 호출시 
    void Start()
    {
        
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0)) //타일 선택 인풋(왼쪽 마우스)
        {
            RayInput_Mouse();
        }
        if (Input.GetMouseButtonDown(1)) // (오른쪽마우스)
        {


        }
        else if (Input.GetMouseButton(1))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) //벽생성 인풋키 중복 높이 1고정 key :1
        {
            CreateWall();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) //벽 삭제 인풋키 key : 2
        {
            DestroyWall();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) //벽 교체 key : 3 
        {
            ChageTile(change);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) //벽생성 중복높이 제한없음 key :4
        {
            CreateWall_Nonelimitt();
        }
        if(Input.GetKeyDown(KeyCode.Q)) //플레그 타일 생성 키 
        {
            CreateFlag();
        }


        //이동키 입력시 픽 이동
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            KeyEventTile(-1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            KeyEventTile(1);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            KeyEventTile(MAX);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            KeyEventTile(-MAX);
        }
    }//End Update
    public void KeyEventTile()
    {
        if (pickingTileTr != null)
        {
            tiled.mr.material = tiled.mat;
        }

        pickPos = pickingTileTr.GetComponent<Tiled>().arry;
        pickingTileTr = floorTileMap.Tiles_Sheet[pickingTileTr.GetComponent<Tiled>().arry].GetComponent<Transform>();

        tiled = pickingTileTr.GetComponent<Tiled>();

        tiled.mr.material = materials[0];
        chk_MouseORKey = 2;
    }
    public void KeyEventTile(int range)
    {
        if (pickingTileTr != null)
        {
            tiled.mr.material = tiled.mat;
        }

        pickPos = pickingTileTr.GetComponent<Tiled>().arry+range;
        if(pickPos<0)
        {

        }
        else if(MAX*MAX < pickPos) //Todo : ??
        {

        }
        else
        {
            pickingTileTr = floorTileMap.Tiles_Sheet[pickingTileTr.GetComponent<Tiled>().arry + range].GetComponent<Transform>();
            tiled = pickingTileTr.GetComponent<Tiled>();
        }
        tiled.mr.material = materials[0];


        chk_MouseORKey = 2;
    }

    //타일 생성 
    private void CreateTile()
    {
        GameObject tile = Instantiate(cubePrefab[0], newVector3, Quaternion.identity);
        Tiled cube = tile.GetComponent<Tiled>();
        tile.transform.parent = p_Sheet.transform; //부모정해주기
        cube.arry = arryCount;
        arryCount++;
        arryCount = Mathf.Clamp(arryCount, 0, 500);
    }
    //오버로딩 생성함수 (불러올프리팹 , y위치 , 부모)
    private void CreateTile(int number , int ySum , GameObject parented)
    {
        RaycastHit hit;
        if (Physics.Raycast(pickingTileTr.position, pickingTileTr.up, out hit))
        {
            if (hit.transform.tag == "Tile")
            {
                Debug.Log("타일위에 객체가 존재함");
                if (number == 4)
                {
                    Vector3 pos = new Vector3(tiled.transform.position.x, tiled.transform.position.y+1, tiled.transform.position.z);
                    GameObject tile = Instantiate(cubePrefab[number], pos, Quaternion.identity);
                    Tiled cube = tile.GetComponent<Tiled>();
                    //벽 생성이후 예외 처리
                    tiled.mr.material = tiled.mat;
                    tile.transform.parent = parented.transform; //부모정해주기
                    cube.arry = tiled.arry;
                }
            }
            
        }
        else
        {
            Vector3 pos = new Vector3(tiled.transform.position.x, tiled.transform.position.y + ySum, tiled.transform.position.z);
            GameObject tile = Instantiate(cubePrefab[number], pos, Quaternion.identity);
            Tiled cube = tile.GetComponent<Tiled>();
            //벽 생성이후 예외 처리
            tiled.mr.material = tiled.mat;
            tile.transform.parent = parented.transform; //부모정해주기
            cube.arry = tiled.arry;
        }
       
    }
    //벽생성 제한있음1개
    private void CreateWall()
    {
        if(pickingTileTr != null)
        {
            if (tiled.Values == 1)
            {
                Debug.Log("벽타일 선택시 생성불가");
                return;
            }
            else 
            {
                CreateTile(1, 1, p_Wall);
                //if (chk_MouseORKey==1)
                //{
                //    pickingTileTr = null;
                //    tiled = null;
                //}
            }
        }
    }
    private void CreateFlag()
    {
        if (pickingTileTr != null)
        {
            if (tiled.Values == 4)
            {
                Debug.Log("벽타일 선택시 생성불가");
                return;
            }
            else
            {
                CreateTile(4, 1, p_Flag);
            }
        }
    }
    //벽생성 제한없음
    private void CreateWall_Nonelimitt()
    {
        if (pickingTileTr != null)
        {
            Vector3 pos = new Vector3(tiled.transform.position.x, tiled.transform.position.y + 1, tiled.transform.position.z);
            GameObject tile = Instantiate(cubePrefab[1], pos, Quaternion.identity);
            Tiled cube = tile.GetComponent<Tiled>();
            //벽 생성이후 예외 처리
            tiled.mr.material = tiled.mat;
            tile.transform.parent = p_Wall.transform; //부모정해주기
            pickingTileTr = null;
            tiled = null;
        }
    }
    //벽삭제 
    private void DestroyWall()
    {
        if (pickingTileTr != null)
        {
            if(chk_MouseORKey ==1)
            {
                if (tiled.Values == 1 || tiled.Values==4)
                {
                    Destroy(pickingTileTr.gameObject);
                    //삭제이후 미싱이 되어버리니 예외처리 
                    pickingTileTr = null;
                    tiled = null;
                }
                else
                {
                    Debug.Log("바닥은 삭제불가능");
                    return;
                }
            }
            else if(chk_MouseORKey == 2)
            {
                RayInput_Object_Destroy();
            }
        }
        RayInput_Object_Destroy();
    }
    //타일 교체
    private void ChageTile(int value)
    {
        if (pickingTileTr != null)
        {
            Vector3 pos = tiled.transform.position;
            GameObject tile = Instantiate(cubePrefab[value], pos, Quaternion.identity);
            Tiled cube = tile.GetComponent<Tiled>();
            switch (value)
            {
                case 0:
                    tile.transform.parent = p_Sheet.transform; //p_Sheet
                    break;
                case 1:
                    tile.transform.parent = p_Wall.transform; //p_Wall
                    break;
                case 2:
                    tile.transform.parent = p_Start.transform; //p_Start
                    break;
                case 3:
                    tile.transform.parent = p_End.transform; //p_End
                    break;
            }
            Destroy(pickingTileTr.gameObject);
            pickingTileTr = null;

            tiled = null;
        }
    }


    //객체에서 레이쏘는 함수
    private void RayInput_Object_Destroy()
    {
        //Ray ray = pickingTileTr;
        RaycastHit hit;
        if (Physics.Raycast(pickingTileTr.position, pickingTileTr.up, out hit))
        {
            if (hit.transform.tag == "Tile")
            {
                Destroy(hit.transform.gameObject);
            }
        }
    }
    //마우스 입력시 레이케스트로 마우스 포지션으로 잡은 객체 값담는 함수
    private void RayInput_Mouse()
    {
        chk_MouseORKey = 1;
        //이미 담겨져있는 오브젝트가 있을때 다음 히트로 옮겨지기전에 기존걸 초기화 시켜주는 로직
        if (pickingTileTr != null)
        {
            tiled.mr.material = tiled.mat;
        }

        //레이케스트가 바라본 방향의 객체를 선택해서 오브젝트를 선택해주는 로직
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Tile")
            {
                //Debug.Log("히트 : " + hit.transform.name);
                if (hit.collider != null) //히트가 낫널일때 허공을찍엇거나 레이가 히트하는 대상이 없을때를 예외처리
                {
                    pickingTileTr = hit.transform;
                    tiled = hit.transform.GetComponent<Tiled>();
                }
                tiled.mr.material = materials[0];
            }
        }
    }
}//END Class 
