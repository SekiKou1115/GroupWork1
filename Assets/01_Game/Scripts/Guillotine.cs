using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityChan;
using UnityEngine;

public class Guillotine : MonoBehaviour
{
    [SerializeField] private float _startWaitTime;
    [SerializeField] private float _waitTime;

    [SerializeField] private float _downTime;
    [SerializeField] private Ease _downEase;
    [SerializeField] private float _upTime;
    [SerializeField] private Ease _upEase;
    [SerializeField] private GameObject[] _pos;


    private async void Start()
    {
        gameObject.transform.position = _pos[1].transform.position;

        //  初期設定
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 5000, sequencesCapacity: 200);

        //  キャンセル処理を実装したタスク
        var delayTask = UniTask.Delay(TimeSpan.FromSeconds(_startWaitTime), cancellationToken: this.destroyCancellationToken);
        if (await delayTask.SuppressCancellationThrow()) { return; }

        var moveTask = MoveTask(this.destroyCancellationToken);
        if (await moveTask.SuppressCancellationThrow()) { return; }
    }

    private async UniTask MoveTask(CancellationToken ct)
    {
        while (true)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_startWaitTime));
            await DownMoveTask(ct);
            await UniTask.Delay(TimeSpan.FromSeconds(_waitTime));
            await UpMoveTask(ct);
            await UniTask.Delay(TimeSpan.FromSeconds(_waitTime));
        }
    }
    private async UniTask DownMoveTask(CancellationToken ct)
    {
        await transform.DOMove(_pos[0].transform.position, _downTime)
            .SetLink(gameObject)
            .SetEase(_downEase)
            .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
    }
    private async UniTask UpMoveTask(CancellationToken ct)
    {
        await transform.DOMove(_pos[1].transform.position, _upTime)
            .SetLink(gameObject)
            .SetEase(_upEase)
            .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken: ct);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.KnockBack();
            Debug.Log("ダメージを受けた");
        }
    }
}