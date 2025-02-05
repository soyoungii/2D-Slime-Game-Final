using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Slime : MonoBehaviour
{
    [Header("기본 스탯")]
    public float damage = 1; //공격력 -> 레벨당 1 증가

    public float maxHp = 5; //최대 체력 -> 레벨당 5 증가
    public float currentHp; //현재체력
    public float hpRecover = 0; //체력 회복량 -> 레벨당 0.6 증가

    public float critical = 0; //치명타 확률 -> Max값: 100 -> 레벨당 1%증가
    public float criticalDamage = 100; //치명타 피해 -> 레벨당 1% 증가

    public float attackSpeed = 1; //공격속도 레벨당 0.1 증가 -> 레벨당 0.1 증가 / 레벨 2가되면 1초에 2번
    public float doubleShot = 0; //더블샷 -> 관통 횟수도 설정? -> 레벨당 1%증가

    public float gold = 0;

    [Header("스탯 레벨")]
    private int damageLevel = 0;
    private int hpLevel = 0;
    private int hpRecoverLevel = 0;
    private int criticalLevel = 0;
    private int criDamLevel = 0;
    private int atkSpeedLevel = 0;
    private int dShotLevel = 0;

    [Header("파티클")]
    public ParticleSystem hpRecoverParticle;
    public ParticleSystem levelupParticle;
    public ParticleSystem reviveSlime;
    public ParticleSystem hitParticle;

    [Header("투사체")]
    public GameObject projectilePrefab;
    private float findRange = 4f;
    private float projectileSpeed = 10f;
    private float nextFireTime;

    private Vector3 respawnPosition;
    private Spawner spawner;

    private void Awake()
    {
        spawner = FindObjectOfType<Spawner>(); 

    }
    private void Start()
    {
        currentHp = maxHp;
    }
    private void Update()
    {
        UIManager.Instance.myGold.text = gold.ToString();
        UIManager.Instance.myDamage.text = damage.ToString();
        UIManager.Instance.myHp.text = currentHp.ToString();

        UIManager.Instance.valueText[0].text = damage.ToString();
        UIManager.Instance.valueText[1].text = maxHp.ToString();
        UIManager.Instance.valueText[2].text = hpRecover.ToString();
        UIManager.Instance.valueText[3].text = critical + "%".ToString();
        UIManager.Instance.valueText[4].text = criticalDamage + "%".ToString();
        UIManager.Instance.valueText[5].text = attackSpeed.ToString();
        UIManager.Instance.valueText[6].text = doubleShot + "%".ToString();


        UIManager.Instance.level[0].text = damageLevel.ToString();
        UIManager.Instance.level[1].text = hpLevel.ToString();
        UIManager.Instance.level[2].text = hpRecoverLevel.ToString();
        UIManager.Instance.level[3].text = criticalLevel.ToString();
        UIManager.Instance.level[4].text = criDamLevel.ToString();
        UIManager.Instance.level[5].text = atkSpeedLevel.ToString();
        UIManager.Instance.level[6].text = dShotLevel.ToString();

        UIManager.Instance.gold[0].text = ((damageLevel + 1) * 10).ToString();
        UIManager.Instance.gold[1].text = ((hpLevel + 1) * 10).ToString();
        UIManager.Instance.gold[2].text = ((hpRecoverLevel + 1) * 10).ToString();
        UIManager.Instance.gold[3].text = ((criticalLevel + 1) * 10).ToString();
        UIManager.Instance.gold[4].text = ((criDamLevel + 1) * 10).ToString();
        UIManager.Instance.gold[5].text = ((atkSpeedLevel + 1) * 10).ToString();
        UIManager.Instance.gold[6].text = ((dShotLevel + 1) * 10).ToString();



        if (Time.time >= nextFireTime)
        {
            AttackNearestMonster();
        }
    }

    private IEnumerator AutoAttackRoutine()
    {
        while (true)
        {
            if (Time.time >= nextFireTime)
            {
                AttackNearestMonster();
            }
            yield return null;
        }
    }

    private void AttackNearestMonster()
    {
        Monster nearestMonster = null;
        float nearestDistance = findRange;

        foreach (Monster monster in FindObjectsOfType<Monster>())
        {
            float distance = Vector2.Distance(transform.position, monster.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestMonster = monster;
            }
        }

        if (nearestMonster != null)
        {
            FireProjectile(nearestMonster.transform.position);
            nextFireTime = Time.time + (1f / attackSpeed);
        }
    }

    private void FireProjectile(Vector3 targetPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, projectilePrefab.transform.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();

        // 크리티컬 계산
        bool isCritical = Random.Range(0f, 100f) < critical;
        float finalDamage = damage;
        if (isCritical)
        {
            finalDamage += damage * (criticalDamage / 100f);
            Debug.Log($"크리티컬! 몬스터가 {finalDamage}의 피해를 입음");
        }

        projectileScript.Initialize(transform.position, targetPosition, projectileSpeed, finalDamage);

        // 더블샷 처리
        if (Random.Range(0f, 100f) < doubleShot)
        {
            StartCoroutine(FireSecondProjectile(targetPosition, finalDamage));
            Debug.Log("더블샷 터짐");
        }
    }

    private IEnumerator FireSecondProjectile(Vector2 targetPosition, float damage)
    {
        yield return new WaitForSeconds(0.1f);
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.Initialize(transform.position, targetPosition, projectileSpeed, damage);
    }


    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        if(currentHp > 0)
        {
            var Hitslime = Instantiate(hitParticle, hitParticle.transform.position, quaternion.identity);
            Hitslime.Play();
            Destroy(Hitslime.gameObject, 1f);
        }
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (spawner != null)
        {
           spawner.ResetMonsterStats();
        }

        currentHp = maxHp;
        var particle = Instantiate(reviveSlime, reviveSlime.transform.position, quaternion.identity);
        particle.Play();
        Destroy(particle.gameObject, 1f);
    }
  
    public void DamageLevelUp()
    {
        if (gold >= (damageLevel + 1) * 10)
        {
            damage += 1;
            gold -= (damageLevel + 1) * 10;
            damageLevel++;
            LevelUpEffect();
        }

        else
        {
            UIManager.Instance.upgradeNoGold.SetActive(true);
        }
    }

    public void HpLevelUp()
    {
        if (gold >= (hpLevel + 1) * 10)
        {
            maxHp += 5;
            gold -= (hpLevel + 1) * 10;
            hpLevel++;
            LevelUpEffect();
        }

        else
        {
            UIManager.Instance.upgradeNoGold.SetActive(true);
        }
    }

    public void HpRecoverLevelUp()
    {
        if (gold >= (hpRecoverLevel + 1) * 10)
        {
            hpRecover += 0.6f;
            gold -= (hpRecoverLevel + 1) * 10;
            hpRecoverLevel++;
            LevelUpEffect();
            StartCoroutine(HpRecoveryStart());
        }

        else
        {
            UIManager.Instance.upgradeNoGold.SetActive(true);
        }
    }

    public void CriticalLevelUp()
    {
        if (gold >= (criticalLevel + 1) * 10)
        {
            critical = Mathf.Min(100f, critical + 1f); 
            gold -= (criticalLevel + 1) * 10;
            criticalLevel++;
            LevelUpEffect();
        }
        else
        {
            UIManager.Instance.upgradeNoGold.SetActive(true);
        }
    }

    public void criDamLevelUp()
    {
        if (gold >= (criDamLevel + 1) * 10)
        {
            criticalDamage += 1;
            gold -= (criDamLevel + 1) * 10;
            criDamLevel++;
            LevelUpEffect();
        }

        else
        {
            UIManager.Instance.upgradeNoGold.SetActive(true);
        }
    }

    public void AttackSpeedLevelUp()
    {
        if (gold >= (atkSpeedLevel + 1) * 10)
        {
            attackSpeed += 0.1f;
            gold -= (atkSpeedLevel + 1) * 10;
            atkSpeedLevel++;
            LevelUpEffect();
        }
        else
        {
            UIManager.Instance.upgradeNoGold.SetActive(true);
        }
    }

    public void DoubleShotLevelUp()
    {
        if (gold >= (dShotLevel + 1) * 10)
        {
            doubleShot = Mathf.Min(100f, doubleShot + 1f);
            gold -= (dShotLevel + 1) * 10;
            dShotLevel++;
            LevelUpEffect();
        }
        else
        {
            UIManager.Instance.upgradeNoGold.SetActive(true);
        }
    }

    private IEnumerator HpRecoveryStart()
    {
        while (true)
        {
            if (currentHp < maxHp)
            {
                currentHp = Mathf.Min(maxHp, currentHp + hpRecover);
                if (hpRecoverParticle != null)
                {
                    var hpUp = Instantiate(hpRecoverParticle, hpRecoverParticle.transform.position, Quaternion.identity);
                    hpUp.Play();
                    Destroy(hpUp.gameObject, 1f);
                }
            }
            yield return new WaitForSeconds(5f);
        }
    }

    private void LevelUpEffect()
    {
        var particle = Instantiate(levelupParticle, transform.position, Quaternion.identity);
        particle.Play();
        Destroy(particle.gameObject, 2f);
    }
}




