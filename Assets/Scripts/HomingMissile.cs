using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private GameObject[] _enemy;
    private GameObject _closeEnemy;
    private int _speed = 6;

    void Start()
    {
        _enemy = GameObject.FindGameObjectsWithTag("Enemy");
        
        if(_enemy == null)
        {
            return;
        }

        FindEnemy();
    }

    void Update()
    {
        if (_closeEnemy != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _closeEnemy.transform.position, _speed * Time.deltaTime);
            Vector3 direction = (_closeEnemy.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float offset = -90f;
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        }
        else
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            
            if(transform.position.y > 8.0f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void FindEnemy()
    {
        float closestDist = Mathf.Infinity;

        foreach(GameObject en in _enemy)
        {
            float dist = Vector3.Distance(transform.position, en.transform.position);
            
            if(dist < closestDist)
            {
                closestDist = dist;
                _closeEnemy = en;
            }
        }
    }
}
