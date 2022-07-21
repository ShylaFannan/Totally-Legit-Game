using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]//ID FOR POWERUPS 0=trip shot 1=speed 2=shield 3=ammo collect 4=health collect 5=loveshot 6=ammocut
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;

    [SerializeField]
    private bool _canCollectPickup = false;

    private Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    
        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }
    }

    void Update()
    {
        if(_canCollectPickup)
        {
            if(_player != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
                Vector3 dist = _player.transform.position - transform.position;
                if(dist.magnitude <= 0.5f) StartCoroutine(CollectCoolDown());
            }
        }

        else transform.Translate (Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f) 
        {
            Destroy(this.gameObject);
        } 
    }

    IEnumerator CollectCoolDown()
    {
        yield return new WaitForSeconds(5f);
        _canCollectPickup = false;
    }

    public void  CanCollectPickup()
    {
        _canCollectPickup = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if (player != null)
            { 
                switch(powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    case 3:
                        player.addAmmo();
                        break;
                    case 4:
                        player.Plus1Life();
                        break;
                    case 5:
                        player.LoveShotActive();
                        break;
                    case 6:
                        player.AmmoCut();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                } 
            } 

            Destroy(this.gameObject);
        }

        if(other.tag == "EvilLaser")
        {
            _speed = 0f;
            Destroy(this.gameObject);
        }
    }
}
