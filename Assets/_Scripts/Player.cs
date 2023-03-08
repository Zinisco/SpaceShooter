using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    private float _speedMultiplier = 1f;

    [SerializeField]
    private float _reloadTime = 0.5f;
    [SerializeField]
    private int _ammoCount = 15;
    private float _fuel = 10f;

    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _missilePrefab; 

    [SerializeField]
    private GameObject _shieldVisual;

    [SerializeField]
    private GameObject _damageVisualOne;

    [SerializeField]
    private GameObject _damageVisualTwo;

    [SerializeField]
    private int _playerLives = 3;
    [SerializeField]
    private int _shieldLives = 3;
    private bool _bulletCanFire = true;

    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;
    private bool _isBoosterActive = false;
    private bool _isMissileActive = false;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private CameraManager _cameraManager;

    [SerializeField]
    private AudioClip _laserAudioClip;
    private AudioSource _audioSource;

    [SerializeField]
    private int _score;

    [SerializeField]
    private Material[] _shieldMaterials;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _cameraManager = GameObject.Find("Main Camera").GetComponent<CameraManager>();
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
        PlayerControlledBooster();

        if (Input.GetKeyDown(KeyCode.Space) && _bulletCanFire)
        {
            FireBullet();
        }

    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);    
      
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

        transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);
    }

    void PlayerControlledBooster()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isBoosterActive = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isBoosterActive = false;
        }

        if(_isBoosterActive && _fuel > 0)
        {
            _fuel -= Time.deltaTime;
            _uiManager.UpdateFuelAmount(_fuel);
            _speedMultiplier = 2f;
        }
        else if(_isBoosterActive == false)
        {
            _speedMultiplier = 1f;
        }
    }

    void FireBullet()
    {

        Vector3 offset = new Vector3(0, 1f, 0);

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position + offset, Quaternion.identity);
            StartCoroutine(DisableTripleShot());
        }

        else if(_isMissileActive)
        {
            Instantiate(_missilePrefab, transform.position + offset, Quaternion.identity);
            StartCoroutine(DisableMissile());
        }

        else
        {
            if (_ammoCount > 0)
            {
                _ammoCount--;
                _uiManager.UpdateAmmoCount(_ammoCount);
                Instantiate(_bulletPrefab, transform.position + offset, Quaternion.identity);
            }
        }

        _audioSource.Play();

      _bulletCanFire = false;

       StartCoroutine(BulletReloadTime());


    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _shieldLives--;

            if (_shieldLives == 2)
            {
                _shieldVisual.gameObject.GetComponent<MeshRenderer>().material = _shieldMaterials[1];
            }
            else if (_shieldLives == 1)
            {
                _shieldVisual.gameObject.GetComponent<MeshRenderer>().material = _shieldMaterials[2];
            }
            else if (_shieldLives == 0)
            {
                _isShieldActive = false;
                _shieldVisual.SetActive(false);
            }

        }
        else
        {
            _playerLives--;
            _cameraManager.CameraShake();
        }

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

    public void ActivateMissile()
    {
        _isMissileActive = true;
    }

    public void HealPlayer()
    {
        if (_playerLives < 3)
        {
            _playerLives++;
        }

        _uiManager.UpdateLivesImage(_playerLives);

        if (_playerLives == 3)
        {
            _damageVisualOne.SetActive(false);
        }

        else if (_playerLives == 2)
        {
            _damageVisualTwo.SetActive(false);
        }

    }

    public void RefillSpeedBoost()
    {
        _fuel = 10f;
        _uiManager.UpdateFuelAmount(_fuel);
    }

    public void ActivateShields()
    {
        if (_isShieldActive == false)
        {
            _isShieldActive = true;
            _shieldVisual.SetActive(true);
            _shieldVisual.gameObject.GetComponent<MeshRenderer>().material = _shieldMaterials[0];
            _shieldLives = 3;
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScoreText(_score);
    }

    public void Reload()
    {
        _ammoCount = 15;
        _uiManager.UpdateAmmoCount(_ammoCount);
    }

    IEnumerator BulletReloadTime()
    {
        yield return new WaitForSeconds(_reloadTime);
        _bulletCanFire = true;
    }

    IEnumerator DisableTripleShot()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    IEnumerator DisableMissile()
    {
        yield return new WaitForSeconds(5.0f);
        _isMissileActive = false;
    }

}
