using UnityChan;
using UnityEngine;
using System.Collections;

public class Item_Addinvincible : MonoBehaviour
{
    // [Obj/Item]インスペクター上のプレイヤーをアタッチ。
    private PlayerController _player = null;

    // [Item]アイテムの3D
    [SerializeField] private GameObject _item3d;

    // [Item]衝突判定用のコライダー
    private CapsuleCollider _capsuleCollider;

    [Header("効果")]
    // 効果時間 「触れている間のみ」にしたい場合でも設定してね。
    [SerializeField]
    private float _LimitTime = 0.1f;

    private void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        // アタッチされていない時に検索と格納
        if (_player == null)
            _player = GameObject.Find("unitychan").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == _player.name)
        {
            AddInvincible();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == _player.name)
        {
            RemoveInvincible();
        }
    }

    // 無敵オン！
    private void AddInvincible()
    {
        _player.invincible = true;
    }

    // 無敵オフ…
    private void RemoveInvincible()
    {
        _player.invincible = false;
    }

    // 無敵オフコルーチン
    private IEnumerator RemoveInvincibleCoroutine()
    {
        yield return new WaitForSeconds(_LimitTime);
        RemoveInvincible();
    }

    // アイテム取得時関数
    private void Gotton()
    {
        _item3d.SetActive(false);
        _capsuleCollider.enabled = false;
        StartCoroutine(RemoveInvincibleCoroutine());
    }
}
