using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveCtrl : MonoBehaviour
{
    private MeshRenderer mr = null;
    private Material mat = null;
    public Texture2D tex = null;

    [SerializeField]float dissolve;
    [SerializeField] float speed;
    [SerializeField] bool chk;

    private void OnEnable()
    {
        
    }

    private void Awake()
    {
        mr = this.GetComponent<MeshRenderer>();
        mat = mr.material;
        dissolve = 0;
        speed = 1.0f;

        //SetTexture(stirng 프로퍼티이름 , 텍스쳐 벨류);
        mat.SetTexture("_MainTex", tex);
        mat.SetColor("_OutColor", new Color(0.5f,0.5f,0.5f));
        mat.SetFloat("_Cut", dissolve);
        //mat.SetInt();
        //mat.SetMatrix();

    }
    void Start()
    {
        
    }
    void Update()
    {
        chk = dissolve > 1 ? true : false;
        if (chk == false)
        {
            dissolve += speed * Time.deltaTime;
        }
        else
        {
            dissolve -= speed * Time.deltaTime;
        }
        //Mathf.Sin();

        mat.SetFloat("_Cut", dissolve);
    }
}
