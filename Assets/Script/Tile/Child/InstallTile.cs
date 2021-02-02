using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallTile : Tiled
{
    [SerializeField] protected ePosition ePosition = ePosition.All;

    [SerializeField] private LightQuad quad = null;

    [SerializeField] protected InStageToggleGroup toggleGroup = null;

    protected override void Start()
    {
        TileQuad();
        //quad = this.GetComponentInChildren<GameObject>();
        toggleGroup = FindObjectOfType<InStageToggleGroup>();
        tileMap = this.transform.parent.parent.GetComponent<FloorTileMap>();
        quad.transform.position = this.transform.localPosition+(Vector3.up*0.2f);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(!tileMap.isFactoryMode)
        {
            if (InStageUI.instance.NowToggle)
            {
                if (InStageUI.instance.NowToggle.operatorInfo.Position == ePosition ||
                    InStageUI.instance.NowToggle.operatorInfo.Position == ePosition.All)
                {
                    if (quad.gameObject.activeSelf == false)
                    {
                        quad.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (quad.gameObject.activeSelf == true)
                    {
                        quad.gameObject.SetActive(false);
                    }

                }
            }
            else
            {

                if (quad.gameObject.activeSelf == true)
                {
                    quad.gameObject.SetActive(false);
                }
            }
        }
        
    }

    private LightQuad TileQuad()
    {
        if (this.transform.childCount == 0)
        {
            LightQuad obj = Instantiate(Resources.Load<LightQuad>("Prefap/Tile/Quad"));
            obj.transform.parent = this.transform;
            
            return quad = obj;
        }
        else
        {
            return quad = this.GetComponentInChildren<LightQuad>();
        }


    }
}
