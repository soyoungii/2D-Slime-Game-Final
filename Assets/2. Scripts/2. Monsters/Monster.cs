using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Tooltip("캐릭터에게 주는 피해량")]
    public float damage = 1; //공격력
    [Tooltip("몬스터 체력")]
    public float hp = 1f; //체력
    public float moveSpeed; //이동속도

    private Transform target; //추적 대상(슬라임)
    private Rigidbody2D rb;
    public ParticleSystem monsterDieParticle;

    public GameObject monsterProjectilePrefab;
    Slime slime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        //GameManager.Instance.enemies.Add(this);
        target = GameObject.FindGameObjectWithTag("Player").transform;
}

    private void Update()
    {
        MonsterMove();

    }

    public void SetHp(float newHp)
    {
        hp = newHp;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
    public void MonsterMove()
    {
        Vector3 direction = target.position - transform.position;
        direction.Normalize();

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void StopMonster()
    {
        moveSpeed = 0f;
        //Vector3.Distance(transform.position, target.position);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    public void SlimeDie()
    {
        damage = 1;
        hp = 1;
    }

    public void Die()
    {
        Destroy(gameObject);
        GameManager.Instance.slime.gold += 10;
        var particle = Instantiate(monsterDieParticle, transform.position, Quaternion.identity);
        particle.Play();
        Destroy(particle.gameObject, 2f);
        //LeanPool.Despawn(gameObject); -> leanpool 사용시 이걸로 바꾸기
    }

    private IEnumerator Projectile()
    {
        while (true)
        {
            GameObject projectile = Instantiate(monsterProjectilePrefab, monsterProjectilePrefab.transform.position, Quaternion.identity);
            MonsterProjectile monsterProjectile = projectile.GetComponent<MonsterProjectile>();
            if (monsterProjectile != null)
            {
                monsterProjectile.SetMonster(this);  
            }

            yield return new WaitForSeconds(3f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SlimeMonDist"))
        {
            StopMonster();
            StartCoroutine(Projectile());

        }
    }


}