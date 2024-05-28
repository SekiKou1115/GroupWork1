using UnityChan;
using UnityEngine;
using System.Collections;

public class Item_Addinvincible : MonoBehaviour
{
    // [Obj/Item]�C���X�y�N�^�[��̃v���C���[���A�^�b�`�B
    private PlayerController _player = null;

    // [Item]�A�C�e����3D
    [SerializeField] private GameObject _item3d;

    // [Item]�Փ˔���p�̃R���C�_�[
    private CapsuleCollider _capsuleCollider;

    [Header("����")]
    // ���ʎ��� �u�G��Ă���Ԃ̂݁v�ɂ������ꍇ�ł��ݒ肵�ĂˁB
    [SerializeField]
    private float _LimitTime = 0.1f;

    private void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        // �A�^�b�`����Ă��Ȃ����Ɍ����Ɗi�[
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

    // ���G�I���I
    private void AddInvincible()
    {
        _player.invincible = true;
    }

    // ���G�I�t�c
    private void RemoveInvincible()
    {
        _player.invincible = false;
    }

    // ���G�I�t�R���[�`��
    private IEnumerator RemoveInvincibleCoroutine()
    {
        yield return new WaitForSeconds(_LimitTime);
        RemoveInvincible();
    }

    // �A�C�e���擾���֐�
    private void Gotton()
    {
        _item3d.SetActive(false);
        _capsuleCollider.enabled = false;
        StartCoroutine(RemoveInvincibleCoroutine());
    }
}
