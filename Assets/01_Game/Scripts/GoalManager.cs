using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    //TextMeshPro���g���Ƃ��ɕK�v
using UnityEngine.SceneManagement;  //�V�[���J�ڂ��邽�߂ɕK�v
using UnityChan;

public class GoalManager : MonoBehaviour
{
    [SerializeField]
    GameObject _player;  //���j�e�B�����
    [SerializeField]
    GameObject _text;    //�e�L�X�g
    [SerializeField]
    bool _isGoal = false;    //�S�[������

    void Update()
    {
        //�S�[����ɉ�ʂ��N���b�N������
        if (_isGoal && Input.GetMouseButton(0))
            Restart();
    }

    //�����蔻��
    void OnTriggerEnter(Collider other)
    {
        //�Փ˂����I�u�W�F�N�g�̖��O(other.name)��
        //���j�e�B�����̖��O(player.name)�Ɠ����Ƃ�
        if (other.name == _player.name)
        {
            //�e�L�X�g���e�̕ύX
            _text.GetComponent<TextMeshProUGUI>().text = "Goal!\nClick to Restart";
            //�e�L�X�g��\������
            _text.SetActive(true);

            //���j�e�B�����𓮂��Ȃ�����
            _player.GetComponent<UnityChanControlScriptWithRgidBody>().enabled = false;
            //�A�j���[�V�������I�t�ɂ���
            _player.GetComponent<Animator>().enabled = false;

            //�S�[�������true�ɂ���
            _isGoal = true;
        }
    }

    //���݂̃V�[�����ēǂݍ��݂���
    void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
