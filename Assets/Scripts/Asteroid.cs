using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    //[SerializeField]
    //private AudioClip _explosionSoundClip;
    //private AudioSource _audioSource;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        //_audioSource = GetComponent<AudioSource>();
        //if (_audioSource == null)
        //{
            //Debug.LogError("AudioSource on the asteroid is null!");
        //}
        //else
        //{
            //_audioSource.clip = _explosionSoundClip;
        //}
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
            //_audioSource.Play();
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.25f);
        }
    }
}
