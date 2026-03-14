using UnityEngine;

public class NPCAnim : MonoBehaviour
{
    public Animator animator;

    public int actNum = 0;
    public int walkNum = 0;
    public int idleNum = 1;

    public bool act = false;
    public bool walk = false;
    // public bool idle = true;  idle이 디폴트 state라 필요가 없음 

    //  ****애니메이션 종류 수가 달라지면 값을 바꿔주세요!****
    public int actAnimCount = 7;
    public int walkAnimCount = 7;  // 1: drunk 2-7: sober
    public int idleAnimCount = 4;

    private NPC _npc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _npc = GetComponent<NPC>();
    }

    public void ResetToIdle()
    {
        _npc.isActing = false;
        
        act = false;
        walk = false;
        //Debug.Log("Walk: false");

        actNum = 0;
        walkNum = 0;
        idleNum = Random.Range(1,idleAnimCount+1);

        ApplyTransitionValues();
    }
    void ApplyTransitionValues()
    {
        animator.SetBool("Act", act);
        animator.SetBool("Walk", walk);
        //Debug.Log("Walk APPlied to " + walk);

        animator.SetInteger("ActNum", actNum);
        animator.SetInteger("WalkNum", walkNum);
        animator.SetInteger("IdleNum", idleNum);
    }
    public void Walk()
    {
        ResetToIdle();

        walk = true;
        //Debug.Log("Walk: true");
        
        idleNum = 0;
        walkNum = Random.Range(1,walkAnimCount+1);

        ApplyTransitionValues();
    }
    public void Act()
    {
        ResetToIdle();

        act = true;

        idleNum = 0;
        actNum = Random.Range(1,actAnimCount+1);

        ApplyTransitionValues();
    }
}
