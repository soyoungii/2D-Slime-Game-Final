using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Starlight : BaseSkill
{
    private float attackRange = 4f;
    private float projectileSpeed = 5f;
    private int projectileCount = 10;
    private float spawnInterval = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        cooldown = 7f;
    }

    protected override IEnumerator SkillCoroutine()
    {
        while (true)
        {
            cooldownImage.fillAmount = 1f;

            for (int i = 0; i < projectileCount; i++)
            {
                Monster target = FindNearestMonsterInRange();
                GameObject starlight = Instantiate(skillPrefab, skillPrefab.transform.position, Quaternion.identity);

                if (target != null)
                {
                    StartCoroutine(MoveProjectile(starlight, target));
                }
                else
                {
                    Vector3 targetPosition = slime.transform.position + new Vector3(attackRange, 0f, 0f);
                    StartCoroutine(MoveProjectileToPosition(starlight, targetPosition));
                }

                yield return new WaitForSeconds(spawnInterval);
            }
            
            yield return StartCoroutine(Cooldown()); 
        }
    }

    private IEnumerator MoveProjectile(GameObject projectile, Monster firstTarget)
    {
        while (projectile != null)
        {
            if (firstTarget == null)
            {
                Destroy(projectile); 
                yield break;
            }

            float distanceX = Mathf.Abs(firstTarget.transform.position.x - slime.transform.position.x);
            if (distanceX > attackRange)
            {
                if (firstTarget == null)
                {
                    Destroy(projectile);
                    yield break;
                }
            }

            Vector3 direction = (firstTarget.transform.position - projectile.transform.position).normalized;
            projectile.transform.position += direction * projectileSpeed * Time.deltaTime;

            float distanceToTarget = Vector3.Distance(projectile.transform.position, firstTarget.transform.position);
            if (distanceToTarget < 0.1f)
            {
                if (firstTarget != null)
                {
                    firstTarget.TakeDamage(slime.damage * 1.5f);
                }
                Destroy(projectile);
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator MoveProjectileToPosition(GameObject projectile, Vector3 targetPosition)
    {
        while (projectile != null)
        {
            Vector3 direction = (targetPosition - projectile.transform.position).normalized;
            projectile.transform.position += direction * projectileSpeed * Time.deltaTime;

            float distanceToTarget = Vector3.Distance(projectile.transform.position, targetPosition);
            if (distanceToTarget < 0.1f)
            {
                Destroy(projectile);
                yield break;
            }

            yield return null;
        }
    }
}


