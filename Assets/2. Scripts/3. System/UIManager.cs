using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Serialization;


public class UIManager : SingletonManager<UIManager>
{
    [Header("화면 왼쪽 상단 슬라임 정보")]
    public Text myGold; //현재 골드(왼쪽 상단)
    public Text myDamage; //현재 플레이어의 공격력(왼쪽 상단)
    public Text myHp;

    public List<Text> level;
    public List<Text> valueText;
    public List<Text> gold;

    public List<GameObject> lockImages;
    public List<GameObject> unlockPanels;
    
    [Header("스킬 해금 및 강화 실패 UI")]
    public GameObject upgradeNoGold; //강화 실패 UI
    public GameObject skillNoGold; //스킬 해금 실패 UI

    [Header("화면 오른쪽 상단 버튼")]
    public GameObject speed2xText;
    public GameObject pauseexit;
}
