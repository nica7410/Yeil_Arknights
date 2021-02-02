using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTileMap : MonoBehaviour
{
    public string mapName ="";
    public bool isFactoryMode = false;

    //스포너 매니저에 이식할것인지 검토

    //타일들의 부모
    public GameObject p_map = null;    //1
    public GameObject p_Sheet = null;  //2
    public GameObject p_Wall = null;   //3
    public GameObject p_Start = null;  //4
    public GameObject p_End = null;    //5
    public GameObject p_Flag = null;   //6

    [SerializeField] private Tiled[] tiles_Map = null;    //1
    [SerializeField] private Tiled[] tiles_Sheet = null;  //2
    [SerializeField] private Tiled[] tiles_Wall= null;    //3
    [SerializeField] private Tiled[] tiles_Start= null;   //5
    [SerializeField] private Tiled[] tiles_End= null;     //4
    [SerializeField] private Tiled[] tiles_Flag = null;   //6

    public Tiled[] Tiles_Map { get { return tiles_Map; }set { tiles_Map = value; } }
    public Tiled[] Tiles_Sheet { get { return tiles_Sheet; }set { tiles_Sheet = value; } }
    public Tiled[] Tiles_Wall { get { return tiles_Wall; }set { tiles_Wall = value; } }
    public Tiled[] Tiles_Start { get { return tiles_Start; }set { tiles_Start = value; } }
    public Tiled[] Tiles_End { get { return tiles_End; }set { tiles_End = value; } }
    public Tiled[] Tiles_Flag { get { return tiles_Flag; }set { tiles_Flag = value; } }


    private void Awake()
    {
        tiles_Map = p_Sheet.GetComponentsInChildren<Tiled>();
        tiles_Sheet = p_Sheet.GetComponentsInChildren<Tiled>();
        tiles_Wall = p_Wall.GetComponentsInChildren<Tiled>();
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
