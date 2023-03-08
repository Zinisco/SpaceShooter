using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField]
    private float _magnitude = 1f;
    [SerializeField]
    private float _duration = 0.3f;

    public void CameraShake()
    {
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        Vector3 _cameraPos = transform.position;

        float elapsedTime = 0;

        while(elapsedTime <= _duration)
        {
            float xValue = Random.Range(-0.5f, 0.5f) * _magnitude;
            float yValue = Random.Range(-0.5f, 0.5f) * _magnitude;

            transform.position = new Vector3(xValue, yValue, _cameraPos.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = _cameraPos;
    }


}
