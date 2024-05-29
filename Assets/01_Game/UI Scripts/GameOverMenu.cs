using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void Restart()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName);
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }
}
