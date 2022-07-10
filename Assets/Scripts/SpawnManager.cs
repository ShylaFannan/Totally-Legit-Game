using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;
    private bool _stopSpawning = false;
    private bool _playerIsAlive = false;

    private int _waveCount = 1;
    private int _waveMax = 3;
    private int _enemiesInWave = 3;
    private int _enemiesSpawned = 0;
    private int _enemiesAlive = 0;

    [SerializeField]
    private UIManager _uiManager;
    private IEnumerator _enemyRoutine;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL!!");
        }
    }

    public void StartSpawning()
    {
        _playerIsAlive = true;
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerupRoutine());
        _uiManager.GameStarted();
    }

    IEnumerator SpawnEnemy()
    {
        Debug.Log("This actually ran");
        yield return new WaitForSeconds(3.0f);

        while (_enemiesSpawned < _enemiesInWave && _playerIsAlive == true)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemiesSpawned ++;
            _enemiesAlive ++;
            yield return new WaitForSeconds(5.0f); 
        }
        if (_enemiesSpawned == _enemiesInWave)
        {
            _enemiesSpawned = 0;
            NextWave();
        
        }    
    }

    private void NextWave()
    {
        
        _enemiesInWave += 2;
        StartCoroutine(SpawnEnemy());
        _uiManager.GameStarted();
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 7);
            Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
        _playerIsAlive = false;
        Destroy(_enemyContainer);
    }

    public void EnemyKilled()
    {
        _enemiesAlive --;
    }
}

