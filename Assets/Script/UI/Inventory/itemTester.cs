using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemTester : MonoBehaviour
{
    void Start()
    {
        SetData();
    }

    public void SetData()
    {
        bool tmpbool = true;
        for (int i = 0; i < LoadData.instance.OperatorInfos.Length; ++i)
        {
            tmpbool = true;
            foreach (UserOperator item in LoadData.instance.UserInfo.userOperator)
            {
                if (item.indexName == LoadData.instance.OperatorInfos[i].NameNumber)
                {
                    tmpbool = false;
                    break;
                }
            }
            if (tmpbool == false)
            {
                Inventory.instance.Add(LoadData.instance.Items[i]);
                Inventory.instance.Add(LoadData.instance.OperatorInfos[i]);
            }
        }
    }
}
