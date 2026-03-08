using UnityEngine;
using UnityEngine.UI;

public class UIManager_2 : MonoBehaviour
{
    [SerializeField] private Text resultText;

    public void Showtext(string text)
    {
        if(resultText.text != "")
            resultText.text += ("\n\n" + text);
        else
            resultText.text = text;
    }
}
