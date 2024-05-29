using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.UI;
using UnityEngine.UI;

public class StageMenu : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] GameObject stageButtons;

    private void Awake()
    {
        ButtonsToArray();
        int unlockedStage = PlayerPrefs.GetInt("unlockedStage", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0;i<unlockedStage; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void OpenStage(int stageId)
    {
        string stageName = "Stage" + stageId;
        SceneManager.LoadScene(stageName);
    }

    void ButtonsToArray()
    {
        int childcount = stageButtons.transform.childCount;
        buttons = new Button[childcount];

        for (int i = 0;i< childcount;i++)
        {
            buttons[i] = stageButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
        }
    }
}
