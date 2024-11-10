using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    private int monsterCount = 10;

    public GameObject monsterPrefab; // 적 프리팹
    public float spawnInterval = 3f; 
    private float plusHp = 0.1f;
    private float plusDamage = 1f;
    private int waveCount = 0;  

    private float spawnStartX = 3.5f; // 첫 번째 몬스터 스폰 위치
    public float monsterSpacing = 0.5f; // 몬스터 간 간격

    private float baseHp;
    private float baseDamage;
    
    private void Start()
    {
        Monster monsterbase = monsterPrefab.GetComponent<Monster>();
        baseHp = monsterbase.hp;
        baseDamage = monsterbase.damage;
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

            if (monsters.Length == 0)
            {
                waveCount++;
                Spawn(monsterCount); 
            }

            yield return new WaitForSeconds(spawnInterval); 
        }
    }

    private void Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float y = Random.Range(0.45f, 0.5f);
            float x = spawnStartX + (i * monsterSpacing);

            Vector2 spawnPos = new Vector2(x, y);
            GameObject monsterObject = Instantiate(monsterPrefab, spawnPos, Quaternion.identity);

            Monster monster = monsterObject.GetComponent<Monster>();
            if (monster != null)
            {
                if (waveCount == 1)
                {
                    // 1웨이브는 기본 스탯
                    monster.SetHp(baseHp);
                    monster.SetDamage(baseDamage);
                }
                else
                {
                    // 2웨이브부터는 증가율 적용 (waveCount - 1을 사용하여 계산)
                    float hp = baseHp * (1 + (waveCount - 1) * plusHp);
                    float damage = baseDamage * (1 + (waveCount - 1) * plusDamage);
                    monster.SetHp(hp);
                    monster.SetDamage(damage);
                }

            }
        }
    }

    public void ResetMonsterStats()
    {
        waveCount = 0;
    }
}
