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
    private int waveCount = 0;
    private int wavesSinceReset = 0;

    private float spawnStartX = 4f; // 첫 번째 몬스터 스폰 위치
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
                wavesSinceReset++;
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
                if (wavesSinceReset == 1)
                {
                    monster.SetHp(baseHp);
                    monster.SetDamage(baseDamage);
                }
                else
                {

                    float newHp = baseHp * (1 + ((wavesSinceReset - 1) * 0.1f));
                    float newDamage = baseDamage * (1 + ((wavesSinceReset - 1) * 1f));
                    monster.SetHp(newHp);
                    monster.SetDamage(newDamage);
                }
            }
        }
    }

    public void ResetMonsterStats()
    {
        wavesSinceReset = 0;
    }
}
