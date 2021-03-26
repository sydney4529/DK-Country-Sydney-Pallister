using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(PlayerMovement))]

public class PlayerCollision : MonoBehaviour
{
    public Rigidbody2D rb;
    PlayerMovement pm;
    Animator anim;

    public AudioSource dieSource;
    public AudioSource hitSource;
    public BoxCollider2D playerBox;

    public AudioClip playerDie;
    public AudioClip playerHit;

    public AudioMixerGroup mixerGroup;

    public float bounceForce;
    public bool doneDeath;
    public bool death;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMovement>();
        dieSource = GetComponent<AudioSource>();
        hitSource = GetComponent<AudioSource>();
        playerBox = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        if (bounceForce <= 0)
        {
            bounceForce = 20.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!dieSource)
        {
            dieSource = gameObject.AddComponent<AudioSource>();
            dieSource.clip = playerDie;
            dieSource.outputAudioMixerGroup = mixerGroup;
            dieSource.loop = false;
            //dieSource.Play();
        }
        else
        {
           // dieSource.Play();
        }
        if (!hitSource)
        {
            hitSource = gameObject.AddComponent<AudioSource>();
            hitSource.outputAudioMixerGroup = mixerGroup;
            hitSource.clip = playerHit;
            hitSource.loop = false;
            //dieSource.Play();
        }
        else
        {
            // dieSource.Play();
        }

        anim.SetBool("Dead", death);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Squished")
        {
            if (!pm.isGrounded && collision.gameObject.GetComponentInParent<EnemyWalker>().walkerCollider.enabled == true)
            {
                collision.gameObject.GetComponentInParent<EnemyWalker>().isSquished();
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * bounceForce);
                GameManager.instance.score++;
            }
        }

        if (collision.gameObject.tag == "TurretSquished" && collision.gameObject.GetComponentInParent<EnemyTurret>().turretCollider.enabled == true)
        {
            if (!pm.isGrounded)
            {
                collision.gameObject.GetComponentInParent<EnemyTurret>().isSquished();
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * bounceForce);
                GameManager.instance.score++;
            }
        }

        if (collision.gameObject.tag == "Bounce" && collision.gameObject.GetComponentInParent<EnemyWalkerLarge>().walkerCollider.enabled == true)
        {
            if (!pm.isGrounded)
            {
                collision.gameObject.GetComponentInParent<EnemyWalkerLarge>().isBounce();
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * bounceForce);
            }
        }

        if (collision.gameObject.tag == "LevelEnd")
        {
            GameManager.instance.win = true;
        }

        if (collision.gameObject.tag == "DeathBox")
        {
            //GameManager.IsInputEnabled = false;
            //playerBox.enabled = false;
            //GameManager.instance.lives--;
            //doneDeath = true;
            //dieSource.Play();

            GameManager.IsInputEnabled = false;
            death = true;
            playerBox.enabled = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
            GameManager.instance.lives--;
            dieSource.Play();
        }

        if (collision.gameObject.tag == "Tire")
        {
            if (!pm.isGrounded)
            {
                collision.gameObject.GetComponentInParent<TireBounce>().Bounce();
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * (bounceForce + 150));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EnemyProjectile")
        {
            GameManager.IsInputEnabled = false;
            death = true;
            playerBox.enabled = false;
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 0;
            //dieSource.Play();
            GameManager.instance.lives--;
            Destroy(collision.gameObject);
            dieSource.Play();
            //Destroy(gameObject);
        }

        if(collision.gameObject.tag == "Enemy")
        {
            GameManager.IsInputEnabled = false;
            death = true;
            playerBox.enabled = false;
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 0;
            //dieSource.Play();
            GameManager.instance.lives--;
            dieSource.Play();
            //Destroy(gameObject);
        }

    }

    void PlayHit()
    {
        hitSource.Play();
    }

    void DoneDying()
    {
        doneDeath = true;
    }
}
