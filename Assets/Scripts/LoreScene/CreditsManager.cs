using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    //public Sprite[] loreSprites;
    //public Image currentLoreImg;
    public Text currentCreditText;
    public string[] creditTexts;

    public int idx = 0;

    [SerializeField] private AudioSource sfxManager;
    [SerializeField] private AudioClip pageTurnSfx;

    void Start()
    {
        //currentLoreImg.sprite = loreSprites[0];
        creditTexts = new string[]
        {
            "<color=green>DIRECTOR</color>\n<size=42>Hunter Cave</size>\n\n<color=green>PRODUCER/CO-DIRECTOR</color>\n<size=42>Liam Patton</size>",
            "<color=green>PROGRAMMING</color>\n<size=42>Jisang Ryu</size>",
            "<color=green>ART & MODELING</color>\n<size=42>Hunter Cave\nSam Abreu\nLeo Santiago</size>\n\n<color=green>DESIGN</color>\n<size=42>HunterCave\nLiam Patton\nJoseph Gonzalez</size>",
            "<color=green>UI/UX</color>\n<size=42>Brighton Brown\nKelsey Mwenebatu\nHunter Cave</size>\n\n<color=green>QA & PLAYTESTING</color>\n<size=42>Joseph Gonzalez\nBrighton Brown\nKelsey Mwenebatu</size>",
            "<color=green>MUSIC & SFX</color>\n<size=42>Malachi Boucher\nEddie Shannon</size>\n\n<color=green>DIALOGUE</color>\n<size=42>Liam Patton</size>",
            "<color=green>ORIGINAL CONCEPT</color>\n<size=42>Cayley Settle</size>",
            "<color=green>SPECIAL THANKS</color>\n<size=42>Arial Bergamini\nCarly Flores\nNick McClarnon\nCayley Settle\nMin-Chia Tseng</size>",
            "<color=green>SPECIAL THANKS</color>\n<size=42>WaZ Williams\nBailey Wirdzek\nKendale Young\nSteven Mandiberg\nCoryHaltinner</size>"
        };
        currentCreditText.text = creditTexts[0];
        idx = 0;

    }
    public void Next()
    {   
        //if(idx+1 >= loreSprites.Length)
        //    return;
        //currentLoreImg.sprite = loreSprites[idx+1];

        if(idx+1 >= creditTexts.Length)
            return;
        currentCreditText.text = creditTexts[idx+1];
        idx++;
        sfxManager.PlayOneShot(pageTurnSfx);
    }
    public void Prev()
    {
        if(idx <= 0)
            return;
        //currentLoreImg.sprite = loreSprites[idx-1];
        currentCreditText.text = creditTexts[idx-1];
        idx--;
        sfxManager.PlayOneShot(pageTurnSfx);
    }
    public void BackMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}

