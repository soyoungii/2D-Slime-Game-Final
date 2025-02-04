using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Anger : BaseSkill
{
    private GameObject currentAngerEffect;

    protected override void Awake()
    {
        base.Awake();
        cooldown = 20f;
    }

    protected override IEnumerator SkillCoroutine()
    {
        while (true)
        {
            cooldownImage.fillAmount = 1f;
            currentAngerEffect = Instantiate(skillPrefab, skillPrefab.transform.position, Quaternion.identity);
            float originalDamage = slime.damage;
            slime.damage *= 2f;
            yield return new WaitForSeconds(10f);

            if (currentAngerEffect != null)
            {
                Destroy(currentAngerEffect);
            }
            
            slime.damage = originalDamage;
            yield return StartCoroutine(Cooldown());
        }
    }
}