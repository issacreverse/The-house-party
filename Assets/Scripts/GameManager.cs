using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField] private Transform NPCs;
    private List<NPC> npcList;
    private List<NPC> npcTagTrueList;
    private List<NPC> npcTagFalseList;

    [SerializeField] private float roundTime;
    private float currentTime;

    private bool gamePaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        currentTime = 0f;
        npcList = new List<NPC>();
        npcTagFalseList = new List<NPC>();
        npcTagTrueList = new List<NPC>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gamePaused)
        {
            currentTime += Time.deltaTime;
        }

        if(currentTime >= roundTime)
        {
            RoundOver();
        }
    }
    void RoundOver()
    {
        //라운드 끝나고 이겼는지 졌는지 계산 
        SortTaggedNPC();
        //처형씬으로 전환, 태그된 npc들 앞에 세워놓음
        //monster - tagged : success
        //human - tagged : inocent kill
        //monster - not tagged : monster escape
        //human - not tagged : nothing
        //
        //애니메이션 재생
        //점수 계산해서 UI 보여주고 게임 종료 -> 메인 화면으로?
    }

    void RegisterNPC()
    {
        for(int i=0; i < NPCs.childCount; i++)
        {
            npcList.Add(NPCs.GetChild(i).gameObject.GetComponent<NPC>());
        }
    }
    void SortTaggedNPC()
    {
        foreach(NPC npc in npcList)
        {
            if(npc.isTagged)
            {
                npcTagTrueList.Add(npc);
            }
            else
            {
                npcTagFalseList.Add(npc);
            }
        }
    }
}
