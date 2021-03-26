using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

public class Projectile : MonoBehaviour
{

    public float speed;
    public float lifeTime;

    public ParticleSystem breakBarrel;

    CircleCollider2D thisCollider;

    // Start is called before the first frame update
    void Start()
    {
        thisCollider = GetComponent<CircleCollider2D>();

        if (lifeTime <= 0)
        {
            lifeTime = 2.0f;
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
        Destroy(gameObject, lifeTime);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "JumpThrough")
        {
            Physics2D.IgnoreCollision(collision.collider, thisCollider);
        }

        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 11)
        {
            //Debug.Log(collision.gameObject.tag);
            if(collision.gameObject.tag != "JumpThrough")
            {
                Destroy(gameObject);
            }

        }

        if (gameObject.tag == "PlayerProjectile")
        {
            if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Squished")
            {
                EnemyWalker walkerScript = collision.gameObject.GetComponent<EnemyWalker>();
                EnemyWalkerLarge walkerLargeScript = collision.gameObject.GetComponent<EnemyWalkerLarge>();
                EnemyTurret turretScript = collision.gameObject.GetComponent<EnemyTurret>();

                if (walkerScript)
                {
                    walkerScript.isDead();
                }

                if (walkerLargeScript)
                {
                    walkerLargeScript.isDead();
                }



                Instantiate(breakBarrel, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }

        if (gameObject.tag == "EnemyProjectile")
        {
            if (collision.gameObject.tag == "Player")
            {
                //hurt player here
                Destroy(gameObject);
            }

        }

    }

}
