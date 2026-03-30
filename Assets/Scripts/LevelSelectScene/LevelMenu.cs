using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Script Author: Hunter Cave
public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void OpenBrief(GameObject SelectedBrief)
    {
        //.SetActive(true);

        if (SelectedBrief != null)
        {
            SelectedBrief.SetActive(true);
        }
    }
}
