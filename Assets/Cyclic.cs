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
        // ¬ычислить значение радианов по текущему времени
        float radians = Time.time * Mathf.PI;

        // ѕреобразовать радианы в градусы дл€ отображени€ в инспекторе
        // ќпераци€ "% 360" ограничивает значение диапазоном 0 .. 359.9999
        theta = Mathf.Round(radians * Mathf.Rad2Deg) % 360;

        // ”становить исходную позицию
        pos = Vector3.zero;

        // ¬ычислить x и y как косинус и синус угла соответственно
        pos.x = Mathf.Cos(radians);
        pos.y = Mathf.Sin(radians);

        // »спользовать синус и косинус, если
        // соответствующие флажки установлены в инспекторе
        Vector3 tPos = Vector3.zero;
        if (showCosX) tPos.x = pos.x;
        if (showSinY) tPos.y = pos.y;

        // ѕозиционировать this.gameObject (Sphere)
        transform.position = tPos;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        // Ќарисовать цветные волнистые линии
        // (можете просто пропустить этот цикл for)

        int inc = 10;

        for (int i=0; i<360; i += inc)
        {

        }
    }
}
