using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private TMP_Text _gameOverText;
    [SerializeField]
    private TMP_Text _restartText;
    [SerializeField]
    private TMP_Text _AmmoText;
    [SerializeField]
    private TMP_Text _waveText;
    [SerializeField]
    private int _waveNumber = 0;

    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private Slider _thrusterSlider;

    void Start()
    {
        _scoreText.text = "Score:" + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _AmmoText.text = "Ammo:" + 15; //start of game Ammo is 15
        _waveText.text = "";

        if(_gameManager == null)
        {
            Debug.LogError("GameManager is Null");
        }
    }

    public void GameStarted()
    {
        StartCoroutine(WaveDisplay());
    }

    IEnumerator WaveDisplay()
    {
        _waveNumber ++;
        _waveText.text = "WAVE " + _waveNumber;
        yield return new WaitForSeconds(5.0f);
        _waveText.text = "";
    }  

    public void UpdateThrusterFuel(float value)
    {
        _thrusterSlider.value = value;
    }
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score:" + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];
        //display img sprite = current amount of lives 0-3

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void UpdateAmmo(int currentAmmo) //updates current ammo text
    {
        _AmmoText.text = "Ammo:" + currentAmmo.ToString() + "/15"; //gets current ammo from player and add it to our counter
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
    }
}
