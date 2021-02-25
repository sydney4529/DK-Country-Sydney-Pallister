using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer donkeyKongSprite;

    public float speed;
    public int jumpForce;
    public bool isGrounded;
    public LayerMask isGroundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;
    public bool isFiring = false;
    public bool isRoll = false;

    int _score;
    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            Debug.Log("Current Score is " + _score);
        }
    }

    public int maxLives = 3;
    int _lives = 3;
    public int lives
    {
        get { return _lives; }
        set
        {
            _lives = value;
            if(_lives > maxLives)
            {
                _lives = maxLives;
            }
            else if(_lives < 0)
            {
                //game over code goes here
            }
            Debug.Log("Current lives: " + _lives);
        }
    }


    private Vector3 initialScale;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        donkeyKongSprite = GetComponent<SpriteRenderer>();

        initialScale = transform.localScale;

        if (speed <= 0)
        {
            speed = 5.0f;
        }

        if (jumpForce <= 0)
        {
            jumpForce = 100;
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.01f;
        }

        if (!groundCheck)
        {
            Debug.Log("groundCheck does not exist, please set a transform value for groundCheck");
        }

    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);



        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //make jump velocity always the same, comment out if you dont want it
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);
            
        }

        if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.UpArrow))
        {
            isRoll = true;
        }
        else
        {
            isRoll = false;
        }

        if (Input.GetKeyUp(KeyCode.Space) && Input.GetKeyUp(KeyCode.UpArrow))
        {
            isRoll = false;
        }
      

        if (Input.GetButtonDown("Fire1"))
        {
            isFiring = true;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            isFiring = false;
        }

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        anim.SetFloat("speed", Mathf.Abs(horizontalInput));

        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isFiring", isFiring);
        anim.SetBool("isRoll", isRoll);

        if(donkeyKongSprite.flipX && horizontalInput > 0 || !donkeyKongSprite.flipX && horizontalInput < 0)
        {
            donkeyKongSprite.flipX = !donkeyKongSprite.flipX;
        }

    }

    public void startJumpForceChange()
    {
        StartCoroutine(JumpForceChange());
    }

    IEnumerator JumpForceChange()
    {
        jumpForce = 300;
        yield return new WaitForSeconds(2.0f);
        jumpForce = 200;
    }

    void OnTriggerStay2D(Collider2D collision)
    {

        if(collision.gameObject.tag == "Pickups")
        {
            Pickups curPickup = collision.GetComponent<Pickups>();
            if (Input.GetKey(KeyCode.E))
            {
                switch (curPickup.currentCollectible)
                {
                    case Pickups.CollectibleType.BONUS:
                        //add to inventory or other mechanic
                        Destroy(collision.gameObject);
                        break;
                }
            }

        }
           
    }
}
