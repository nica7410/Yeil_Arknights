using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSetting : MonoBehaviour
{
    //public MeshFilter[] mf = null;
    public MeshRenderer[] mr = null;
    GameObject[] childRange = null;
    Material[] materials = null;
    void Start()
    {
        //childRange = this.GetComponentsInChildren<GameObject>();
        mr = this.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i< mr.Length;i++)
        {
            mr[i].material = Resources.Load<Material>("Mat/Shader/MatAttackRange");
        }
    }
}
