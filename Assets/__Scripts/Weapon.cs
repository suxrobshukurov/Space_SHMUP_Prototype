using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Это перечисление всех возможных типов оружия.
/// Также включает тип "shield", чтобы дать возможность совершенствовать защиту.
/// Аббревиатурой [HP] ниже отмечены элементы, не реализованные.
/// </summary>

public enum WeaponType
{
    none,           // По умолчанию / нет оружия
    blaster,        // Простой бластер
    spread,         // Веерная пушка, стреляющая несколькими снарядами
    phaser,         // [HP] Волновой фазер
    missile,        // [HP] Самонаводящиеся ракеты
    laser,          // [HP] Наносит повреждения при долговременном воздействии
    shield          // Увеличивает shieldLevel
}


/// <summary>
/// Класс WeaponDefinition позволяет настраивать свойства
/// конкретного вида оружия в инспекторе. Для этого класс Main
/// будет хранить массив элементов типа WeaponDefinition.
/// </summary>
[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter; // Буква на кубике, изображающем бонус
    public Color color = Color.white; // Цвет ствола оружия и кубика бонуса
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float damageOnHit = 0; // Разрушительная мощность
    public float continuousDamage = 0; //Степень разрушения в секунду (для Laser)
    public float delayBetweenShots = 0;
    public float velocity = 20; // Скорость полета снарядов
}

public class Weapon : MonoBehaviour
{

}