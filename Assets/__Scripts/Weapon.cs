using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ������������ ���� ��������� ����� ������.
/// ����� �������� ��� "shield", ����� ���� ����������� ���������������� ������.
/// ������������� [HP] ���� �������� ��������, �� �������������.
/// </summary>

public enum WeaponType
{
    none,           // �� ��������� / ��� ������
    blaster,        // ������� �������
    spread,         // ������� �����, ���������� ����������� ���������
    phaser,         // [HP] �������� �����
    missile,        // [HP] ��������������� ������
    laser,          // [HP] ������� ����������� ��� �������������� �����������
    shield          // ����������� shieldLevel
}


/// <summary>
/// ����� WeaponDefinition ��������� ����������� ��������
/// ����������� ���� ������ � ����������. ��� ����� ����� Main
/// ����� ������� ������ ��������� ���� WeaponDefinition.
/// </summary>
[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter; // ����� �� ������, ������������ �����
    public Color color = Color.white; // ���� ������ ������ � ������ ������
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float damageOnHit = 0; // �������������� ��������
    public float continuousDamage = 0; //������� ���������� � ������� (��� Laser)
    public float delayBetweenShots = 0;
    public float velocity = 20; // �������� ������ ��������
}

public class Weapon : MonoBehaviour
{

}