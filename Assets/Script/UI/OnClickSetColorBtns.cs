using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnClickSetColorBtns : MonoBehaviour
{
    public Image clickedImage;
    public Image nonClickedImage1;
    public Image nonClickedImage2;
    public Image nonClickedImage3;
    public void OnClickChangeColor()
    {
        clickedImage.color = new Color(1, 1, 1);
        nonClickedImage1.color = new Color(0.2f, 0.2f, 0.2f);
        nonClickedImage2.color = new Color(0.2f, 0.2f, 0.2f);
        nonClickedImage3.color = new Color(0.2f, 0.2f, 0.2f);
    }
}
