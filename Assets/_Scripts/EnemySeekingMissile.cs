using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeekingMissile : MonoBehaviour
{
    [SerializeField] private float _missileSpeed;
    private Enemy _enemy;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = FindObjectOfType<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_enemy != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, _enemy.transform.position, _missileSpeed * Time.deltaTime);
            transform.up = _enemy.transform.position - transform.position;
        }
        else if(_enemy == null)
        {
            transform.Translate(Vector3.up * _missileSpeed * Time.deltaTime);
            Destroy(gameObject, 3f);
        }
    }
}
