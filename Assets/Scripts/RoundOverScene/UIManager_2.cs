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
    
}
