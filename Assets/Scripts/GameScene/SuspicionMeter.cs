using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class SuspicionMeter : MonoBehaviour
{

    public Image suspicionMeter;

    public float TalkNPCFillAmount = 0.2f;
    
    void Start()
    {
        suspicionMeter.fillAmount = 0f;
    }
    public void TalkNPC()
    {
        float currentFill = suspicionMeter.fillAmount;

        StartCoroutine(TalkNPCCoroutine());
    }
    public IEnumerator TalkNPCCoroutine()
    {
        float currentFill = suspicionMeter.fillAmount;
        Tween t = suspicionMeter.DOFillAmount(currentFill + TalkNPCFillAmount, 0.5f);

        yield return t.WaitForCompletion();
        
        if(suspicionMeter.fillAmount >= 1.0f)
        {
            GameManager.Instance.KickedOut();
        }
    }
}
