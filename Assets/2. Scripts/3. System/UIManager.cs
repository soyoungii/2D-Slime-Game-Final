using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class UIManager : SingletonManager<UIManager>
{
    [Header("화면 왼쪽 상단 슬라임 정보")]
    public Text gold; //현재 골드(왼쪽 상단)
    public Text myDamage; //현재 플레이어의 공격력(왼쪽 상단)
    public Text myHp;

    [Header("강화창 데미지 정보")]
    public Text damageLevel; //강화 화면 속 플레이어의 데미지 레벨
    public Text damageText; //강화 화면 속 플레이어의 데미지(공격력)
    public Text damageGold; //강화 화면 속 데미지 강화에 필요한 골드

    [Header("강화창 체력 정보")]
    public Text hpLevel; //강화 화면 속 플레이어의 Hp 레벨
    public Text hpText; //강화 화면 속 플레이어의 Max Hp
    public Text hpGold; //강화 화면 속 hp 강화에 필요한 골드

    [Header("강화창 체력 회복 정보")]
    public Text hpRecoverLevel; //강화 화면 속 플레이어의 체력 회복 레벨
    public Text hpRecoverText; //강화 화면 속 플레이어의 체력 회복량
    public Text hpRecoverGold; //강화 화면 속 체력회복 강화에 필요한 골드

    [Header("강화창 치명타 확률 정보")]
    public Text criticalLevel; //강화 화면 속 플레이어의 치명타 확률 레벨
    public Text criticalText; //강화 화면 속 플레이어의 치명타 확률
    public Text criticalGold; //강화 화면 속 치명타 확률 강화에 필요한 골드

    [Header("강화창 치명타 피해 정보")]
    public Text criDamLevel; //강화 화면 속 플레이어의 치명타 피해 레벨
    public Text criDamText; //강화 화면  속 플레이어의 치명타 피해
    public Text criDamGold; //강화 화면 속 치명타 피해 강화에 필요한 골드

    [Header("강화창 공격 속도 정보")]
    public Text atkSpeedLevel; //강화 화면 속 플레이어의 공격 속도 레벨
    public Text atkSpeedText; //강화 화면 속 플레이어의 공격 속도
    public Text atkSpeedGold; //강화 화면 속 공격 속도 강화에 필요한 골드

    [Header("강화창 더블샷 정보")]
    public Text dShotLevel; //강화 화면 속 플레이어의 더블샷 레벨
    public Text dShotText; //강화 화면 속 플레이어의 더블샷 확률
    public Text dShotGold; //강화 화면 속 더블샷 강화에 필요한 골드

    [Header("스킬 자물쇠 이미지")]
    public GameObject lockStarlight; //스타라이트 스킬 자물쇠 이미지
    public GameObject lockSphere;
    public GameObject lockMeteor;
    public GameObject lockThunder;
    public GameObject lockAnger;

    [Header("스킬 해금창 UI")]
    public GameObject starlightUnlock; // 스타라이트 스킬 해금창 UI
    public GameObject sphereUnlock; //구체 스킬 해금창 UI
    public GameObject meteorUnlock; //메테오 해금창 UI
    public GameObject thunderUnlock; //벼락 해금창 UI
    public GameObject angerUnlock; //분노 해금창 UI

    [Header("스킬 해금창 골드 이미지(해금 후 삭제)")]
    public GameObject starlightGoldImage; //스타라이트 골드 이미지(해금 후 삭제용)
    public GameObject sphereGoldImage;
    public GameObject meteorGoldImage;
    public GameObject thunderGoldImage;
    public GameObject angerGoldImage;

    [Header("스킬 해금창 상단 글자(해금 후 변경)")]
    public Text starlightTop; //스타라이트 상단 글자(해금 후 변경용)
    public Text sphereTop;
    public Text meteorTop;
    public Text thunderTop;
    public Text angerTop;

    [Header("스킬 해금창 하단 버튼1(해금 후 삭제)")]
    public GameObject starlightBottom; //스타라이트 하단 버튼(해금 후 삭제용)
    public GameObject sphereBottom;
    public GameObject meteorBottom;
    public GameObject thunderBottom;
    public GameObject angerBottom;

    [Header("스킬 해금창 하단 버튼2(해금 후 변경)")]
    public Text starlightCloseScreen; //스타라이트 창닫기 글자(해금 후 변경용)
    public Text sphereCloseScreen;
    public Text meteorCloseScreen;
    public Text thunderCloseScreen;
    public Text angerCloseScreen;

    [Header("스킬 해금 및 강화 실패 UI")]
    public GameObject upgradeNoGold; //강화 실패 UI
    public GameObject skillNoGold; //스킬 해금 실패 UI

    [Header("화면 오른쪽 상단 버튼")]
    public GameObject speed2xText;
    public GameObject pauseexit;
}
