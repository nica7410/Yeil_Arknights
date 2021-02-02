using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTileMapTest : MonoBehaviour
{
    public string mapName ="";
    public bool isFactoryMode = false;

    //스포너 매니저에 이식할것인지 검토

    //타일들의 부모
    public GameObject p_map = null;    //1
    public GameObject p_Sheet = null;  //2

    public GameObject p_bottom = null;   //3
    public GameObject p_top = null;   //3
    public GameObject p_all = null;   //3
    public GameObject p_none = null;   //3

    public GameObject p_Start = null;  //4
    public GameObject p_End = null;    //5
    public GameObject p_Flag = null;   //6

    [SerializeField] private Tiled[] tiles_Map = null;    //1
    [SerializeField] private Tiled[] tiles_sheet = null;  //2

    [SerializeField] private Tiled[] tiles_bottom = null;  //2
    [SerializeField] private Tiled[] tiles_top = null;  //2
    [SerializeField] private Tiled[] tiles_all = null;  //2
    [SerializeField] private Tiled[] tiles_none = null;  //2

    [SerializeField] private Tiled[] tiles_Start= null;   //5
    [SerializeField] private Tiled[] tiles_End= null;     //4
    [SerializeField] private Tiled[] tiles_Flag = null;   //6



    private void Awake()
    {
        tiles_Map = p_Sheet.GetComponentsInChildren<Tiled>();

        tiles_sheet = p_Sheet.GetComponentsInChildren<Tiled>();

        tiles_bottom = p_bottom.GetComponentsInChildren<Tiled>();
        tiles_top = p_top.GetComponentsInChildren<Tiled>();
        tiles_all = p_all.GetComponentsInChildren<Tiled>();
        tiles_none = p_none.GetComponentsInChildren<Tiled>();

        tiles_Start = p_Start.GetComponentsInChildren<Tiled>();
        tiles_End = p_End.GetComponentsInChildren<Tiled>();
        tiles_Flag = p_Flag.GetComponentsInChildren<Tiled>();
    }
    
    public Vector3 GetTile_Transform(int section)
    {
        
            return tiles_Start[section].transform.position;
    }

    public Transform[] GetPathWithPatten(int[] patten)
    {
        List<Transform> path = new List<Transform>();
        for(int i=0;i<patten.Length;i++)
        {
            path.Add(tiles_Flag[patten[i]].transform);
        }
        return path.ToArray();
    }
}
