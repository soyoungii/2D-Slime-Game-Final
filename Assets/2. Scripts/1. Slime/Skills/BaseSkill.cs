using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseSkill : MonoBehaviour
{
    [SerializeField] protected GameObject skillPrefab;
    [SerializeField] protected Image cooldownImage;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float detectionRange = 4f;
    
    protected Slime slime;
    protected bool isActive;

    protected List<Monster> monsterList = new List<Monster>();
        
    protected virtual void Awake()
    {
        slime = FindObjectOfType<Slime>();
        StartCoroutine(UpdateMonsterListRoutine());

    }

    public void StartSkill()
    {
        if (!isActive) 
        {
            isActive = true;
            StartCoroutine(SkillCoroutine());
        }
    }
    protected abstract IEnumerator SkillCoroutine();

    protected IEnumerator Cooldown()
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
    
    protected IEnumerator UpdateMonsterListRoutine()
    { 
        while (true)
        {
            UpdateMonsterList();
            yield return new WaitForSeconds(1f);
        }
    }
    
    // 몬스터 리스트 갱신
    protected void UpdateMonsterList()
    {
        monsterList.Clear();
        monsterList.AddRange(FindObjectsOfType<Monster>());
    }

    // 최적화된 가까운 몬스터 찾기
    protected Monster FindNearestMonsterInRange()
    {
        // 1. 결과값을 저장할 변수 초기화
        Monster nearestMonster = null;
        float nearestDistance = float.MaxValue;
    
        // 2. 캐싱된 몬스터 리스트 사용
        foreach (Monster monster in monsterList)
        {
            if (monster == null) continue;  // 파괴된 몬스터 체크
    
            // 3. X축 거리 계산
            float distanceX = Mathf.Abs(monster.transform.position.x - slime.transform.position.x);
    
            // 4. 감지 범위 체크
            if (distanceX <= detectionRange)
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