using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FallingFloor : MonoBehaviour
{
    [SerializeField] private float _waitTime;
    private Rigidbody _rigidBody;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(gameObject.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }

    private async void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var objectFall = ObjectFall(this.destroyCancellationToken);
            if (await objectFall.SuppressCancellationThrow()) { return; }
        }
    }

    private async UniTask ObjectFall(CancellationToken ct)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_waitTime));

        _rigidBody.useGravity = true;
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotation
            | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        await UniTask.Delay(TimeSpan.FromSeconds(_waitTime));
        Destroy(gameObject);
    }
}