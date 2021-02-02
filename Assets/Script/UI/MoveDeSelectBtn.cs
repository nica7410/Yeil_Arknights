using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDeSelectBtn : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 inSide;
    private Vector2 outSide;

    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
        inSide = new Vector2(-2f, -145f);
        outSide = new Vector2(-2f, 1000f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoveDeSelctBtnOut()
    {
        rectTransform.anchoredPosition = outSide;
    }
    public void MoveDeSelctBtnIn()
    {
        rectTransform.anchoredPosition = inSide;
    }
}
