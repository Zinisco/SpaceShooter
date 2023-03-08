using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4f;

    private Player _player;

    [SerializeField]
    private GameObject _explosionObject;

    [SerializeField]
    private GameObject _shipObject;

    [SerializeField]
    private GameObject _laserPrefab;

    private bool _isEnemyAlive = true;

    private void OnEnable()
    {
        _explosionObject.SetActive(false);
        _shipObject.SetActive(true);
        StartCoroutine(EnemyFireLaser());
    }


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if(transform.position.y < -5)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7f, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: " + other.transform.name);
        if(other.gameObject.tag == "Player")
        {
            
            if (_player != null)
            {
                _player.Damage();
            }

            _isEnemyAlive = false;
            _explosionObject.SetActive(true);
            _shipObject.SetActive(false);
            _enemySpeed = 0;
            Destroy(gameObject,1.4f);
        }

        if(other.gameObject.tag == "Missile")
        {
            if (_player != null)
            {
                _player.AddScore(10);

            }

            _isEnemyAlive = false;
            _explosionObject.SetActive(true);
            _shipObject.SetActive(false);
            _enemySpeed = 0;
            Destroy(other.gameObject);
            Destroy(gameObject, 1.4f);
        }
    }

    public void DestroyEnemyByLaser()
    {
            if (_player != null)
            {
                _player.AddScore(10);

            }

            _isEnemyAlive = false;
            _explosionObject.SetActive(true);
            _shipObject.SetActive(false);
            _enemySpeed = 0;
            Destroy(GetComponent<BoxCollider>());
            Destroy(gameObject, 1.4f);
        
    }

    IEnumerator EnemyFireLaser()
    {
        while (_isEnemyAlive)
        {
            Vector3 offset = new Vector3(0, -1.7f, 0);
            float randomTime = Random.Range(3f, 6f);
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

            yield return new WaitForSeconds(randomTime);
        }
    }
}
