using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class EnemyTurret : MonoBehaviour
{
    public Transform projectileSpawnPointRight;
    public Transform projectileSpawnPointLeft;
    public Projectile projectile;

    public int distance;

    SpriteRenderer turretSprite;

    public GameObject playerInstance;

    public float projectileForce;
    public float projectileFireRate;

    float timeSinceLastFire = 0.0f;
    public int health;

    Animator anim;
    AudioSource deathSource;
    AudioSource fireSource;
    public Collider2D turretCollider;

    public AudioClip enemyDeath;
    public AudioClip fire;

    public AudioMixerGroup mixerGroup;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        turretSprite = GetComponent<SpriteRenderer>();
        deathSource = GetComponent<AudioSource>();
        turretCollider = gameObject.GetComponent<BoxCollider2D>();

        if (deathSource)
        {
            deathSource.clip = enemyDeath;
            deathSource.loop = false;
        }

        if(projectileForce <= 0)
        {
            projectileForce = 5.0f;
        }

        if(health <= 0)
        {
            health = 5;
        }

        if(distance <= 0)
        {
            distance = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(playerInstance.transform.position, turretSprite.gameObject.transform.position) < distance)
        {
            if (Time.time >= timeSinceLastFire + projectileFireRate)
            {
                anim.SetBool("Fire", true);
                timeSinceLastFire = Time.time;
            }
        }
        else
        {

        }

        if( turretSprite.flipX && playerInstance.transform.position.x > turretSprite.gameObject.transform.position.x || !turretSprite.flipX && playerInstance.transform.position.x < turretSprite.gameObject.transform.position.x)
        {
            turretSprite.flipX = !turretSprite.flipX;
        }

        if (!deathSource.isPlaying && !turretCollider.enabled)
        {
            Destroy(gameObject);
        }

        if (!deathSource)
        {
            deathSource = gameObject.AddComponent<AudioSource>();
            deathSource.outputAudioMixerGroup = mixerGroup;
            deathSource.clip = enemyDeath;
            deathSource.loop = false;
            //dieSource.Play();
        }
        else
        {
            // dieSource.Play();
        }

        if (!fireSource)
        {
            fireSource = gameObject.AddComponent<AudioSource>();
            fireSource.outputAudioMixerGroup = mixerGroup;
            fireSource.clip = fire;
            fireSource.loop = false;
        }

    }

    public void Fire()
    {
        //firing function
        if (turretSprite.flipX)
        {
            Projectile temp = Instantiate(projectile, projectileSpawnPointLeft.position, projectileSpawnPointLeft.rotation);
            temp.speed = projectileForce * -1;
            fireSource.Play();
        }
        else
        {
            Projectile temp = Instantiate(projectile, projectileSpawnPointRight.position, projectileSpawnPointRight.rotation);
            temp.speed = projectileForce;
            fireSource.Play();
        }
    }

    public void ReturnToIdle()
    {
        anim.SetBool("Fire", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "PlayerProjectile")
        {
            health--;
            Destroy(collision.gameObject);
            if(health <= 0)
            {
                //Destroy(gameObject);
                anim.SetBool("Death", true);
                GameManager.instance.score++;
            }
        }
    }

    public void isSquished()
    {
        anim.SetBool("Squish", true);
    }

    public void FinishedDeath()
    {
        turretCollider.enabled = false;
        deathSource.Play();
    }
}
