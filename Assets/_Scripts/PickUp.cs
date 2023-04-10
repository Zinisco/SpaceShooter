using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private float _pickUpSpeed = 3;

    [SerializeField] private int _pickUpID;

    [SerializeField] private AudioClip _clip;

    [SerializeField] private GameObject _pickupVisual;

    [SerializeField]
    private GameObject _explosionObject;

    private float _moveSpeed = 5;

    private Player _player;
    private Vector3 directionOfPlayer;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _explosionObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        directionOfPlayer = (gameObject.transform.position - _player.gameObject.transform.position).normalized;

        if (transform.position.y < - 5.5f)
        {
            Destroy(gameObject);
        }

        if(Input.GetKey(KeyCode.C))
        {
            MoveTowardsPlayer();
        }
        else
        {
            transform.Translate(Vector3.down * _pickUpSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (_player != null)
            {
                switch(_pickUpID)
                {
                    case 0:
                        _player.ActivateTripleShot();
                        break;
                    case 1: 
                        _player.RefillSpeedBoost();
                        break;
                    case 2:
                        _player.ActivateShields();
                        break;
                    case 3:
                        _player.Reload();
                        break;
                    case 4:
                        _player.HealPlayer();
                        break;
                    case 5:
                        _player.ActivateMissile();
                        break;
                    case 6:
                        _player.ReduceSpeed();
                        break;
                    default:
                        Debug.Log("Default value");
                        break;
                }
               
            }
            else
            {
                Debug.LogError("Player is Null");
            }

            Destroy(gameObject);
        }

        if(other.gameObject.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
            _explosionObject.SetActive(true);
            _pickupVisual.SetActive(false);
            _pickUpSpeed = 0;
            _moveSpeed = 0;
            Destroy(gameObject, 1.4f);
        }
    }

    public void MoveTowardsPlayer()
    {
        gameObject.transform.position -= directionOfPlayer * Time.deltaTime * _moveSpeed;
    }
}
