using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SuspicionMeter : MonoBehaviour
{

    public Image suspicionMeter;

    public Sprite[] sprites;

    public float susDelta = 0.2f;
    public float flickerTimeInterval = 0.2f;
    public int flickerAmount = 5;

    public bool kickedOutFlag;
    
    void Start()
    {
        suspicionMeter.sprite = sprites[0];

        kickedOutFlag = false;
    }
    public void TalkNPC()
    {
        GameManager.Instance.currentSusVal += susDelta;

        StartCoroutine(TalkNPCCoroutine());
    }
    public IEnumerator TalkNPCCoroutine()
    {
        int idx = Mathf.FloorToInt(GameManager.Instance.currentSusVal*sprites.Length);  // 0은 예외로 0, 0.124까지도 0, 0.125이면 1. 0.99까지는 7, 1은 8.

        //미터가 꽉찼다면 일단 게임오버는 확정이고, 다 찼다는 UI는 보여주고 끝내기 위해 idx를 조정한다. 
        if(idx >= sprites.Length)
        {
            idx = sprites.Length-1;
            kickedOutFlag = true;
        }

        Sprite prevSprite = suspicionMeter.sprite;
        Sprite nextSprite = sprites[idx];

        //이미 미터가 다 찬 상태였다면 깜빡거리는 모션도 보여줄 필요가 없기에 바로 게임 오버처리한다. 
        if(prevSprite == sprites[sprites.Length-1] && kickedOutFlag)
        {
            yield return new WaitForSeconds(0.5f);
            GameManager.Instance.KickedOut();
        }
        //그게 아닐 경우 깜빡거리는 모션을 보여준다. 
        else
        {
            for(int i=0; i<flickerAmount; i++)
            {
                suspicionMeter.sprite = nextSprite;
                yield return new WaitForSeconds(flickerTimeInterval);
                suspicionMeter.sprite = prevSprite;
                yield return new WaitForSeconds(flickerTimeInterval);
            }
            suspicionMeter.sprite = nextSprite;
        }

        //미터가 꽉찼다면 (게임 오버할 상태였다면) 게임 오버처리한다. 
        if(kickedOutFlag)
        {
            GameManager.Instance.KickedOut();
        }
        
    }
}
