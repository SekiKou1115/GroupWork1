using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearMenu : MonoBehaviour
{
    [SerializeField] GameObject _clearMenu;
    [SerializeField] GameObject _finalclearMenu;
    public string nextSceneName;

   public void Next()
    {
        SceneManager.LoadSceneAsync(nextSceneName);
    }

    public void Home()
    {
        SceneManager.LoadScene("Title");
        Time.timeScale = 1;
    }


}
