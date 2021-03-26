using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]

public class EnemyWalkerLarge : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    AudioSource deathSource;
    AudioSource bounceSource;
    public Collider2D walkerCollider;

    public AudioClip enemyDeath;
    public AudioClip bounce;

    public AudioMixerGroup mixerGroup;

    public int health;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        deathSource = GetComponent<AudioSource>();
        walkerCollider = gameObject.GetComponent<BoxCollider2D>();

        if (deathSource)
        {
            deathSource.clip = enemyDeath;
            deathSource.loop = false;
        }

        if (speed <= 0)
        {
            speed = 5.0f;
        }
        if (health <= 0)
        {
            health = 3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(anim.GetBool("Bounce"));
        if (!anim.GetBool("Death") && !anim.GetBool("Bounce"))
        {
            if (sr.flipX)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
        }

        if (!deathSource.isPlaying && !walkerCollider.enabled)
        {
            Destroy(gameObject);
        }

        if (!bounceSource)
        {
            bounceSource = gameObject.AddComponent<AudioSource>();
            bounceSource.outputAudioMixerGroup = mixerGroup;
            bounceSource.clip = bounce;
            bounceSource.loop = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Barrier")
        {
            sr.flipX = !sr.flipX;
        }
    }

    public void isDead()
    {
        health--;
        //Debug.Log(health);
        if (health <= 0)
        {
            anim.SetBool("Death", true);
            rb.velocity = Vector2.zero;
            GameManager.instance.score++;
        }
    }

    public void isBounce()
    {
        anim.SetBool("Bounce", true);
        bounceSource.Play();
    }

    public void FinishedBounce()
    {
        anim.SetBool("Bounce", false);
    }

    public void FinishedDeath()
    {
        //Destroy(gameObject);
        //rb.gravityScale = 0;
        walkerCollider.enabled = false;
        deathSource.Play();
    }
}
