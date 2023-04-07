using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4f;

    [SerializeField]
    private float _enemyID;

    private float _distance;
    [SerializeField]
    private float _ramSpeed = 5.0f;
    [SerializeField]
    private float _ramAttackRange = 4.0f;

    [SerializeField]
    private Transform _enemyLaserFirePoint;

    [SerializeField]
    private GameObject _explosionObject;

    [SerializeField]
    private GameObject _shipObject;

    [SerializeField]
    private GameObject _enemyLaserPrefab;

    [SerializeField]
    private GameObject _shield;

    private bool _isShieldActive = false;
    private bool _isEnemyAlive = true;
    private bool _canFireLaserBackwards = false;
    private bool _canFireAtPickUp = true;
    private bool _canDodge = false;

    private int randomEnemyMovement;

    private float _randomAngle;
    private float _randomDodge;

    private Vector3 _laserOffset;
    private SpawnManager _spawnManager;
    private Player _player;

    private void OnEnable()
    {
        _explosionObject.SetActive(false);
        _shipObject.SetActive(true);
        randomEnemyMovement = Random.Range(0, 2);
        _randomAngle = Random.Range(0, 2);
        _shield.gameObject.SetActive(false);

        switch (_enemyID)
        {
            case 0:
                StartCoroutine(EnemyFireLaser());
                break;
            case 1:
                break;
            case 2:
                StartCoroutine(EnemyFireLaser());
                _isShieldActive = true;
                _shield.gameObject.SetActive(true);
                break;
            case 3:
                _isShieldActive = true;
                _shield.gameObject.SetActive(true);
                break;
            default:
                Debug.Log("Default value");
                break;
        }
    }


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if (_player == null)
        {
            Debug.LogError("Player is Null");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            _distance = Vector3.Distance(_player.transform.position, gameObject.transform.position);
        }

        if (_enemyID == 0 || _enemyID == 2)
        {
            TrackPlayer();

            RaycastPickUp();

            BoxCastLaser();

            if (_spawnManager.waveNumber > 1)
            {

                switch (randomEnemyMovement)
                {
                    case 0:
                        EnemyMovementOne();
                        break;
                    case 1:
                        EnemyMovementTwo();
                        break;
                    default:
                        Debug.Log("Default value");
                        break;
                }
            }
            else
            {
                EnemyMovementOne();
            }
        }
        else if (_enemyID == 4)
        {
            RaycastPickUp();

            if (_distance <= _ramAttackRange)
            {
                RamPlayer();
            }
            else
            {
                EnemyMovementOne();
            }

        }
        else
        {
            EnemyMovementThree();
        }

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }

        if(_canDodge)
        {
            Vector3 dodgePath = transform.position + new Vector3(3,0,0);
            transform.Translate(dodgePath * _enemySpeed * Time.deltaTime);
            _canDodge = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            if (_player != null)
            {
                _player.Damage();
            }


            if (_isShieldActive)
            {
                _shield.gameObject.SetActive(false);
                _isShieldActive = false;
                return;
            }
            else
            {
                _spawnManager.EnemyDead();
                _isEnemyAlive = false;
                _explosionObject.SetActive(true);
                _shipObject.SetActive(false);
                _enemySpeed = 0;
                Destroy(gameObject, 1.4f);
            }
        }


        if (other.gameObject.tag == "Missile" || other.gameObject.tag == "Laser")
        {

            if (_player != null)
            {
                
                if(_enemyID == 2 || _enemyID == 3)
                {
                    _player.AddScore(40);
                }
                else if (_enemyID == 4)
                {
                    _player.AddScore(20);
                }
                else
                {
                    _player.AddScore(10);
                }
            }

            Destroy(other.gameObject);

            if (_isShieldActive)
            {
                _shield.gameObject.SetActive(false);
                _isShieldActive = false;
            }
            else
            {
                _spawnManager.EnemyDead();
                _isEnemyAlive = false;
                _explosionObject.SetActive(true);
                _shipObject.SetActive(false);
                _enemySpeed = 0;
                Destroy(gameObject, 1.4f);
            }

        }
    }

    public void EnemyMovementOne()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
    }

    public void EnemyMovementTwo()
    {
        if(_randomAngle == 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 30f);
        }
        else if(_randomAngle == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, -30f);
        }

        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

    }

    public void BoxCastLaser()
    {
        bool enemyBoxCast = Physics.BoxCast(transform.position, transform.localScale, transform.up * -1, Quaternion.identity, 1f, 1 << 8);

        if(enemyBoxCast)
        {
            Debug.Log("Hit Laser! Dodge!");
            _randomDodge = Random.Range(0, 6);
            if(_randomDodge == 0)
            {
                _canDodge = true;
            }
            else
            {
                _canDodge = false;
            }
        }
    }

    public void TrackPlayer()
    {
        if (_player != null)
        {
            if (transform.position.y > _player.transform.position.y)
            {
                _canFireLaserBackwards = false;
                _laserOffset = new Vector3(0, 1, 0);
            }
            else
            {
                _canFireLaserBackwards = true;
                _laserOffset = new Vector3(0, -1, 0);
            }
        }
    }

    public void RaycastPickUp()
    {
        Ray ray = new Ray(transform.position, transform.up * -1);

        if (Physics.Raycast(ray, 10, 1<<7))
        {
            if(_canFireAtPickUp)
            {
                Debug.Log("Fired laser at PickUp!");
                Instantiate(_enemyLaserPrefab, transform.position + _laserOffset, Quaternion.identity);
                _canFireAtPickUp = false;
            }
        }
    }

    public void EnemyMovementThree()
    {
        float amplitude = 2;
        float frequency = 2f;

        if (_isEnemyAlive)
        {
            transform.position = new Vector3(Mathf.Sin(Time.time * frequency) * amplitude, transform.position.y, transform.position.z);
            transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        }
    }

    public void RamPlayer()
    {
        if (_isEnemyAlive)
        {
            Vector3 direction = gameObject.transform.position - _player.transform.position;
            direction = direction.normalized;
            gameObject.transform.position -= direction * Time.deltaTime * _ramSpeed;
        }

    }

    IEnumerator EnemyFireLaser()
    {
        while (_isEnemyAlive)
        {
            if (_player != null)
            {
                GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position + _laserOffset, transform.rotation);
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

                if (_canFireLaserBackwards)
                {
                    for (int i = 0; i < lasers.Length; i++)
                    {
                        lasers[i].ReverseLaserDirection();
                    }
                }
                else
                {

                    for (int i = 0; i < lasers.Length; i++)
                    {
                        lasers[i].ForwardLaserDirection();
                    }
                }
            }

            yield return new WaitForSeconds(Random.Range(3f, 6f));
        }
    }
}
