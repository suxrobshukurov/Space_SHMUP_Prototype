using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;

    [Header("Set in Inspectot")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameDestroyDelay = 2f;
    public GameObject projectilePrefab;
    public float profectileSpeed = 40;
    public Weapon[] weapons;

    public float fireRate = 0.2f; // для автовыстрела  
   

    [Header("Set Dynamically")]
    //public float shieldLevel = 1;
    [SerializeField] private float _shieldLevel = 1;
    public bool canShoot; // для автовыстрела

    private GameObject lastTriggerGo = null;

    // Объявление нового делегата тиа WeaponeFireDelegate
    public delegate void WeaponeFireDelegate();
    // Создать поле типа WeaponeFireDelegate с именем fireDelegate
    public WeaponeFireDelegate fireDelegate;

    private void Awake()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.Log("Hero.Awake() - Попоытка назначить второй Hero.S!");
        }
        // fireDelegate += TempFire;

        canShoot = true;
    }

    private void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        //Позволять короблю выстрелить
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TempFire();
        //}

        // Произвести выстрел из всех видов оружия вызовом fireDelegate
        // Сначала проверить нажатие клавиш: Axis("Jump")
        // затем убедиться, что значение fireDelegate не равно null, чтобы избежать ошибки
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }

        // для авто выстрела 
        //if (canShoot)
        //{
        //    StartCoroutine(TempFireAuto());
        //}

    }

    // Позволять короблю выстрелить 
    void TempFire()
    {
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        // rigidB.velocity = Vector3.up * profectileSpeed;

        Projectile proj = projGO.GetComponent<Projectile>();
        proj.type = WeaponType.blaster;
        float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
        rigidB.velocity = Vector3.up * tSpeed;
    }

    // для авто выстрела 
    IEnumerator TempFireAuto()
    {
        canShoot = false;
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        rigidB.velocity = Vector3.up * profectileSpeed;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        //print("Triggered: " + go.name);

        if (go == lastTriggerGo)
        {
            return;
        }
        lastTriggerGo = go;

        if (go.tag == "Enemy")
        {
            shieldLevel--;
            Destroy(go);
        }
        else if (go.tag == "PowerUp")
        {
            AbsorbPowerUp(go);
        }
        else
        {
            print("Triggered by non-Enemy: " + go.name);
        }

    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.shield:
                shieldLevel++;
                break;
            default:
                if (pu.type == weapons[0].type)
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        w.SetType(pu.type);
                    }
                }
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }

    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }

    void ClearWeapons()
    {
        foreach(Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }

    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameDestroyDelay);
            }
        }
    }
}
