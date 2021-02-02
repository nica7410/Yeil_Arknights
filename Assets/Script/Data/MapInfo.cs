using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Map", menuName = "Inventory/MapStat")]
public class MapInfo : ScriptableObject
{
    public TileSheet tileSheet;
    public TileWall tileWall;
}
