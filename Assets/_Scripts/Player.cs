using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    private float _speedMulitplier = 2f;

    [SerializeField]
    private float _reloadTime = 0.5f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _shieldVisual;

    [SerializeField]
    private GameObject _damageVisualOne;

    [SerializeField]
    private GameObject _damageVisualTwo;

    [SerializeField]
    private int _playerLives = 3;
    private bool _laserCanFire = true;

    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserAudioClip;
    private AudioSource _audioSource;

    [SerializeField]
    private int _score;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager  null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is  null");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on the Player is  null");
        }
        else
        {
            _audioSource.clip = _laserAudioClip;
        }

        transform.position = new Vector3(0, 0, 0);

        _damageVisualOne.SetActive(false);
        _damageVisualTwo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space) && _laserCanFire)
        {
            FireLaser();
        }

    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);



        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.3f)
        {
            transform.position = new Vector3(transform.position.x, -3.3f, 0);
        }


        if (transform.position.x >= 11f)
        {
            transform.position = new Vector3(-11f, transform.position.y, 0);
        }

        else if (transform.position.x <= -11f)
        {
            transform.position = new Vector3(11f, transform.position.y, 0);
        }
    }


    void FireLaser()
    {
        Vector3 offset = new Vector3(0, 1f, 0);

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position + offset, Quaternion.identity);
            StartCoroutine(DisableTripleShot());
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
        }

        _audioSource.Play();

        _laserCanFire = false;

        StartCoroutine(LaserReloadTime());
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisual.SetActive(false);
            return;
        }

        _playerLives--;

        _uiManager.UpdateLivesImage(_playerLives);

        if(_playerLives == 2)
        {
            _damageVisualOne.SetActive(true);
        }

        else if(_playerLives == 1)
        {
            _damageVisualTwo.SetActive(true);
        }

        else if (_playerLives == 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
        }
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
    }

    public void ActivateSpeedBoost()
    {
        _speed *= _speedMulitplier;
        StartCoroutine(DisableSpeedBoost());
    }

    public void ActivateShields()
    {
        _isShieldActive = true;
        _shieldVisual.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScoreText(_score);
    }

    IEnumerator LaserReloadTime()
    {
        yield return new WaitForSeconds(_reloadTime);
        _laserCanFire = true;
    }

    IEnumerator DisableTripleShot()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    IEnumerator DisableSpeedBoost()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMulitplier;
    }
}
