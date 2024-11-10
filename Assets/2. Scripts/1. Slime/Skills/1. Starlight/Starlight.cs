using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Starlight : MonoBehaviour
{
    public GameObject starlightPrefab;
    public Image cooldownImage;
    private float cooldown = 7f;
    private Slime slime;

    private float attackRange = 4f;
    private float projectileSpeed = 5f;
    private int projectileCount = 10;
    private float spawnInterval = 0.5f;

    private void Awake()
    {
        slime = FindObjectOfType<Slime>();
    }

    public void StartSkill()
    {
        StartCoroutine(StarlightCoroutine());
    }

    private IEnumerator StarlightCoroutine()
    {
        while (true)
        {
            cooldownImage.fillAmount = 1f;

            for (int i = 0; i < projectileCount; i++)
            {
                Monster target = FindNearestMonsterInRange(); 
                if (target != null)
                {
                    GameObject starlight = Instantiate(starlightPrefab, starlightPrefab.transform.position, Quaternion.identity);
                    StartCoroutine(MoveProjectile(starlight, target));
                }

                yield return new WaitForSeconds(spawnInterval);
            }

            yield return StartCoroutine(Cooldown());
        }
    }


    private IEnumerator MoveProjectile(GameObject projectile, Monster fistTarget)
    {
        while (projectile != null)
        {
            if (fistTarget == null)
            {
                fistTarget = FindNearestMonsterInRange(); 
                if (fistTarget == null)
                {
                    Destroy(projectile);
                    yield break;
                }
            }

            float distanceX = Mathf.Abs(fistTarget.transform.position.x - slime.transform.position.x);
            if (distanceX > attackRange)
            {
                fistTarget = FindNearestMonsterInRange();
                if (fistTarget == null)
                {
                    Destroy(projectile);
                    yield break;
                }
            }
            Vector3 direction = (fistTarget.transform.position - projectile.transform.position).normalized;
            projectile.transform.position += direction * projectileSpeed * Time.deltaTime;

            float distanceToTarget = Vector3.Distance(projectile.transform.position, fistTarget.transform.position);
            if (distanceToTarget < 0.1f)
            {
                if (fistTarget != null)
                {
                    fistTarget.TakeDamage(slime.damage * 1.5f);
                }
                Destroy(projectile);
                yield break;
            }

            yield return null;
        }
    }
    private IEnumerator Cooldown()
    {
        float elapsed = 0f;
        while (elapsed < cooldown)
        {
            elapsed += Time.deltaTime;
            if (cooldownImage != null)
            {
                cooldownImage.fillAmount = 1f - (elapsed / cooldown);
            }
            yield return null;
        }
    }


    private Monster FindNearestMonsterInRange()
    {
        Monster nearestMonster = null;
        float nearestDistance = float.MaxValue;

        foreach (Monster monster in FindObjectsOfType<Monster>())
        {
            float distanceX = Mathf.Abs(monster.transform.position.x - slime.transform.position.x);

            if (distanceX <= attackRange)
            {
                float distance = Vector2.Distance(slime.transform.position, monster.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestMonster = monster;
                }
            }
        }

        return nearestMonster;
    }

}


