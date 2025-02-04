using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Meteor : BaseSkill
{
    public ParticleSystem bombParticle;
    private float meteorSpeed = 10f;

    protected override void Awake()
    {
        base.Awake();
        detectionRange = 3.9f;
        cooldown = 3f;
    }

    protected override IEnumerator SkillCoroutine()
    {
        while (true)
        {
            cooldownImage.fillAmount = 1f;

            Monster target = FindNearestMonsterInRange();
            Vector3 targetPosition;

            if (target != null)
            {
                targetPosition = target.transform.position;
            }
            else
            {
                targetPosition = slime.transform.position + new Vector3(detectionRange, 0f, 0f);
            }

            GameObject meteor = Instantiate(skillPrefab, skillPrefab.transform.position, Quaternion.identity);
            StartCoroutine(MoveMeteor(meteor, targetPosition));
            Destroy(meteor, 2f);

            yield return StartCoroutine(Cooldown());
        }
    }

    private IEnumerator MoveMeteor(GameObject meteor, Vector3 targetPosition)
    {
        Vector3 startPos = meteor.transform.position;
        float journeyLength = Vector3.Distance(startPos, targetPosition);
        float startTime = Time.time;

        while (meteor != null)
        {
            float distanceCovered = (Time.time - startTime) * meteorSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            if (fractionOfJourney >= 1f)
            {
                if (meteor != null)
                {
                    Collider2D[] hits = Physics2D.OverlapCircleAll(targetPosition, 1f);
                    foreach (Collider2D hit in hits)
                    {
                        if (hit.TryGetComponent<Monster>(out Monster monster))
                        {
                            float distanceX = Mathf.Abs(monster.transform.position.x - slime.transform.position.x);
                            if (distanceX <= detectionRange)
                            {
                                monster.TakeDamage(slime.damage * 12f);
                                var particle = Instantiate(bombParticle, monster.transform.position, Quaternion.identity);
                                particle.Play();
                                Destroy(particle.gameObject, 1f);
                            }
                        }
                    }
                    Destroy(meteor);
                }
                break;
            }

            if (meteor != null)
            {
                meteor.transform.position = Vector3.Lerp(startPos, targetPosition, fractionOfJourney);
                meteor.transform.Rotate(0, 0, 360 * Time.deltaTime);
            }

            yield return null;
        }
    }
}