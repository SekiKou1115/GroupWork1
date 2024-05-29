using System.Collections;
using System.Collections.Generic;
using UnityChan;
using UnityEngine;

public class Magma : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.GameOver();
        }
    }
}
