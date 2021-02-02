using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardPanel : MonoBehaviour
{
    [SerializeField] private Text boardText = null;
    [SerializeField] private Text lifeText = null;


    private void Awake()
    {
        boardText = GameObject.Find("EnmeyText").GetComponent<Text>();
        lifeText = GameObject.Find("LifeText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        BoarderUpdate();
    }

    private void BoarderUpdate()
    {
        boardText.text = Stage.instance.NowEntry + "/" + Stage.instance.TotalEntry;
        if(Stage.instance.Life > 100)
        {
            lifeText.text = "0";
        }
        else
        {
            lifeText.text = "" + Stage.instance.Life;
        }
    }
}
