using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    [SerializeField]
    float _speed = 0.05f;    //障害物の速度
    [SerializeField]
    float _max_x = 10.0f;   //障害物の横移動の最大距離
    [SerializeField]
    bool _isDirection;    //true:左　false:右

    void Update()
    {
        if(_isDirection)
            //_speedの値分だけフレーム毎に横移動する
            gameObject.transform.Translate((-_speed), 0, 0);
        else
            //_speedの値分だけフレーム毎に横移動する
            gameObject.transform.Translate(_speed, 0, 0);

        //Transformのxの値が一定値を超えたら移動方向を反転する
        if (gameObject.transform.position.x > _max_x || gameObject.transform.position.x < (-_max_x))
            _speed *= -1;
    }
}