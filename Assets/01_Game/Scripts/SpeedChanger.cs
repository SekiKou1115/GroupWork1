using UnityChan;
using UnityEngine;
using System.Collections;

public class SpeedChanger : MonoBehaviour
{
    // [Obj/Item]インスペクター上のプレイヤーをアタッチ。
    private PlayerController _player = null;

    // [Item]アイテムの3D
    [SerializeField] private GameObject _item3d;

    // [Item]衝突判定用のコライダー
    private CapsuleCollider _capsuleCollider;

    enum ObjectType
    {
        Item,
        Object,
    }
    [Header("このオブジェクトの種類"), SerializeField]
    ObjectType objectType;

    // バフorデバフ スタート時に自動設定
    private bool _isBuff = false;

    [Header("効果")]
    // 威力 プレイヤーの基礎Speedに乗算
    [SerializeField]
    private float _changeSpeed;
    // 効果時間 「触れている間のみ」にしたい場合でも設定してね。
    [SerializeField] 
    private float _LimitTime = 0.1f;

    private void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        // アタッチされていない時に検索と格納
        if (_player == null)
        _player = GameObject.Find("unitychan").GetComponent<PlayerController>();
        // バフorデバフ判定
        if (_changeSpeed >= 1)
        {
            _isBuff = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == _player.name)
        {
            ChangeSpeed(objectType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == _player.name)
        {
            ResetSpeed(objectType);
        }
    }

    // スピード代入関数
    private void ChangeSpeed(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Item:
                _player.itemSpd = _changeSpeed;
                Gotton();
                break;
            case ObjectType.Object:
                if(_isBuff)
                    _player.objectSpd = _changeSpeed;
                else if(!_isBuff && !_player.invincible)
                    _player.objectSpd = _changeSpeed;
                break;
            default:
                break;
        }
    }

    // スピードリセット関数
    private void ResetSpeed(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Item:
                _player.itemSpd = 1;
                break;
            case ObjectType.Object:
                _player.objectSpd = 1;
                break;
            default:
                break;
        }
    }

    // スピードリセットコルーチン
    private IEnumerator ResetSpeedCoroutine(ObjectType type)
    {
        yield return new WaitForSeconds(_LimitTime);
        ResetSpeed(type);
    }

    // アイテム取得時関数
    private void Gotton()
    {
        _item3d.SetActive(false);
        _capsuleCollider.enabled = false;
        StartCoroutine(ResetSpeedCoroutine(objectType));
    }
}
