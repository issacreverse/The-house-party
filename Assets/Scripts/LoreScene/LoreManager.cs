using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoreManager : MonoBehaviour
{
    public Sprite[] loreSprites;
    public Image currentLoreImg;

    public int idx = 0;

    void Start()
    {
        currentLoreImg.sprite = loreSprites[0];
        idx = 0;

    }
    public void Next()
    {   
        if(idx+1 >= loreSprites.Length)
            return;
        currentLoreImg.sprite = loreSprites[idx+1];
        idx++;
    }
    public void Prev()
    {
        if(idx <= 0)
            return;
        currentLoreImg.sprite = loreSprites[idx-1];
        idx--;
    }
    public void BackMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}

