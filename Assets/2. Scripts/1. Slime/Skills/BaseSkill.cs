using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class BaseSkill : MonoBehaviour
{
    [SerializeField] protected GameObject skillPrefab;
    [SerializeField] protected Image cooldownImage;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float detectionRange = 4f;
    
    protected Slime slime;
    protected bool isActive;

    protected virtual void Awake()
    {
        slime = FindObjectOfType<Slime>();
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

    protected Monster FindNearestMonsterInRange()
    {
        Monster nearestMonster = null;
        float nearestDistance = float.MaxValue;

        foreach (Monster monster in FindObjectsOfType<Monster>())
        {
            float distanceX = Mathf.Abs(monster.transform.position.x - slime.transform.position.x);

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