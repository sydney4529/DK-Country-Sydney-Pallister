using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            Vector3 cameraTransform;
            cameraTransform = transform.position;
            cameraTransform.x = player.transform.position.x;
            cameraTransform.x = Mathf.Clamp(cameraTransform.x, -12.116f, 36.98f);
            cameraTransform.y = player.transform.position.y + 0.3f;
            cameraTransform.y = Mathf.Clamp(cameraTransform.y, -2.465f, 0.926f);
            transform.position = cameraTransform;
        }
    }
}
