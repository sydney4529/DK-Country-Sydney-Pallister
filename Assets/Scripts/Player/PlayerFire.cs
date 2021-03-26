using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(SpriteRenderer))]

public class PlayerFire : MonoBehaviour
{
    SpriteRenderer donkeyKongSprite;
    AudioSource fireAudioSource;

    public Transform spawnPointLeft;
    public Transform spawnPointRight;

    public float projectileSpeed;
    public Projectile projectilePrefab;

    public AudioClip fireSFX;

    public AudioMixerGroup mixerGroup;

    public bool isFiring;

    // Start is called before the first frame update
    void Start()
    {
        donkeyKongSprite = GetComponent<SpriteRenderer>();
        fireAudioSource = GetComponent<AudioSource>();

        if (projectileSpeed <=0)
        {
            projectileSpeed = 7.0f;
        }

        if(!spawnPointLeft || !spawnPointRight || !projectilePrefab)
        {
            Debug.Log("Unity Inspector values not set");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1 && GameManager.IsInputEnabled)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //FireProjectile();
                isFiring = true;

                if (!fireAudioSource)
                {
                    fireAudioSource = gameObject.AddComponent<AudioSource>();
                    fireAudioSource.clip = fireSFX;
                    fireAudioSource.outputAudioMixerGroup = mixerGroup;
                    fireAudioSource.loop = false;
                    fireAudioSource.Play();
                }
                else
                {
                    fireAudioSource.Play();
                }
            }
        }

    }

    void FireProjectile()
    {
        if (donkeyKongSprite.flipX)
        {
            //Debug.Log("fire left");
            Projectile projectileInstance = Instantiate(projectilePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
            projectileInstance.speed = projectileSpeed * -1;
            Physics2D.IgnoreCollision(projectileInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            isFiring = false;
        }
        else
        {
            //Debug.Log("fire right");
            Projectile projectileInstance = Instantiate(projectilePrefab, spawnPointRight.position, spawnPointRight.rotation);
            projectileInstance.speed = projectileSpeed;
            Physics2D.IgnoreCollision(projectileInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            isFiring = false;
        }
    }
}
