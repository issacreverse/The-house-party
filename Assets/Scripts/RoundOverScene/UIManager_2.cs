using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager_2 : MonoBehaviour
{
    [SerializeField] private Text suspicionLevel;
    [SerializeField] private Text monstersEliminated;
    [SerializeField] private Text humansHarmed;

    [SerializeField] private Image missionResult;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failedSprite;


    // UnlockNewLevel function authored by Hunter Cave
    // To be active when Levels are built
    /*
    void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
     */

    //UIManager_2의 텍스트를 변경합니다. 필드 차례대로 1부터 받습니다.
    public void PrintText(int textNo, string text)
    {
        switch (textNo)
        {
            case 1: 
                suspicionLevel.text = text;
                break;
            case 2:
                monstersEliminated.text = text;
                break;
            case 3:
                humansHarmed.text = text;
                break;
            default:
                break;
        }
    }

    public void ShowResultImage(bool success)
    {
        missionResult.gameObject.SetActive(true);
        if(success)
        {
            missionResult.sprite = successSprite;
            // To be active when UnlockNewLevel function is active
            // UnlockNewLevel();
        }
        else
        {
            missionResult.sprite = failedSprite;
        }
    }

    public void RetryBtn()
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
