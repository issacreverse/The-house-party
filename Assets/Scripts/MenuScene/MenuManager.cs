using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Settings()
    {
        //toggle settings UI
    }

    public void CustomModes()
    {
        //toggle CustomModes UI
        //or go to CustomModes Scene
    }

    public void Lore()
    {
        //toggle Lore UI
        //or go to Lore Scene
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
