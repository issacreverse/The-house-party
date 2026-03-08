using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

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

    //시작 필드
    public int monsterCount;
    public int tagsLeft = 1;

    public int monsterTagged;   //잘 잡은 거
    public int monsterNotTagged; // 놓친 거
    public int humanTagged; //생사람 잡은 거
    public int humanNotTagged; //일반 사람 

    private bool isRoundOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public event Action OnTagsLeftChanged;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        currentTime = 0f;
        npcList = new List<NPC>();
        npcTagFalseList = new List<NPC>();
        npcTagTrueList = new List<NPC>();

        RegisterNPC();
        InitialUIUpdate();

        // for prototype
        SceneManager.sceneLoaded += ShowResults;
    }
    // for prototype
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= ShowResults;
    }

    // Update is called once per frame
    void Update()
    {

        if(isRoundOver)
            return;
            
        if(!gamePaused)
        {
            currentTime += Time.deltaTime;
        }

        if(currentTime >= roundTime)
        {
            //RoundOver();
        }
        if(tagsLeft <= 0)
        {
            RoundOver();
        }
    }
    void RoundOver()
    {
        isRoundOver = true;

        //라운드 끝나고 이겼는지 졌는지 계산 
        SortTaggedNPC();

        monsterTagged = 0;
        monsterNotTagged = 0;
        humanTagged = 0;
        humanNotTagged = 0;

        foreach(NPC npc in npcTagTrueList)
        {
            if(npc.data.npcType == NPCType.monster)
                monsterTagged++;
            else if(npc.data.npcType == NPCType.human)
                humanTagged++;     
        }
        foreach(NPC npc in npcTagFalseList)
        {
            if(npc.data.npcType == NPCType.monster)
            {
                monsterNotTagged++;
            }
            else if(npc.data.npcType == NPCType.human)
            {
                humanNotTagged++;
            }
        }

        SceneManager.LoadScene("RoundOverScene");

        

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
        monsterCount = 0;
        for(int i=0; i < NPCs.childCount; i++)
        {
            NPC npc = NPCs.GetChild(i).gameObject.GetComponent<NPC>();
            if(npc.data.npcType == NPCType.monster)
                monsterCount++;
            npcList.Add(npc);
        }
        tagsLeft = monsterCount;
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
    public void UseTagsLeft()
    {
        tagsLeft--;
        OnTagsLeftChanged?.Invoke();
    }
    public void InitialUIUpdate()
    {
        OnTagsLeftChanged?.Invoke();
    }

    // for prototype
    public void ShowResults(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "RoundOverScene")
            return;

        StartCoroutine(ShowResultWithDelay());
    }
    // for prototype
    public IEnumerator ShowResultWithDelay()
    {
        GameObject UIManager = GameObject.Find("UIManager");
        UIManager_2 _UI = UIManager.GetComponent<UIManager_2>();
        
        if(monsterTagged != 0)
        {
            yield return new WaitForSeconds(2.5f);
            _UI.Showtext("You executed " + monsterTagged + " monsters.");
        }
           
        if(humanTagged != 0)
        {
            yield return new WaitForSeconds(2.5f);
            _UI.Showtext("You killed " + humanTagged + " inocent people.");
        }
            
        if(monsterNotTagged != 0)
        {
            yield return new WaitForSeconds(2.5f);
            _UI.Showtext("You let " + monsterNotTagged + " monsters go away...");
        }
            
        
        if(monsterTagged == monsterCount)
        {
            yield return new WaitForSeconds(3.5f);
            _UI.Showtext("<color=green>Looks like you saved another halloween. Well Done.</color>");
        }
            
        else
        {
            yield return new WaitForSeconds(3.5f);
            _UI.Showtext("<color=red>You hear SCREAMING VOICES...Game Over.</color>");
        }
    }
}
