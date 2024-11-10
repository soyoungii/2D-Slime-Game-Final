using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SphereProjecitle : MonoBehaviour
{
    private float sphereSpeed = 0.6f;
    private float pierce = 10;

    private float stopPosition = 3.2f;
    private Vector2 startPosition;

    Slime slime;

    private void Awake()
    {
        slime = FindObjectOfType<Slime>();
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector2.right * sphereSpeed * Time.deltaTime);
        float distance = Vector2.Distance(transform.position, startPosition);
        if(distance >= stopPosition)
        {
            sphereSpeed = 0;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Monster>(out Monster monster))
        {
            float damage = slime.damage * 1.2f;
            monster.TakeDamage(damage);
            pierce--;
            if (pierce <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
