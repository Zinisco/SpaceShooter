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

    private void Start()
    {
        _explosionObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if(transform.position.y < - 5.5f)
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
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {
                switch(_pickUpID)
                {
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1: 
                        player.RefillSpeedBoost();
                        break;
                    case 2:
                        player.ActivateShields();
                        break;
                    case 3:
                        player.Reload();
                        break;
                    case 4:
                        player.HealPlayer();
                        break;
                    case 5:
                        player.ActivateMissile();
                        break;
                    case 6:
                        player.ReduceSpeed();
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
        Vector3 direction = gameObject.transform.position - _player.transform.position;
        direction = direction.normalized;
        gameObject.transform.position -= direction * Time.deltaTime * _moveSpeed;
    }
}
