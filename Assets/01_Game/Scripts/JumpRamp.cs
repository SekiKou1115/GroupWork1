using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRamp : MonoBehaviour
{
    [SerializeField] private float jumpForce = 20.0f;

    private void OnTriggerEnter(Collider other)
    {
        // Player が触れた時
        if (other.gameObject.CompareTag("Player"))
        {
            // 触れた相手のRigidbodyを取得して、上向きに力を加える
            other.gameObject.GetComponent<Rigidbody>().
                AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }
    }
}