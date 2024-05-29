using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu;

    void Start()
    {
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Pause();
        }
    }
   public void Pause()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

   public void Home()
    {
        SceneManager.LoadScene("Title");
        Time.timeScale = 1;
    }

   public void Resume()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName);
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }

}
