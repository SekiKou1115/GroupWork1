using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearMenu : MonoBehaviour
{
    [SerializeField] GameObject _clearMenu;
    public string nextSceneName;

   public void Next()
    {
        SceneManager.LoadSceneAsync(nextSceneName);
    }


}
