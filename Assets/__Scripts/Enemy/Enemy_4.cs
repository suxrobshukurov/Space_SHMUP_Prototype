using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Part -- еще один сериализуемый класс подобно WeaponDefinition,
/// предназначенный для хранения данных
/// </summary>
[System.Serializable]
public class Part
{
    // Значения этих трех полей дожно определиться в инспекторе
    public string name;             // Имя этой части
    public float health;            // Степень стойкости этой части
    public string[] protectedBy;    // Другие части, защищающие эту


    // эти два поля инициализируются автоматичеки в Start()
    // Кэширование, как здесь, ускоряет получение необходимых данных
    // не позволяет следующему полю пояаиться в инспекторе
    [HideInInspector] public GameObject go;                 // игровой объект этой части
    [HideInInspector] public Material mat;                  // Материал для отображения повреждений 

}

public class Enemy_4 : Enemy
{
    [Header("Set in Inspector: Enemy_4")]
    public Part[] parts;                    // массив частей, составляющих корабль

    private Vector3 p0, p1;
    private float timeStart;
    private float duration = 4;

    private void Start()
    {
        // начальная позиция уже выбрана в Main.SpawnEnemy()
        p0 = p1 = pos;
        InitMovement();

        // Записать в кэш игровой объект и материал каждой части в parts
        Transform t;
        foreach ( Part prt in parts)
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }
    }

    void InitMovement()
    {
        p0 = p1;

        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);

        timeStart = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - timeStart) / duration;

        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);
        pos = (1 - u) * p0 + u * p1;
    }

    // эти двае функции выполняют поиск части в массиве parts n 
    // по именеи или по ссылке на игровой объект
    Part FindPart(string n)
    {
        foreach(Part part in parts)
        {
            if (part.name == n)
            {
                return (part);
            }
        }
        return (null);
    }

    Part FindPart(GameObject go)
    {
        foreach (Part part in parts)
        {
            if (part.go == go)
            {
                return (part);
            }
        }
        return (null);
    }

    // Эту функции возвращают true, если данная часть уничтожена
    bool Destroyed(GameObject go)
    {
        return (Destroyed(FindPart(go)));
    }

    bool Destroyed(string n)
    {
        return (Destroyed(FindPart(n)));
    }
    
    bool Destroyed(Part prt)
    {
        if (prt == null)        // Если ссылка на часть не было передана
        {
            return (true);      // Вернуть true (то есть: да, было уничтожена)
        }
        // Вернуть результат сравнения: prt.health <= 0
        // Если prt.health <= 0, вернуть true (да, было уничтожена)
        return (prt.health <= 0);
    }

    // Окрашивает в красный только одну часть, а не весь корабль
    void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }

    // Переопределяет метод OnCollisionEnter из сценария Enemy.cs
    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        switch (other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                // если корабль за границей экрана, не повреждать его
                if (!bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }

                // Поразить вражеский корабль 
                GameObject goHit = collision.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if (prtHit == null)
                {
                    goHit = collision.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }

                // проверить, защищена ли еще эта часть корабля
                if (prtHit.protectedBy != null)
                {
                    foreach(string s in prtHit.protectedBy)
                    {
                        // если хотя юы одна из защищяющих частей езе не разрушена...
                        if (!Destroyed(s))
                        {
                            //... не наносить повреждения этой части
                            Destroy(other);         // уничтожить снаряд ProjectileHero
                            return;                 // выйти, не повреждения Еnemy_4
                        }
                    }
                }

                // эта часть не защищена, нанести ей повреждение
                // получить разрушающую силу из Projectile.type и Main.WEAR_DICT
                prtHit.health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                // показать эффект попадания в часть
                ShowLocalizedDamage(prtHit.mat);
                if (prtHit.health <= 0)
                {
                    prtHit.go.SetActive(false);
                }

                // проверить, был ои корабль полностью зарзрушен
                bool allDestroyed = true;           // предложить, что разрушен
                foreach(Part prt in parts)
                {
                    if (!Destroyed(prt))
                    {
                        allDestroyed = false;
                        break;
                    }
                }

                if (allDestroyed)
                {
                    Main.S.ShipDestroyed(this);
                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;
        }
    }
}
