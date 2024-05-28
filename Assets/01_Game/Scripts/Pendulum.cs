using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    [SerializeField] GameObject _fulcrum;
    [SerializeField] float _speed;

    void Update()
    {
        _fulcrum.transform.Rotate(0, 0, _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ƒ_ƒ[ƒW‚ğó‚¯‚½");
        }
    }
}
