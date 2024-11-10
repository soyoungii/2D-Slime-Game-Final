using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float speed = 0.4f;
    public float detectionRange = 4.5f; // ���� ���� ����
    public float resetPosition = -3.5f; // ����� ���µ� ��ġ
    public float startPosition = 13;   // ��� ���� ��ġ

    private Slime slime;
    private bool isMoving = true;

    private void Start()
    {
        slime = FindObjectOfType<Slime>();

    }
    private void Update()
    {
        // ���� �� ���� Ȯ��
        bool monsterInRange = CheckForMonstersInRange();

        // ���Ͱ� ���� ���� ���� ���� �̵�
        if (!monsterInRange)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            // ����� ������ �̵��ߴ��� Ȯ��
            if (transform.position.x <= resetPosition)
            {
                // ���� ��ġ�� ����
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
                return true; // ���� ���� ���Ͱ� ����
            }
        }
        return false; // ���� ���� ���Ͱ� ����
    }
}
