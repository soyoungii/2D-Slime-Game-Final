using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float speed = 0.4f;
    public float detectionRange = 4.5f; // 몬스터 감지 범위
    public float resetPosition = -3.5f; // 배경이 리셋될 위치
    public float startPosition = 13;   // 배경 시작 위치

    private Slime slime;
    private bool isMoving = true;

    private void Start()
    {
        slime = FindObjectOfType<Slime>();

    }
    private void Update()
    {
        bool monsterInRange = CheckForMonstersInRange();
        if (!monsterInRange)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= resetPosition)
            {
                transform.position = new Vector3(startPosition, transform.position.y, transform.position.z);
            }
        }
    }

    private bool CheckForMonstersInRange()
    {
        if (slime == null) return false;

        foreach (Monster monster in FindObjectsOfType<Monster>())
        {
            float distanceX = Mathf.Abs(monster.transform.position.x - slime.transform.position.x);
            if (distanceX <= detectionRange)
            {
                return true; 
            }
        }
        return false;
    }
}
