using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateExplosion : MonoBehaviour
{
    [SerializeField] private Animator _anim;

    private void OnEnable()
    {
        _anim.GetComponent<Animator>();

        if(_anim == null)
        {
            Debug.LogError("Animator is Null");
        }

        _anim.SetTrigger("Explode");
    }
}
