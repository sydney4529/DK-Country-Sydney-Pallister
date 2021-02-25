using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs;

    private int randomPrefab;

    private void Start()
    {
        randomPrefab = Random.Range(0, 4);
        Instantiate(prefabs[randomPrefab], transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
