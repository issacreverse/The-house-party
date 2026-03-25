using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{   
    public Image howToPlayPanel;

    public Image settingsPanel;

    public void NewGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Settings()
    {
        settingsPanel.gameObject.SetActive(true);
    }
    public void SettingsPanelOff()
    {
        settingsPanel.gameObject.SetActive(false);
    }

    public void CustomModes()
    {
        //toggle CustomModes UI
        //or go to CustomModes Scene
    }

    public void HowToPlay()
    {
        howToPlayPanel.gameObject.SetActive(true);
    }
    public void HowToPlayPanelOff()
    {
        howToPlayPanel.gameObject.SetActive(false);
    }
    public void Lore()
    {
        SceneManager.LoadScene("LoreScene");
    }
    public void Credits()
    {
        //toggle Credits UI
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
