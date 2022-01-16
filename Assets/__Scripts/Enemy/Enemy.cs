using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;

    protected BoundsCheck bndCheck;

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    private void Update()
    {
        Move();

        if (bndCheck != null && bndCheck.offDown)
        {
            Destroy(gameObject);
        }
    }

    public virtual void Move()
    {
        Vector3 temPos = pos;
        temPos.y -= speed * Time.deltaTime;
        pos = temPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGo = collision.gameObject;
        if(otherGo.tag == "ProjectileHero")
        {
            Destroy(otherGo);
            Destroy(gameObject);
        } 
        else
        {
            print("Enemy hit by non-ProjectileHero: " + otherGo.name);
        }
    }

    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }
}
