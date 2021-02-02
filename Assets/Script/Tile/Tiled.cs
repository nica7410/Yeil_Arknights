using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiled : MonoBehaviour
{
    public int arry = 0;
    //public Vector2 posNum; //2차원 배열대신 사용되는 자기의 위치값
    protected int values;
    public int Values { get { return values; } set { values = value; } }
    public MeshRenderer mr; //매테리얼 랜더러
    public Material mat; // 매테리얼
    public FloorTileMap tileMap = null;




    private int curTexNum = 0;
    private void OnEnable()
    {
        //mr = this.GetComponent<MeshRenderer>();
        //mat = mr.material;
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }
    protected virtual void Update()
    {
        
    }
}

