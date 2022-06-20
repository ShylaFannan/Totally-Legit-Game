using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  [SerializeField]
  private float _speed = 3.5f;
  [SerializeField]
  private float _speedMultiplier = 2;
  [SerializeField]
  private GameObject _laserPrefab;
  [SerializeField]
  private GameObject _tripleShotPrefab; 
  [SerializeField]
  private float _fireRate = 0.25f;
  private float _canFire = -1f;
  [SerializeField]
  private int _lives = 3;
  private SpawnManager _spawnManager;
  private bool _isTripleShotActive = false;
  //private bool _isSpeedBoostActive = false;
  private bool _isShieldActive = false;
  [SerializeField]
  private GameObject _shieldVisualizer;

    void Start()
    {
      transform.position = new Vector3 (0, 0, 0);
     _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

      if (_spawnManager == null)
      {
        Debug.LogError("The Spawn Manager is Null");
      }

    }
      


    void Update()
    {
     CalculateMovement(); 

     if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
     {
      FireLaser();
     }

    }
    void CalculateMovement()
    {
      float horizontalInput = Input.GetAxis("Horizontal");
      float verticalInput = Input. GetAxis("Vertical");
      Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
      transform.Translate (direction * _speed * Time.deltaTime);

      if (transform.position.y>=0)
      {
        transform.position = new Vector3(transform.position.x,0,0);
      }

      else if (transform.position.y<=-3.8f)
      {
        transform.position = new Vector3(transform.position.x,0,0);
      }

      if (transform.position.x>11.3f)
      {
        transform.position = new Vector3(-11.3f, transform.position.y, 0);
      }

      else if (transform.position.x <-11.3f)
      {
        transform.position = new Vector3(11.3f, transform.position.y, 0);
      }
    }

      
    
void FireLaser()
      {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
          Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }

        else
        {
        Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }

      }

public void Damage()
  {
    //if shield is active
    //do nothing
    //deactivate shield
    //return; 
    if (_isShieldActive == true)
    {
       _isShieldActive = false;
       _shieldVisualizer.SetActive(false);
       return;
    }

     _lives--;
   
    if (_lives < 1)
    {
      _spawnManager.OnPlayerDeath();
      Destroy(this.gameObject);
    }

  }

  public void TripleShotActive()
  {
    _isTripleShotActive = true;
    StartCoroutine(TripleShotPowerDownRoutine());
  }

  IEnumerator TripleShotPowerDownRoutine()
  {
    yield return new WaitForSeconds(5.0f);
    _isTripleShotActive = false;
  }

  public void SpeedBoostActive()
  {
    //_isSpeedBoostActive = true;
    _speed *= _speedMultiplier;
    StartCoroutine(SpeedBoostPowerDownRoutine());
  }

  IEnumerator SpeedBoostPowerDownRoutine()
  {
    yield return new WaitForSeconds(5.0f);
   // _isSpeedBoostActive = false;
    _speed /= _speedMultiplier;
  }

  public void ShieldsActive()
  {
    _isShieldActive = true;
    _shieldVisualizer.SetActive(true);
  }
  
}
