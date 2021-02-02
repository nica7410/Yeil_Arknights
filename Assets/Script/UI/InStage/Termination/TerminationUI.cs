using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminationUI : MonoBehaviour
{
    [SerializeField] private Image thisimage = null;
    [SerializeField] private Text text= null;
    [SerializeField] private Image image= null;

    [SerializeField] private Sprite clear = null;

    [SerializeField] private Sprite failed = null;


    private void Start()
    {
        thisimage = this.GetComponent<Image>();
        text = GameObject.Find("GameEndLayerText").GetComponent<Text>();
        image = GameObject.Find("GameEndLayerImage").GetComponent<Image>();
        clear = Resources.Load<Sprite>("Icon/Class/00Vanguard");
        failed = Resources.Load<Sprite>("Icon/Class/04Medic");


        this.gameObject.SetActive(false);

    }

    public void MissonFailed(bool chk)
    {
        if(chk)
        {
            text.text = "<size=70>임무성공</size>\n" + "<size=30>MiSSion ACCOMPLISHED</size>";
            thisimage.color = RGBColor(0, 253, 224, 100);
            image.sprite = clear;
        }
        else
        {
            text.text = "<size=70>임무실패</size>\n" + "<size=30>MiSSion FALED</size>";
            thisimage.color = RGBColor(253, 0, 35, 100);
            image.sprite = failed;
        }

    }

    public Color RGBColor(int r, int g, int b, int a)
    {
        return new Color(r/255f, g/255f, b/255f, a/255f);
    }
}
