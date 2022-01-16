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

    public float fireRate = 0.2f; // дл€ автовыстрела  
   

    [Header("Set Dynamically")]
    //public float shieldLevel = 1;
    [SerializeField] private float _shieldLevel = 1;
    public bool canShoot; // дл€ автовыстрела

    private GameObject lastTriggerGo = null;

    private void Awake()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.Log("Hero.Awake() - ѕопоытка назначить второй Hero.S!");
        }

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

        //ѕозвол€ть короблю выстрелить
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TempFire();
        }

        // дл€ авто выстрела 
        //if (canShoot)
        //{
        //    StartCoroutine(TempFireAuto());
        //}

    }

    // ѕозвол€ть короблю выстрелить 
    void TempFire()
    {
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        rigidB.velocity = Vector3.up * profectileSpeed;
    }

    // дл€ авто выстрела 
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
        else
        {
            print("Triggered by non-Enemy: " + go.name);
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
