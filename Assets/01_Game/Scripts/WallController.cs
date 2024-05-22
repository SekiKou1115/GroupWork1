using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    [SerializeField]
    float _speed = 0.05f;    //��Q���̑��x
    [SerializeField]
    float _max_x = 10.0f;   //��Q���̉��ړ��̍ő勗��
    [SerializeField]
    bool _isDirection;    //true:���@false:�E

    void Update()
    {
        if(_isDirection)
            //_speed�̒l�������t���[�����ɉ��ړ�����
            gameObject.transform.Translate((-_speed), 0, 0);
        else
            //_speed�̒l�������t���[�����ɉ��ړ�����
            gameObject.transform.Translate(_speed, 0, 0);

        //Transform��x�̒l�����l�𒴂�����ړ������𔽓]����
        if (gameObject.transform.position.x > _max_x || gameObject.transform.position.x < (-_max_x))
            _speed *= -1;
    }
}