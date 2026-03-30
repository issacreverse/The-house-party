using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BriefMenu : MonoBehaviour
{
    public void OpenBrief(GameObject SelectedBrief)
    {
        //.SetActive(true);

        if (SelectedBrief != null)
        {
            SelectedBrief.SetActive(true);
        }
    }
    public void CloseBrief(GameObject Brief)
    {

        if (Brief != null)
        {
            Brief.SetActive(false);
        }
    }

    public void OpenLevel(int levelId)
    {
        string levelName = "Level " + levelId;
        SceneManager.LoadScene(levelName);
    }
}
