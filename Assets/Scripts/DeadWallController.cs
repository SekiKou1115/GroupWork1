using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityChan;

public class DeadWallController : MonoBehaviour
{
    [SerializeField]
    float _speed = 0.05f;
    [SerializeField]
    float _max_x = 10.0f;
    [SerializeField]
    bool _isDirection;

    [SerializeField]
    GameObject _player;
    [SerializeField]
    GameObject _text;

    [SerializeField]
    bool _isGameOver = false;   //ゲームオーバー判定

    void Update()
    {
        if (_isDirection)
            gameObject.transform.Translate((-_speed), 0, 0);
        else
            gameObject.transform.Translate(_speed, 0, 0);

        if (gameObject.transform.position.x > _max_x || gameObject.transform.position.x < (-_max_x))
            _speed *= -1;

        if (_isGameOver && Input.GetMouseButton(0))
            Restart();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == _player.name)
        {
            _text.GetComponent<TextMeshProUGUI>().text = "GameOver...\nClick to Restart";
            _text.SetActive(true);

            _player.GetComponent<UnityChanControlScriptWithRgidBody>().enabled = false;
            _player.GetComponent<Animator>().enabled = false;

            //ゲームオーバー判定をtrueにする
            _isGameOver = true;
        }
    }

    //現在のシーンを再読み込みする
    void Restart()
    {
        SceneManager.LoadScene(0);
    }
}