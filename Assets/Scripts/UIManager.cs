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
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
  
    void Start()
    {
        _scoreText.text = "Score:" + 0;
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score:" + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {

        _LivesImg.sprite = _liveSprites[currentLives];
        //display img sprite = current amount of lives 0-3
    }
}
