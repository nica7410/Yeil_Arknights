using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUImanager : MonoBehaviour
{
    public static InGameUImanager instance =null;
    public Image panel = null;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        panel = this.transform.GetChild(1).GetComponent<Image>(); 
    }
}
