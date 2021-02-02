using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapCanvasUI : MonoBehaviour
{
    TileManager tileManager = null;
    private void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
    }

}
