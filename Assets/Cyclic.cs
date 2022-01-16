using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclic : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float theta = 0;
    public bool showCosX = false;
    public bool showSinY = false;

    [Header("Set Dynamically")]
    public Vector3 pos;

    void Update()
    {
        // ��������� �������� �������� �� �������� �������
        float radians = Time.time * Mathf.PI;

        // ������������� ������� � ������� ��� ����������� � ����������
        // �������� "% 360" ������������ �������� ���������� 0 .. 359.9999
        theta = Mathf.Round(radians * Mathf.Rad2Deg) % 360;

        // ���������� �������� �������
        pos = Vector3.zero;

        // ��������� x � y ��� ������� � ����� ���� ��������������
        pos.x = Mathf.Cos(radians);
        pos.y = Mathf.Sin(radians);

        // ������������ ����� � �������, ����
        // ��������������� ������ ����������� � ����������
        Vector3 tPos = Vector3.zero;
        if (showCosX) tPos.x = pos.x;
        if (showSinY) tPos.y = pos.y;

        // ��������������� this.gameObject (Sphere)
        transform.position = tPos;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        // ���������� ������� ��������� �����
        // (������ ������ ���������� ���� ���� for)

        int inc = 10;

        for (int i=0; i<360; i += inc)
        {

        }
    }
}
