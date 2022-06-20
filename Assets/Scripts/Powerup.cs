using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]//ID FOR POWERUPS 0=trip shot 1=speed 2=shield
    private int powerupID;

    void Update()
    {
       transform.Translate (Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f) 
        {
           Destroy(this.gameObject);
        } 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
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
                        Debug.Log("Collected Shield");
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            
            Destroy(this.gameObject);
        }
    }
}
