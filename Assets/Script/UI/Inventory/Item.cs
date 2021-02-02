using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    new public string name = "New Item";	// Name of the item
    public Sprite CharacterImage;
    public string nameText;
    public string levelText;
    //작업 우선 순위에 따라 정렬 함.

    public Sprite skillIconImage;

    public int exp = 0;

    public Sprite expImage;
    public Sprite starImage;
    public int starNumber;
    public Sprite starColorGradiantImage;
    public Sprite classImage;
    public Sprite eliteImage;
    public Sprite potenialImage;

    public void RemoveFromInventory()
    {
        //Inventory.instance.Remove(this);
    }
}
