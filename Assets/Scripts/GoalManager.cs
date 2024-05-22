using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    //TextMeshProを使うときに必要
using UnityEngine.SceneManagement;  //シーン遷移するために必要
using UnityChan;

public class GoalManager : MonoBehaviour
{
    [SerializeField]
    GameObject _player;  //ユニティちゃん
    [SerializeField]
    GameObject _text;    //テキスト
    [SerializeField]
    bool _isGoal = false;    //ゴール判定

    void Update()
    {
        //ゴール後に画面をクリックした時
        if (_isGoal && Input.GetMouseButton(0))
            Restart();
    }

    //当たり判定
    void OnTriggerEnter(Collider other)
    {
        //衝突したオブジェクトの名前(other.name)が
        //ユニティちゃんの名前(player.name)と同じとき
        if (other.name == _player.name)
        {
            //テキスト内容の変更
            _text.GetComponent<TextMeshProUGUI>().text = "Goal!\nClick to Restart";
            //テキストを表示する
            _text.SetActive(true);

            //ユニティちゃんを動けなくする
            _player.GetComponent<UnityChanControlScriptWithRgidBody>().enabled = false;
            //アニメーションをオフにする
            _player.GetComponent<Animator>().enabled = false;

            //ゴール判定をtrueにする
            _isGoal = true;
        }
    }

    //現在のシーンを再読み込みする
    void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
