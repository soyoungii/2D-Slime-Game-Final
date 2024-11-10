using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterProjectile : MonoBehaviour
{
    private float projectileSpeed = 2f;
    private float damage;
    Monster monster;

    public void SetMonster(Monster monsterRef)
    {
        monster = monsterRef;
    }

    private void Update()
    {
        transform.Translate(Vector2.left * projectileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.slime.TakeDamage(monster.damage);
            Destroy(gameObject);
        }
    }
}
