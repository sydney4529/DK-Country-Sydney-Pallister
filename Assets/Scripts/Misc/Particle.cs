using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    AudioSource particleAudio;
    // Start is called before the first frame update
    void Start()
    {
        particleAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!particleAudio.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
