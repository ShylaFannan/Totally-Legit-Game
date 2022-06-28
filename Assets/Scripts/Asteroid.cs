using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        transform.Rotate (0,0,25*Time.deltaTime); //rotates 25 degrees per second around z axis
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.25f);
        }
    }
    //check for LASER collision
    //instantiate explosion at position of asteroid
    //destrpu explosion after 3 seconds.

}
