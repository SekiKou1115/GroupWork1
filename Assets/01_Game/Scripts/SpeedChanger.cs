using UnityChan;
using UnityEngine;
using System.Collections;

public class SpeedChanger : MonoBehaviour
{
    // [Obj/Item]�C���X�y�N�^�[��̃v���C���[���A�^�b�`�B
    private PlayerController _player = null;

    // [Item]�A�C�e����3D
    [SerializeField] private GameObject _item3d;

    // [Item]�Փ˔���p�̃R���C�_�[
    private CapsuleCollider _capsuleCollider;

    enum ObjectType
    {
        Item,
        Object,
    }
    [Header("���̃I�u�W�F�N�g�̎��"), SerializeField]
    ObjectType objectType;

    // �o�tor�f�o�t �X�^�[�g���Ɏ����ݒ�
    private bool _isBuff = false;

    [Header("����")]
    // �З� �v���C���[�̊�bSpeed�ɏ�Z
    [SerializeField]
    private float _changeSpeed;
    // ���ʎ��� �u�G��Ă���Ԃ̂݁v�ɂ������ꍇ�ł��ݒ肵�ĂˁB
    [SerializeField] 
    private float _LimitTime = 0.1f;

    private void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        // �A�^�b�`����Ă��Ȃ����Ɍ����Ɗi�[
        if (_player == null)
        _player = GameObject.Find("unitychan").GetComponent<PlayerController>();
        // �o�tor�f�o�t����
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

    // �X�s�[�h����֐�
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

    // �X�s�[�h���Z�b�g�֐�
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

    // �X�s�[�h���Z�b�g�R���[�`��
    private IEnumerator ResetSpeedCoroutine(ObjectType type)
    {
        yield return new WaitForSeconds(_LimitTime);
        ResetSpeed(type);
    }

    // �A�C�e���擾���֐�
    private void Gotton()
    {
        _item3d.SetActive(false);
        _capsuleCollider.enabled = false;
        StartCoroutine(ResetSpeedCoroutine(objectType));
    }
}
