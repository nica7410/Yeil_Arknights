using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfo : MonoBehaviour
{
    [SerializeField] private int nameNumber; //넘버링
    [SerializeField] protected int attack; //공격력
    [SerializeField] protected float attackTime; //공격 속도
    [SerializeField] protected int defence; //방어력
    [SerializeField] protected int resistance; // 마법저항력
    [SerializeField] protected int maxHp; //체력
    [SerializeField] protected int nowHp; //체력

    [SerializeField] protected ePosition position; //배치가능한 위치

    [SerializeField] protected UVClip[] uVClip;

    public int NameNumber { get => nameNumber; set => nameNumber = value; }
    public int Attack { get => attack; set => attack = value; }
    public int Defence { get => defence; set => defence = value; }
    public int Resistance { get => resistance; set => resistance = value; }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public int NowHp { get => nowHp; set => nowHp = value; }
    public ePosition Position { get => position; set => position = value; }
    public float AttackTime { get => attackTime; set => attackTime = value; }
    public UVClip[] UVClip { get => uVClip; set => uVClip = value; }
}
