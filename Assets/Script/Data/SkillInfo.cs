
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ESkillValue
{
    aotoAdd,
    aotoActive
}

public class SkillInfo : MonoBehaviour
{
    [SerializeField] private string skillName;
    [SerializeField] private Sprite skillImage;
    [SerializeField] private string skillStat1;
    [SerializeField] private string skillStat2;
    [SerializeField] private string skillTag;

    public string SkillName { get => skillName; set => skillName = value; }
    public Sprite SkillImage { get => skillImage; set => skillImage = value; }
    public string SkillTag { get => skillTag; set => skillTag = value; }
    public string SkillStat1 { get => skillStat1; set => skillStat1 = value; }
    public string SkillStat2 { get => skillStat2; set => skillStat2 = value; }

    /// <summary>
    /// 상태 스트링을 바인딩 작업 시켜줘서 그거대로 받아 오는함수
    /// </summary>
    /// <param name="eSkillValue"></param>
    /// <returns></returns>
    public string SkillSatat1(ESkillValue eSkillValue)
    {
        
        switch (eSkillValue)
        {
            case ESkillValue.aotoAdd:
                skillStat1 = "자동 회복";
                break;
            case ESkillValue.aotoActive:
                skillStat1 = "자동 발동";
                break;
            default:
                break;
        }

        return skillStat1;
    }
}


