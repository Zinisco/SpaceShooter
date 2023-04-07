using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateHelicopterMainMenu : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField]
    private float _amplitude = 1f;
    [SerializeField]
    private float _frequency = 1f;

    [SerializeField]
    private GameObject _propeller;

    // Update is called once per frame
    void Update()
    {
        
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * _frequency) * _amplitude, transform.position.z);

        _propeller.transform.localRotation = Quaternion.Euler(0, 0, Time.time * _speed);
    }



}
