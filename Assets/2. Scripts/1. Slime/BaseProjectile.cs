using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;
    
    protected bool hasHit;
    protected Vector2 startPosition;
    protected Vector2 targetPosition;

    public virtual void Initialize(Vector2 start, Vector2 target, float damageAmount)
    {
        startPosition = start;
        targetPosition = target;
        damage = damageAmount;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        if (other.CompareTag("Monster"))
        {
            hasHit = true;
            if (other.TryGetComponent<Monster>(out Monster monster))
            {
                monster.TakeDamage(damage);
            }
            OnProjectileHit();
        }
    }

    protected virtual void OnProjectileHit()
    {
        Destroy(gameObject);
    }
} 