using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    // Enemy_3 ��������� ����� Enemy
    // ���������� �������� Enemy_3 ����������� ����� ��������
    // ������������ ������ ����� �� ����� ��� ���� ������.

    [Header("Set in Inspector: Enemy_3")]
    public float lifeTime = 5;

    [Header("Set Dynamically: Enemy_3")]
    public Vector3[] points;
    public float birthTime;

    private void Start()
    {
        points = new Vector3[3];

        // ��������� ������� ��� ���������� � Main.SpawnEnemy()
        points[0] = pos;

        // ���������� xMin � ���� ��� ��, ��� ��� ������ Main.SpawnEnemy()
        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;

        Vector3 v;
        // �������� ������� ������� ����� ���� ������ ������� ������
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax);
        v.y = -bndCheck.camHeight + Random.Range(2.75f, 2);
        points[1] = v;

        // �������� ������� �������� ����� ���� ������� ������� ������
        v = Vector3.zero;
        v.y = pos.y;
        v.x = Random.Range(xMin, xMax);
        points[2] = v;

        birthTime = Time.time;
    }

    public override void Move()
    {
        // ������ ����� ����������� �� ������ �������� � ����� 0 � 1
        float u = (Time.time - birthTime) / lifeTime;

        if ( u > 1)
        {
            // ���� ��������� Enemy_2 �������� ���� ��������� ����
            Destroy(this.gameObject);
            return;
        }

        // ��������������� ������ ����� �� ���� ������
        Vector3 p01, p12;
        u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
        p01 = (1 - u) * points[0] + u * points[1];
        p12 = (1 - u) * points[1] + u * points[2];
        pos = (1 - u) * p01 + u * p12;
    }

}
