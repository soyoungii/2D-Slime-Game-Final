using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StarlightProjectile : MonoBehaviour
{
    private bool hashit = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hashit) return;

        if (other.CompareTag("Monster"))
        {
            hashit = true;
            float damage = GameManager.Instance.slime.damage * 1.5f;
            other.TryGetComponent<Monster>(out Monster monster);
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
