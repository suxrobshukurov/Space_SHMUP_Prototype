using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    [Header("Set in Inspector: Enemy_1")]
    public float waveFrequency = 2; // число секунд полного цикла синусоиды
    public float waveWidth = 4; //шиирина синусоида в метрах
    public float waveRotY = 45;

    private float x0; // начальное значение координата x
    private float birthTime;

    private void Start()
    {
        // Установить начальную координату X объекта Enemy_l
        x0 = pos.x;

        birthTime = Time.time;
    }


    public override void Move() //Переопределить функцию Move суперкласса Enemy
    {
        // Так как pos * это свойство, нельзя напрямую изменить pos.x
        // поэтому получим pos в виде вектора Vector3, доступного для изменения
        Vector3 tempPos = pos;
        // значение theta изменяется с течением времени
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;

        // повернуть немного относительно оси Y
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);

        // base.Move() обрабатывает движение вниз, вдоль оси Y
        base.Move();

        //print(bndCheck.isOnScreen);
    }

}
