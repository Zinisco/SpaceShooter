using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 5f;

    private bool _isEnemyLaser = false;


    void Update()
    {
        if(_isEnemyLaser)
        {
            transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);

            if (transform.position.y < -8f)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }

                Destroy(gameObject);
            }
        }
        else
        {
            transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

            if (transform.position.y > 8f)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }

                Destroy(gameObject);
            }
        }

    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && _isEnemyLaser)
        {
            Player player = other.gameObject.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                Destroy(gameObject);
            }
        }

        if(other.gameObject.tag == "Enemy" && _isEnemyLaser == false)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if(enemy != null)
            {
                enemy.DestroyEnemyByLaser();
                Destroy(gameObject);
            }
        }
    }
}
