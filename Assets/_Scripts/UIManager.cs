using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    [SerializeField] private TMP_Text _gameOverText;

    [SerializeField] private TMP_Text _restartGameText;

    [SerializeField] private TMP_Text _countdownTimerText;

    [SerializeField]
    private Sprite[] _livesSprite;

    [SerializeField]
    private Image _livesImage;

    private GameManager _gameManager;
    private SpawnManager _spawnManager;


    private float _timeLeft = 3f;
    private bool _startGame = false;

    // Start is called before the first frame update
    void Start()
    {
        _startGame = true;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _restartGameText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _scoreText.text = "Score: " + 0;

        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is null");
        }

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_startGame == true)
        {
            _timeLeft -= Time.deltaTime;
            _countdownTimerText.text = (_timeLeft).ToString("0");

            if(_timeLeft < 1)
            {
                _countdownTimerText.enabled = false;
                _startGame = false;
                _spawnManager.StartSpawning();
            }
        }

    }

    public void UpdateScoreText(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLivesImage(int currentPlayerLives)
    {
        _livesImage.sprite = _livesSprite[currentPlayerLives];

        if(currentPlayerLives == 0)
        {
            _gameOverText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlicker());
            _restartGameText.gameObject.SetActive(true);
            _gameManager.GameOver();
        }
    }

    IEnumerator GameOverFlicker()
    {
      while(true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
