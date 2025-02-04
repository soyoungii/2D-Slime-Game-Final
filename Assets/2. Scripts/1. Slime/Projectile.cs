using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Projectile : MonoBehaviour
{
    public float rotateSpeed = 350f; // 투사체 회전 속도
    public float baseArcHeight = 2f; // 기본 포물선 높이

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float projectileSpeed;
    private float damage;
    private bool hasHit = false;
    private float arcHeight;

    private float elapsedTime = 0f;
    private float totalTime;

    public void Initialize(Vector2 start, Vector2 target, float speed, float damageAmount)
    {
        startPosition = start;
        targetPosition = target;
        projectileSpeed = speed;
        damage = damageAmount;

        // 거리 계산
        float distance = Vector2.Distance(start, target);

        // 거리에 따른 이동 시간 계산
        totalTime = distance / speed;

        // 가까울수록 낮게, 멀수록 높게(최소 1, 최대 4)
        arcHeight = Mathf.Clamp(distance * 0.2f, 0.2f, 0.5f) * baseArcHeight;
    }

    private void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

        if (hasHit) return;

        elapsedTime += Time.deltaTime;
        float progress = elapsedTime / totalTime;

        if (progress >= 1f)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 currentPos = Vector2.Lerp(startPosition, targetPosition, progress);

        // 포물선 높이 계산
        float heightCurve = Mathf.Sin(progress * Mathf.PI);
        float heightOffset = arcHeight * heightCurve;

        currentPos.y += heightOffset;
        transform.position = currentPos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        if (other.CompareTag("Monster"))
        {
            hasHit = true;
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}