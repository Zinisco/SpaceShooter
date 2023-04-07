using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private int _bossHealth = 30;

    [SerializeField]
    private int _bossSpeed = 3;

    [SerializeField]
    private GameObject _enemyLaserPrefab;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private GameObject _bossVisual;

    [SerializeField]
    private Transform _laserFirePointOne;
    [SerializeField]
    private Transform _laserFirePointTwo;

    private Vector3 _currentPos;
    [SerializeField]
    private Vector3 _endPos;
    [SerializeField]
    private Vector3 _firstPoint;
    [SerializeField]
    private Vector3 _secondPoint;

    private bool _movedRight = false;
    private bool _bossCanFireLaser = true;
    private bool _bossIsDead = false;

    [SerializeField]
    private AudioClip _bossMusic;

    private Player _player;
    private UIManager _uIManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;

    enum BossState
    {
        Start,
        Idle,
        Moving,
        Death
    }

    private BossState state;

    // Start is called before the first frame update
    void Start()
    {

        _bossVisual.gameObject.SetActive(true);
        _explosionPrefab.gameObject.SetActive(false);

        _audioSource = GameObject.Find("Background Music").GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uIManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        _audioSource.clip = _bossMusic;
        _audioSource.Play();
        _audioSource.volume = 0.8f;

        if (_player == null)
        {
            Debug.LogError("Player is Null");
        }

        if(_uIManager == null)
        {
            Debug.LogError("UI_Mananger is Null");
        }

        _currentPos = transform.position;
        state = BossState.Start;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case BossState.Start:
                BossStart();
                break;
            case BossState.Idle:
                BossIdle();
                break;
            case BossState.Moving:
                BossMoving();
                break;
            case BossState.Death:
                BossDeath();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Laser" || other.gameObject.tag == "Missile")
        {
            if (_bossHealth > 0)
            {
                _bossHealth--;
                _uIManager.UpdateBossHealth(_bossHealth);
            }

            if (_bossHealth == 0)
            {
                state = BossState.Death;
            }

            Destroy(other.gameObject);
        }
    }

    private void BossStart()
    {
        _uIManager.DisplayBossHealth();
      
        if (transform.position != _endPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, _endPos, _bossSpeed * Time.deltaTime);
        }
        else
        {
            state = BossState.Idle;
        }
    }

    private void BossIdle()
    {
        StartCoroutine(IdleRoutine(3));
    }

    private void BossMoving()
    {
        if(_movedRight == false)
        {
            MoveRight();
        }
        else
        {
            MoveLeft();
        }
    }

    private void MoveRight()
    {
        if(transform.position != _firstPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, _firstPoint, 1f * Time.deltaTime);
            BossLaserFiring();
        }
        else
        {
            _movedRight = true;
            state = BossState.Idle;
        }
    }

    private void MoveLeft()
    {
        if (transform.position != _secondPoint)
        {
            BossLaserFiring();
            transform.position = Vector3.MoveTowards(transform.position, _secondPoint, 1f * Time.deltaTime);
        }
        else
        {
            _movedRight = false;
            state = BossState.Idle;
        }
    }

    private void BossLaserFiring()
    {
        if(_bossCanFireLaser)
        {
            _bossCanFireLaser = false;
            Instantiate(_enemyLaserPrefab, _laserFirePointOne.transform.position, Quaternion.identity);
            Instantiate(_enemyLaserPrefab, _laserFirePointTwo.transform.position, Quaternion.identity);
            StartCoroutine(BossFireLaser());
        }
    }

    private void BossDeath()
    {
        if (_bossIsDead == false)
        {
            _bossVisual.gameObject.SetActive(false);
            _explosionPrefab.gameObject.SetActive(true);
            _uIManager.OnBossDeath();
            _player.AddScore(2000);
            _bossIsDead = true;
            _player.NoSpeed();
            _spawnManager.BossDeath();
        }
    }

    private IEnumerator IdleRoutine(int timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        state = BossState.Moving;
    }

    private IEnumerator BossFireLaser()
    {
        yield return new WaitForSeconds(0.5f);
        _bossCanFireLaser = true;
    }
}
