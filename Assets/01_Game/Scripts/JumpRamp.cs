using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRamp : MonoBehaviour
{
    [SerializeField] private float jumpForce = 20.0f;

    private void OnTriggerEnter(Collider other)
    {
        // Player ���G�ꂽ��
        if (other.gameObject.CompareTag("Player"))
        {
            // �G�ꂽ�����Rigidbody���擾���āA������ɗ͂�������
            other.gameObject.GetComponent<Rigidbody>().
                AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }
    }
}