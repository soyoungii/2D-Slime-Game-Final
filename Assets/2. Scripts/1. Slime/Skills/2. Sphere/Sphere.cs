using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Sphere : BaseSkill
{

    protected override void Awake()
    {
        base.Awake();
        cooldown = 3f;
    }

    protected override IEnumerator SkillCoroutine()
    {
        while (true)
        {
                cooldownImage.fillAmount = 1f;
                GameObject sphereInstance = Instantiate(skillPrefab, skillPrefab.transform.position, Quaternion.identity);
                Destroy(sphereInstance, 5f);

                yield return StartCoroutine(Cooldown());
        }
    }
}