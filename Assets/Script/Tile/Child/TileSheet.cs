using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSheet : InstallTile
{
    protected override void Start()
    {
        base.Start();
        values = 0;  //TILE: Tile 0바닥 
        ePosition = ePosition.Bottom;
    }

    protected override void Update()
    {
        base.Update();
    }
}
