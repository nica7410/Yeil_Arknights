using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkRangeChild : MonoBehaviour
{
    private Operator AD;
    // 생성되면 부모에게 자식의 위치 정보를 받게함
    void Start()
    {
        AD = this.GetComponentInParent<Operator>();
        AD.SetChildTrs();
    }
}
