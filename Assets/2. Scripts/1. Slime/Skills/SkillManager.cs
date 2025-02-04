using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Text 컴포넌트를 위한 네임스페이스

public class SkillManager : MonoBehaviour
{
    [System.Serializable]
    public class SkillData
    {
        public BaseSkill skill;
        public int cost;
        public GameObject lockIcon;
        public GameObject bottomText;
        public GameObject goldImage;
        public GameObject unlockPanel;
        public Text topText;    
        public Text closeText;    
    }

    [SerializeField] private List<SkillData> skills;
    
    public void UnlockStarlight()
    {
        UnlockSkill(0);
    }

    public void UnlockSphere()
    {
        UnlockSkill(1);
    }

    public void UnlockMeteor()
    {
        UnlockSkill(2);
    }

    public void UnlockThunder()
    {
        UnlockSkill(3);
    }

    public void UnlockAnger()
    {
        UnlockSkill(4);
    }
    
    private void UnlockSkill(int skillIndex)
    {
        var skillData = skills[skillIndex];
        
        if (GameManager.Instance.slime.gold >= skillData.cost)
        {
            GameManager.Instance.slime.gold -= skillData.cost;
            
            Destroy(skillData.lockIcon);
            Destroy(skillData.bottomText);
            Destroy(skillData.goldImage);
            
            skillData.skill.StartSkill();
            skillData.unlockPanel.SetActive(false);
            
            skillData.topText.text = "이미 해금된 스킬입니다";
            skillData.closeText.text = "창닫기";
        }
        else
        {
            skillData.unlockPanel.SetActive(false);
            UIManager.Instance.skillNoGold.SetActive(true);
        }
    }
}


