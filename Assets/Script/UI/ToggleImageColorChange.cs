using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleImageColorChange : MonoBehaviour
{
    private Image clickedImage;

    private Color gray;
    private Color white;
    void Start()
    {
        clickedImage = this.GetComponent<Image>();
        gray = new Color(0.3f, 0.3f, 0.3f);
        white = new Color(1f, 1f, 1f);
    }
    public void setGray()
    {
        clickedImage.color = gray;
    }
    public void setWhite()
    {
        clickedImage.color = white;
    }
}
