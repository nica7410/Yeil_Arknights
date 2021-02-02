using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operator_TestKitano : Operator
{

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        //uVManager.SettingData("Character/Glaucus_Idle_Blue_15_Alpha", 3, 15);  // idle
        //uVManager.SettingData("Character/Glaucus_Die_Blue_55_Alpha", 11, 55); //die
        //uVManager.SettingData("Character/Glaucus_Attack_Blue_20_Aplha", 4, 20); //attack
    }
    protected override void Update()
    {
        base.Update();
    }
}
