using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Thunder : BaseSkill
{
    protected override void Awake()
    {
        base.Awake();
        cooldown = 5f;
    }

    protected override IEnumerator SkillCoroutine()
    {
        while (true)
        {
            cooldownImage.fillAmount = 1f;

            for (int i = 0; i < 8; i++)
            {
                Monster target = FindNearestMonsterInRange();
                Vector3 spawnPosition;

                if (target != null)
                {
                    spawnPosition = target.transform.position;
                    GameObject thunder = Instantiate(skillPrefab, spawnPosition, Quaternion.identity);
                    target.TakeDamage(slime.damage);
                    Destroy(thunder, 1f);
                }
                else
                {
                    spawnPosition = slime.transform.position + new Vector3(detectionRange, 0f, 0f);
                    GameObject thunder = Instantiate(skillPrefab, spawnPosition, Quaternion.identity);
                    Destroy(thunder, 1f);
                }
                yield return new WaitForSeconds(0.5f);
            }

            yield return StartCoroutine(Cooldown());

        }
    }
}