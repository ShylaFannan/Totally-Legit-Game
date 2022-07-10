using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  [SerializeField]
  private float _speed = 3.5f;
  [SerializeField]
  private float _speedMultiplier = 3f;

  private float _extraSpeed = 0f;
  [SerializeField]
  private GameObject _thruster;
  private float _fuel = 100f;
  private bool _fuelCooldownActive = false;

  [SerializeField]
  private GameObject _laserPrefab;
  [SerializeField]
  private GameObject _tripleShotPrefab;
  [SerializeField]
  private GameObject _loveShotPrefab; 


  private bool _isTripleShotActive = false;
  private bool _isLoveShotActive = false;
  

  private bool _isShieldActive = false;
  [SerializeField]
  private GameObject _shieldVisualizer;
  [SerializeField]
  private int _shieldLife;
  SpriteRenderer _shieldColor;

  [SerializeField]
  private int _lives = 3;
  private SpawnManager _spawnManager;
  [SerializeField]
  private int _score;
  private UIManager _uiManager;
  [SerializeField]
  private GameObject _rightEngine, _leftEngine; 
  [SerializeField]
  private AudioClip _laserSoundClip;
  private AudioSource _audioSource;

  [SerializeField]
  private float _fireRate = 0.25f;
  private float _canFire = -1f;

  [SerializeField]
  private int _currentAmmo; //how much ammo player currently has
  [SerializeField]
  private int _maxAmmo = 15; //how much ammo player can possibly get
  private int _minAmmo = 0; //can't shoot when reach this number
  //[SerializeField]
  //private GameObject _ammoCollectPrefab;
  //[SerializeField]
  //private GameObject _ammoCutPrefab;

  private DizzyCam _cameraShake; //references camera gameobject


  void Start()
  {
    transform.position = new Vector3 (0, 0, 0);
    _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    _audioSource = GetComponent<AudioSource>();
    _shieldColor = _shieldVisualizer.GetComponent<SpriteRenderer>();
    _currentAmmo = _maxAmmo;
    _cameraShake = GameObject.Find("Main_Cam").GetComponent<DizzyCam>(); //get dizzycam script from Main Camera

    if (_spawnManager == null)
    {
      Debug.LogError("The Spawn Manager is Null");
    }

    if(_uiManager == null)
    {
      Debug.LogError("The UI Manager is NULL.");
    }

    if(_cameraShake == null)
    {
      Debug.LogError("camerashake is null!");
    }

    if (_audioSource == null)
    {
      Debug.LogError("AudioSource on the player is null!");
    }
    else
    {
      _audioSource.clip = _laserSoundClip;
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
    transform.Translate (direction * (_speed + _extraSpeed) * Time.deltaTime);

    if(Input.GetKey(KeyCode.LeftShift) && !_fuelCooldownActive)
    {
      if (_fuel > 0)
      {
        _thruster.SetActive(true);
        _extraSpeed = 3.5f;
        _fuel -= 15 * Time.deltaTime;
      }
      else
      {
        _fuel = 0;
        _thruster.SetActive(false);
        _extraSpeed = 0f;
        StartCoroutine(ThrusterCoolDownRoutine());
      }
      _uiManager.UpdateThrusterFuel(_fuel);
    }

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

  IEnumerator ThrusterCoolDownRoutine()
  {
    yield return new WaitForSeconds(3f);
    _fuelCooldownActive = true;

    while(true)
    {
      _fuel += 15 * Time.deltaTime;

      if (_fuel >= 100f)
      {
        _fuelCooldownActive = false;
        break;
      }

      _uiManager.UpdateThrusterFuel(_fuel);
      yield return new WaitForSeconds(15 * Time.deltaTime);
    }
  }

  void FireLaser()
  {
    _canFire = Time.time + _fireRate; //canfire equals the time the game has been running plus fire rate

    if (_isTripleShotActive == true && _currentAmmo > _minAmmo) //is triple shot active and current ammo is greater than 0?
    {
      Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity); //create triple shot 
      _audioSource.Play();
    }
    else if(_isLoveShotActive == true && _currentAmmo > _minAmmo) //love shot is active and cur ammo more than 0
    {
      Instantiate(_loveShotPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity); //create love shot
      _audioSource.Play();
    }
    else if (_currentAmmo > _minAmmo) //special shots not active but is current ammo greater than 0?
    {
      Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity); //create regular laser
      _audioSource.Play();
    }
    else //if current ammo is less than 0, log this message
    {
      Debug.Log("Player is out of Ammo");
    }

    _currentAmmo --; //minus 1 ammo each time this is called

    int _AmmoClamp = Mathf.Clamp(_currentAmmo, _minAmmo, _maxAmmo); //current ammo can't be more or less than max/min
    _currentAmmo = _AmmoClamp;
    _uiManager.UpdateAmmo(_currentAmmo); //get current ammo from UIManager script 
  }

  public void Damage()
  {
    if (_isShieldActive == true)
    {
      _shieldLife --;

      if(_shieldLife == 0)
      {
        _isShieldActive = false;
        _shieldVisualizer.SetActive(false);
        return;
      }
      else if (_shieldLife == 1)
      {
        _shieldColor.color = Color.green;
      }
      else if (_shieldLife == 2)
      {
        _shieldColor.color = Color.cyan;
        return; 
      }
    }

    _lives--;
    _cameraShake.TakeDamage(); //start take damage method from dizzycam script
    

    if(_lives == 2)
    {
      _leftEngine.SetActive(true);
    }
    else if(_lives == 1)
    {
      _rightEngine.SetActive(true);
    }

    _uiManager.UpdateLives(_lives);

    if (_lives < 1)
    {
      _spawnManager.OnPlayerDeath();
      Destroy(this.gameObject);
    }
  }

  public void TripleShotActive()
  {
    _isTripleShotActive = true;
    _currentAmmo += 10;
    StartCoroutine(TripleShotPowerDownRoutine());
  }

  IEnumerator TripleShotPowerDownRoutine()
  {
    yield return new WaitForSeconds(5.0f);
    _isTripleShotActive = false;
  }

  public void LoveShotActive()
  {
    _isLoveShotActive = true;
    _currentAmmo += 5;
    StartCoroutine(LoveShotPowerDownRoutine());
  }

  IEnumerator LoveShotPowerDownRoutine()
  {
    yield return new WaitForSeconds(5.0f);
    _isLoveShotActive = false;
  }


  public void SpeedBoostActive()
  {
    _speed *= _speedMultiplier;
    StartCoroutine(SpeedBoostPowerDownRoutine());
  }

  IEnumerator SpeedBoostPowerDownRoutine()
  {
    yield return new WaitForSeconds(5.0f);
    _speed /= _speedMultiplier;
  }

  public void ShieldsActive()
  {
    if(_shieldLife < 3)
    {
      _shieldLife ++;
      _isShieldActive = true;
      _shieldVisualizer.SetActive(true);
    }

    if(_shieldLife == 1)
    {
      _shieldColor.color = Color.green;
    }
    else if(_shieldLife == 2)
    {
      _shieldColor.color = Color.cyan;
    }
    else if(_shieldLife == 3)
    {
      _shieldColor.color = Color.red;
    }
  }

  public void addAmmo()
  {
    _currentAmmo = 15;
    _uiManager.UpdateAmmo(_currentAmmo); //get current ammo from UIManager script
  }

  public void AmmoCut()
  {
    _currentAmmo = 0;
    _uiManager.UpdateAmmo(_currentAmmo);
  }

  public void Plus1Life()
  {
    if(_lives == 3)
    {
      Debug.Log("Lives at MAX");
      return;
    }

    _lives ++;
    _uiManager.UpdateLives(_lives);

    if(_lives == 3)
    {
      _leftEngine.SetActive(false);
    }
    else if(_lives == 2)
    {
      _rightEngine.SetActive(false);
    }
  }
  
  public void AddScore(int points)
  {
    _score += points;
    _uiManager.UpdateScore(_score);
  }
}
