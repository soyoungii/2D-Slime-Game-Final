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
    private int damageLevel = 1;
    private int hpLevel = 1;
    private int hpRecoverLevel = 1;
    private int criticalLevel = 1;
    private int criDamLevel = 1;
    private int atkSpeedLevel = 1;
    private int dShotLevel = 1;

    [Header("파티클")]
    public ParticleSystem hpRecoverParticle;
    public ParticleSystem levelupParticle;
    public ParticleSystem reviveSlime;

    [Header("투사체")]
    public GameObject projectilePrefab;
    private float detectionRange = 4f; // 탐지 범위
    private float projectileSpeed = 10f;
    private float nextFireTime;

    private Vector3 respawnPosition;
    private Spawner spawner;

    private void Start()
    {
        currentHp = maxHp;
    }
    private void Update()
    {
        UIManager.Instance.gold.text = gold.ToString();
        UIManager.Instance.myDamage.text = damage.ToString();
        UIManager.Instance.myHp.text = currentHp.ToString();

        UIManager.Instance.damageText.text = damage.ToString();
        UIManager.Instance.hpText.text = maxHp.ToString();
        UIManager.Instance.hpRecoverText.text = hpRecover.ToString();
        UIManager.Instance.criticalText.text = critical.ToString();
        UIManager.Instance.criDamText.text = criticalDamage.ToString();
        UIManager.Instance.atkSpeedText.text = attackSpeed.ToString();
        UIManager.Instance.dShotText.text = doubleShot.ToString();


        UIManager.Instance.damageLevel.text = damageLevel.ToString();
        UIManager.Instance.hpLevel.text = hpLevel.ToString();
        UIManager.Instance.hpRecoverLevel.text = hpRecoverLevel.ToString();
        UIManager.Instance.criticalLevel.text = criticalLevel.ToString();
        UIManager.Instance.criDamLevel.text = criDamLevel.ToString();
        UIManager.Instance.atkSpeedLevel.text = atkSpeedLevel.ToString();
        UIManager.Instance.dShotLevel.text = dShotLevel.ToString();

        UIManager.Instance.damageGold.text = ((damageLevel + 1) * 10).ToString();
        UIManager.Instance.hpGold.text = ((hpLevel + 1) * 10).ToString();
        UIManager.Instance.hpRecoverGold.text = ((hpRecoverLevel + 1) * 10).ToString();
        UIManager.Instance.criticalGold.text = ((criticalLevel + 1) * 10).ToString();
        UIManager.Instance.criDamGold.text = ((criDamLevel + 1) * 10).ToString();
        UIManager.Instance.atkSpeedGold.text = ((atkSpeedLevel + 1) * 10).ToString();
        UIManager.Instance.dShotGold.text = ((dShotLevel + 1) * 10).ToString();



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
        float nearestDistance = detectionRange;

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

        Monster[] allMonsters = FindObjectsOfType<Monster>();
        foreach (Monster monster in allMonsters)
        {
            monster.SlimeDie();
        }

        currentHp = maxHp;
        var particle = Instantiate(reviveSlime, reviveSlime.transform.position, quaternion.identity);
        particle.Play();
        Destroy(particle.gameObject, 2f);
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
            critical = Mathf.Min(100f, critical + 1f); // 최대 100%로 제한
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
            doubleShot = Mathf.Min(100f, doubleShot + 1f); // 최대 100%로 제한
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

    public void StarlightUnlock()
    {
        if(gold >= 20)
        {
            Destroy(UIManager.Instance.lockStarlight.gameObject);
            Destroy(UIManager.Instance.starlightUnlock.gameObject);
            //스타라이트 스킬 코루틴 시작
        }

       else
        {
            UIManager.Instance.skillNoGold.SetActive(true);
        }
    }

    public  void SphereUnlock()
    {
        if(gold>=10)
        {
            Destroy(UIManager.Instance.lockSphere.gameObject);
            Destroy(UIManager.Instance.sphereUnlock.gameObject);
            //보이드 스킬 코루틴 시작
        }

        else
        {
            UIManager.Instance.skillNoGold.SetActive(true);
        }
    }

    public void MeteorUnlock()
    {
        if(gold>=30)
        {
            Destroy(UIManager.Instance.lockMeteor.gameObject);
            Destroy(UIManager.Instance.meteorUnlock.gameObject);
            //메테오 스킬 코루틴 시작
        }

        else
        {
            UIManager.Instance.skillNoGold.SetActive(true);
        }
    }

    public void ThunderUnlock()
    {
        if (gold >= 40)
        {
            Destroy(UIManager.Instance.lockThunder.gameObject);
            Destroy(UIManager.Instance.thunderUnlock.gameObject);
            //벼락 스킬 코루틴 시작
        }

        else
        {
            UIManager.Instance.skillNoGold.SetActive(true);
        }
    }

    public void AngerUnlock()
    {
        if (gold >= 10)
        {
            Destroy(UIManager.Instance.lockAnger.gameObject);
            Destroy(UIManager.Instance.angerUnlock.gameObject);
            //분노 스킬 코루틴 시작
        }

        else
        {
            UIManager.Instance.skillNoGold.SetActive(true);
        }
    }
}




