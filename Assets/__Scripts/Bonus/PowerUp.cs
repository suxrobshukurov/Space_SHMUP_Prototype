using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(0.25f, 2);
    public float lifeTime = 6f; //врем€ сушествовоани€ PowerUp
    public float fadeTime = 4f; 

    [Header("Set Dynamically")]
    public WeaponType type; // “ип бонуса
    public GameObject cube; // —сылка на вложеный куб
    public TextMesh letter; // —сылка на TextMesh
    public Vector3 rotPerSecond;
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;

    private void Awake()
    {
        // получить ссылку на куб
        cube = transform.Find("Cube").gameObject;

        // ѕолучить ссылку на TextMesh и другие компоненты
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        // ¬ыбрать случайную скорость
        Vector3 vel = Random.onUnitSphere; // получить случайную скорость XYZ Random.onUnitSphere возвращают вектор, указывающий на случайную точку,
        // наход€щуюс€ на поверхности сферы с радиусом 1м и с центром в начале координат
        vel.z = 0; // ќтобразить vel на плоскости XY
        vel.Normalize(); // Ќормализаци€ устанавливает длину Vector3 равной 1м
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        // установить угол поворота этого игрового объекта равным R: [0, 0, 0]
        transform.rotation = Quaternion.identity; // отсуствие поворота

        // ¬ыбрать случайную скорость вращени€ дл€ вложенного куба с использованием rotMinMax.x и rotMinMax.y
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y));

        birthTime = Time.time;
    }

    private void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        // Ёффект растворени€ куба PowerUp с течением времени со значением по умолчанию бонус существует 10 секунд
        // а затем раствор€етс€ в течение 4 секунд
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        // в течение lifeTime секунд значение u будет <= 0. «атем оно станет положительным и через fadeTime секунд станет больше 1
        // если u >= 1 уничтожить бонус 
        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }
        // использовать u дл€ определение альфа-значени€ куба и буквы 
        if ( u > 0)
        {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if (!bndCheck.isOnScreen)
        {
            Destroy(gameObject);
        }
    }

    public void SetType (WeaponType wt)
    {
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        cubeRend.material.color = def.color;
        // letter.color = def.color;
        letter.text = def.letter;
        type = wt;
    }

    public void AbsorbedBy(GameObject target)
    {
        Destroy(this.gameObject);
    }
}
