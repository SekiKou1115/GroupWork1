using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityChan;
using UnityEngine.SceneManagement;

public class NewGoalManager : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _clear;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UnlockStage();
            Finish();

            _player.GetComponent<Animator>().enabled = false;

            PlayerController.Instance.GameClear();

        }
    }

    private void Finish()
    {
        _clear.SetActive(true);
    }

    void UnlockStage()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("unlockedStage", PlayerPrefs.GetInt("unlockedStage", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}
