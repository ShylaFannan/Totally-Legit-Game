using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  [SerializeField]
  private float _speed = 4.0f;
  private float _halfSpeed = 2.0f;
  private int movementTypeID;
  
  [SerializeField]
  private GameObject _laserPrefab;
  private float _fireRate = 3.0f;
  private float _canFire = -1;

  private Player _player;

  private Animator _anim;
  private AudioSource _audioSource; 

  //shield stuff
  [SerializeField]
  private GameObject _shield;
  [SerializeField]
  private bool _isShieldActive = false;
  private int _shieldChance;
  private int _shieldPower;
  //end shield stuff

  void Start()
  {
    movementTypeID = Random.Range(1,4);
    _player = GameObject.Find("Player").GetComponent<Player>();
    
    if (_player == null)
    {
      Debug.LogError("The Player is NULL.");
    }

    _anim = GetComponent<Animator>();

    if (_anim == null)
    {
      Debug.LogError("The Animation is NULL.");
    }

    _audioSource = GetComponent<AudioSource>();

    if (_audioSource == null)
    {
      Debug.LogError("AudioSource on the player is null!");
    }
    //shield stuff
    _isShieldActive = false;
    _shield.SetActive(false);
    ShieldCheck();
    //end shield stuff
  }

  void Update()
    {
      CalculateMovement();

      if(Time.time > _canFire)
      {
        _fireRate = Random.Range(5f, 10f);
        _canFire = Time.time + _fireRate;
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        
        for (int i = 0; i < lasers.Length; i++)
        {
          lasers[i].AssignEnemyLaser();
        }
      }
    }
    
  void CalculateMovement()
  {
    switch (movementTypeID)
    {
      case 1:
        transform.Translate((Vector3.down + Vector3.left) * _halfSpeed * Time.deltaTime);
        break;
      case 2:
        transform.Translate((Vector3.down + Vector3.right) * _halfSpeed * Time.deltaTime);
        break;
      case 3: 
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        break;
      default:
        break;
    }
  }

//shield stuff
  private void ShieldIsActive()
  {
    _shieldPower = 1;
    _isShieldActive = true;
    _shield.SetActive(true);
  }

  private void ShieldCheck()
  {
    _shieldChance = Random.Range(0, 4);
    if (_shieldChance == 0)
    {
      ShieldIsActive();
    }
  }

  IEnumerator ShieldChangeDelay()
  {
    yield return new WaitForSeconds(1f);
    _isShieldActive = false;
  }
  //END SHIELD STUFF

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Player")
    {
      Player player = other.transform.GetComponent<Player>();

    if (player != null)
    {
      player.Damage();
    }

      _anim.SetTrigger("OnEnemyDeath");
      _speed = 0;
      _audioSource.Play();
      Destroy(this.gameObject, 2.8f);
    }
      
    if (other.tag == "laser" && _isShieldActive == true) //&& _isShieldActive == true
    {
      Destroy(other.gameObject);
      _shieldPower --; //SHIELD
      _shield.SetActive(false); //SHIELD
      StartCoroutine(ShieldChangeDelay()); //SHIELD
      return; //SHIELD
    }

    if (other.tag == "laser" && _isShieldActive == false)
    {
      Destroy(other.gameObject);
      _anim.SetTrigger("OnEnemyDeath");
      _speed = 0;
      _audioSource.Play();
      Destroy(GetComponent<Collider2D>());
      Destroy(this.gameObject, 2.8f);

      if (_player != null)
      {
        _player.AddScore(10);
      }
    } 
  }
}

