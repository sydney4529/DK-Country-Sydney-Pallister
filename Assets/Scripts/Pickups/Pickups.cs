using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public enum CollectibleType
    {
        POWERUP,
        COLLECTIBLE,
        LIVES,
        COLLECTIBLE2,
        BONUS
    }

    public CollectibleType currentCollectible;
    public AudioSource pickupAudioSource;
    BoxCollider2D pickupCollider;

    private SpriteRenderer sprite;

    public AudioClip pickupAudio;

    private void Start()
    {
        pickupAudioSource = gameObject.GetComponent<AudioSource>();
        pickupCollider = gameObject.GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();

        if (pickupAudio)
        {
            pickupAudioSource.clip = pickupAudio;
            pickupAudioSource.loop = false;
        }
    }

    private void Update()
    {
        if (!pickupAudioSource.isPlaying && !pickupCollider.enabled)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (currentCollectible)
            {

                case CollectibleType.POWERUP:
                    //Debug.Log("powerup");
                    collision.GetComponent<PlayerMovement>().startJumpForceChange();
                    //Destroy(gameObject);
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -5000);

                    pickupCollider.enabled = false;
                    pickupAudioSource.Play();
                    break;

                case CollectibleType.LIVES:
                    //Debug.Log("lives");
                    GameManager.instance.lives++;
                    //Destroy(gameObject);
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -5000);

                    pickupCollider.enabled = false;
                    pickupAudioSource.Play();
                    break;

                case CollectibleType.COLLECTIBLE:
                    //Debug.Log("colectible");
                    GameManager.instance.score++;
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -5000);

                    pickupCollider.enabled = false;
                    pickupAudioSource.Play();
                    break;

                case CollectibleType.COLLECTIBLE2:
                    //Debug.Log("colectible");
                    GameManager.instance.score+=3;
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -5000);

                    pickupCollider.enabled = false;
                    pickupAudioSource.Play();
                    break;

            }
        }

    }

}
