using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWall : InstallTile
{
    protected override void Start()
    {
        base.Start();
        ePosition = ePosition.Top;
        values = 1; //TILE: Tile 1벽 

    }

    protected override void Update()
    {
        base.Update();
    }
}
