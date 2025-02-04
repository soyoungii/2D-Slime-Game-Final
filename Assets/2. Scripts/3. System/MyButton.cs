using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class MyButton : MonoBehaviour
{
    private bool isDoubleSpeed = false; //2배속 여부
    public void Speed2X()
    {
        isDoubleSpeed = !isDoubleSpeed; 
        if (isDoubleSpeed)
        {
            Time.timeScale = 2f;
            UIManager.Instance.speed2xText.SetActive(true);
        }

        else
        {
            Time.timeScale = 1f;
            UIManager.Instance.speed2xText.SetActive(false);
        }

    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        UIManager.Instance.pauseexit.SetActive(true);
        isDoubleSpeed = false;
        UIManager.Instance.speed2xText.SetActive(false);
    }

    public void Restart()
    {
        UIManager.Instance.pauseexit.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ClickStarlight()
    {
        UIManager.Instance.unlockPanels[0].SetActive(true);
    }

    public void ClickSphere()
    {
        UIManager.Instance.unlockPanels[1].SetActive(true);
    }

    public void ClickMeteor()
    {
        UIManager.Instance.unlockPanels[2].SetActive(true);
    }

    public void ClickThunder()
    { 
        UIManager.Instance.unlockPanels[3].SetActive(true);
    }

    public void ClickAnger()
    {
        UIManager.Instance.unlockPanels[4].SetActive(true);

    }
    public void NoStarlight()
    {
        UIManager.Instance.unlockPanels[0].SetActive(false);
    }

    public void NoSphere()
    {
        UIManager.Instance.unlockPanels[1].SetActive(false);
    }

    public void NoMeteor()
    {
        UIManager.Instance.unlockPanels[2].SetActive(false);
    }

    public void NoThunder()
    {
        UIManager.Instance.unlockPanels[3].SetActive(false);
    }

    public void NoAnger()
    {
        UIManager.Instance.unlockPanels[4].SetActive(false);
    }

    public void SkillScreenClose()
    {
        UIManager.Instance.skillNoGold.SetActive(false);
    }

    public void UpGradeScreenClose()
    {
        UIManager.Instance.upgradeNoGold.SetActive(false);
    }
}
