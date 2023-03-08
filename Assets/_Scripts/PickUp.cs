using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private float _pickUpSpeed = 3;

    [SerializeField] private int _pickUpID;

    [SerializeField] private AudioClip _clip;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _pickUpSpeed * Time.deltaTime);

        if(transform.position.y < - 5.5f)
        {
            Destroy(gameObject);
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
                    default:
                        Debug.Log("Default value");
                        break;
                }
               
            }

            Destroy(gameObject);
        }
    }
}
