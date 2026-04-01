using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_3 : MonoBehaviour
{
    public void OnRetryButtonClicked()
    {
        GameObject[] persistents = GameObject.FindGameObjectsWithTag("Persistent");

        foreach (GameObject obj in persistents)
        {
            Destroy(obj);
        }

        SceneManager.LoadScene("GameScene");
    }

    public void MenuBtn()
    {
        GameObject[] persistents = GameObject.FindGameObjectsWithTag("Persistent");

        foreach (GameObject obj in persistents)
        {
            Destroy(obj);
        }

        SceneManager.LoadScene("MenuScene");
    }
}
