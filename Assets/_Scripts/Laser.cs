using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 5f;

    private bool laserIsReverse = false;

    private Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if(_player == null)
        {
            Debug.LogError("Player is Null");
        }
    }

    void Update()
    {
        if (gameObject.tag == "EnemyLaser" && laserIsReverse == false)
        {
            transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);
        }
        else if(gameObject.tag == "EnemyLaser" && laserIsReverse)
        {
            transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameObject.tag == "EnemyLaser")
        {
            if (_player != null)
            {
                _player.Damage();
                Destroy(gameObject);
            }
        }
    }

    public void ReverseLaserDirection()
    {
        laserIsReverse = true;
    }

    public void ForwardLaserDirection()
    {
        laserIsReverse = false;
    }
}
