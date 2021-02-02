using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostUI : MonoBehaviour
{
    [SerializeField] Text costText = null;
    
    [SerializeField] private Slider slider = null;
    [SerializeField] private float charging = 0;
    [SerializeField] private float maxCharging = 2.5f;

    private void Awake()
    {
        this.costText = this.GetComponentInChildren<Text>();
        this.slider = this.GetComponentInChildren<Slider>();
    }
    void Start()
    {
        maxCharging = 2.5f;
        slider.maxValue = maxCharging;

        costText.text = "" + Stage.instance.stageInfo.Cost;
    }

    
    void Update()
    {
        ChargingUpdate();
    }

    /// <summary>
    /// 코스트 차징하는 함수
    /// </summary>
    private void ChargingUpdate()
    {
        charging += Time.deltaTime * Stage.instance.NowTime;
        slider.value = charging;
        if (charging > maxCharging)
        {
            charging = 0;
            Stage.instance.Cost++;
        }
        costText.text = "" + Stage.instance.Cost;
    }
}
