using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] float _startWaitTime;

    [SerializeField] GameObject[] _pos;
    [SerializeField] float _duration;
    [SerializeField] Ease _ease;
    [SerializeField] float _stayTime;

    private async void Start()
    {
        gameObject.transform.position = _pos[0].transform.position;

        //  初期設定
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 5000, sequencesCapacity: 200);

        //  キャンセル処理を実装したタスク
        var delayTask = UniTask.Delay(TimeSpan.FromSeconds(_startWaitTime), cancellationToken: this.destroyCancellationToken);
        if (await delayTask.SuppressCancellationThrow()) { return; }

        var moveTasks = MoveTasks(this.destroyCancellationToken);
        if (await moveTasks.SuppressCancellationThrow()) { return; }
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

    private async UniTask MoveTasks(CancellationToken ct)
    {
        while (true)
        {
            foreach (var i in _pos)
            {
                await transform.DOMove(i.transform.position, _duration)
                    .SetLink(gameObject)
                    .SetEase(_ease)
                    .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, ct);

                await UniTask.Delay(TimeSpan.FromSeconds(_stayTime));
            }
        }
    }
}
