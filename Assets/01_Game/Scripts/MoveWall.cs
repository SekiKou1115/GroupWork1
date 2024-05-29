using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityChan;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Wall : MonoBehaviour
{
    [SerializeField] private float _waitTime;

    [SerializeField] private GameObject _object;
    [SerializeField] private float _speed;
    [SerializeField] private float _duration;
    [SerializeField] private Ease _ease;

    private async void Start()
    {
        //  初期設定
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 5000, sequencesCapacity: 200);

        //  キャンセル処理を実装したタスク
        var delayTask = UniTask.Delay(TimeSpan.FromSeconds(_waitTime), cancellationToken: this.destroyCancellationToken);
        if (await delayTask.SuppressCancellationThrow()) { return; }

        var moveTask = MoveTask(this.destroyCancellationToken);
        if (await moveTask.SuppressCancellationThrow()) { return; }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.GameOver();
            Debug.Log("ダメージを受けた");
        }
    }

    private async UniTask MoveTask(CancellationToken ct)
    {
        await transform.DOMove(new Vector3(0, _object.transform.position.y, _object.transform.position.z), _duration)
            .SetLink(gameObject)
            .SetEase(_ease)
            .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
    }
}
