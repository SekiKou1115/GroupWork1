using UnityChan;
using UnityEngine;
using System.Collections;

public class Item_Addinvincible : MonoBehaviour
{
    // [Item]インスペクター上のプレイヤーをアタッチ。
    private PlayerController _player = null;

    // [Item]アイテムの3D
    [SerializeField] 
    private GameObject _item3d;

    // [Item]衝突判定用のコライダー
    private CapsuleCollider _capsuleCollider;

    [Header("効果")]
    // 効果時間 「触れている間のみ」にしたい場合でも設定してね。
    [SerializeField]
    private float _LimitTime = 1f;

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

    // 無敵オン！
    private void AddInvincible()
    {
        if (_player.itemSpeed < 1)
            _player.itemSpeed = 1;
        if(_player.objectSpd < 1)
            _player.objectSpd = 1;
        _player.invincible = true;
        _player._invincibleEffect.Play();
        Gotton();
    }

    // 無敵オフ…
    private void RemoveInvincible()
    {
        _player.invincible = false;
        _player._invincibleEffect.Stop();
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
