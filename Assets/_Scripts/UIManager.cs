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

    [SerializeField] private TMP_Text _ammoCountText;

    [SerializeField] private TMP_Text _waveCounterText;

    [SerializeField] private TMP_Text _warningText;

    [SerializeField]
    private Sprite[] _livesSprite;

    [SerializeField]
    private Image _livesImage;

    [SerializeField] private Slider _fuelVisualSlider;
    [SerializeField] private Slider _bossVisualSlider;

    private GameManager _gameManager;
    private SpawnManager _spawnManager;


    private float _timeLeft = 3f;
    private bool _startGame = false;

    // Start is called before the first frame update
    void Start()
    {
        _warningText.gameObject.SetActive(true);
        StartCoroutine(WarningTextRoutine());


        _startGame = true;
        _waveCounterText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _restartGameText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _warningText.gameObject.SetActive(false);
        _scoreText.text = "Score: " + 0;
        _ammoCountText.text = "Ammo: " + 25 + " / 25";
        _fuelVisualSlider.value = 10;


        _bossVisualSlider.gameObject.SetActive(false);
        _bossVisualSlider.value = 30;

        if (_gameManager == null)
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
                _spawnManager.StartGame();
            }
        }
    }

    public void UpdateFuelAmount(float fuelAmount)
    {
        _fuelVisualSlider.value = fuelAmount;
    }

    public void UpdateBossHealth(float bossHealth)
    {
        _bossVisualSlider.value = bossHealth;
    }

    public void UpdateScoreText(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateAmmoCount(int playerAmmoCount)
    {
        _ammoCountText.text = "Ammo: " + playerAmmoCount.ToString() + " / 25";
    }

    public void UpdateWaveStartDisplay(int currentWave)
    {
        _waveCounterText.gameObject.SetActive(true);
        _waveCounterText.text = "Wave: " + currentWave;
        StartCoroutine(DisableWaveTextRoutine());
    }

    public void DisplayBossHealth()
    {
        _bossVisualSlider.gameObject.SetActive(true);
    }

    public void UpdateLivesImage(int currentPlayerLives)
    {
        _livesImage.sprite = _livesSprite[currentPlayerLives];

        if(currentPlayerLives == 0)
        {
            StartCoroutine(GameOverFlicker());
            _restartGameText.gameObject.SetActive(true);
            _gameManager.GameOver();
        }
    }

    public void OnBossDeath()
    {
        StartCoroutine(GameOverFlicker());
        _restartGameText.gameObject.SetActive(true);
        _gameManager.GameOver();
    }

    private IEnumerator DisableWaveTextRoutine()
    {
        yield return new WaitForSeconds(3f);
        _waveCounterText.gameObject.SetActive(false);
    }

    public IEnumerator GameOverFlicker()
    {
      while(true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator WarningTextRoutine()
    {
        yield return new WaitForSeconds(3f);
        _warningText.gameObject.SetActive(false);
    }
}
