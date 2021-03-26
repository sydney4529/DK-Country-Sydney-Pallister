using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireBounce : MonoBehaviour
{
    Animator anim;

    AudioSource bounceSource;

    public AudioClip bounce;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        bounceSource = GetComponent<AudioSource>();

        if (bounceSource)
        {
            bounceSource.clip = bounce;
            bounceSource.loop = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bounce()
    {
        anim.SetBool("Bounce", true);
        bounceSource.Play();
    }

    public void FinishBounce()
    {
        anim.SetBool("Bounce", false);
    }
}
