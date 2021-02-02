using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorSlot : MonoBehaviour
{
    [SerializeField]private RectTransform rectTransform;
    Vector2 vector2;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }
    public void ClickOperatorSlot()
    {
        //ToDo : 예외처리가 필요함
        if(!rectTransform)
        {
            rectTransform = this.GetComponent<RectTransform>();
        }
        vector2 = new Vector2(1500 +(180 * ((Inventory.instance.operatorInfo.Count / 2) + 1)), 720);
        rectTransform.sizeDelta = vector2;
        rectTransform.position = new Vector3((vector2.x / 2)+1000, rectTransform.position.y, rectTransform.position.z);
    }



}
