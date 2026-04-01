using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField] private GameObject player;

    [SerializeField] private Transform NPCs;
    private List<NPC> npcList;
    private List<NPC> npcTagTrueList;
    private List<NPC> npcTagFalseList;

    [SerializeField] private float roundTime;
    private float currentTime;

    private bool gamePaused = false;

    public float currentSusVal;

    //시작 필드
    public int monsterCount;
    public int tagsLeft = 1;

    public int monsterTagged;   //잘 잡은 거
    public int monsterNotTagged; // 놓친 거
    public int humanTagged; //생사람 잡은 거
    public int humanNotTagged; //일반 사람 

    [SerializeField] private Door _door;
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
    public void GameReset()
    {
        currentTime = 0f;
        NPCs = GameObject.Find("NPCs").transform;
        npcList = new List<NPC>();
        npcTagFalseList = new List<NPC>();
        npcTagTrueList = new List<NPC>();

        RegisterNPC();
        InitialUIUpdate();

        currentSusVal = 0f;
    }
    void Start()
    {
        GameReset();
        StartCoroutine(GameStartDelay());
        // for prototype
        SceneManager.sceneLoaded += ShowResults;
    }

    public IEnumerator GameStartDelay()
    {
        yield return new WaitForSeconds(10.0f);
        //게임 시작
        UIManager.Instance.UI_HideLoadingScreen();
        UIManager.Instance.UI_ShowUserCanvas();
        player.SetActive(true);
        AudioManager.Instance.PlayGameStart();

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
        if(_door.isPlayerInRange)
        {
            float y = transform.eulerAngles.y;
            if(y >180f)
            {
                y = y - 360f;
            }
            if(y > -35f && y < 35f)
            {
                if(Input.GetKey(KeyCode.E))
                {
                    AudioManager.Instance.DoorSlam();
                    RoundOver();
                }
            }
        }
    }
    //suspicion meter is full, you get kicked out even the round isn't over. sad.
    public void KickedOut()
    {
        SceneManager.LoadScene("KickedOutScene");
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
        
        /*
        //이거 안 좋은 코드인 거 아는데 씬 이벤트가 불리니까 같이 얻어탄다는 마인드 
        //초기값 세팅 
        if(scene.name == "GameScene")
        {
            Debug.Log("AA");
            GameReset();
        }
        */
        if(scene.name != "RoundOverScene")
            return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(ShowResultWithDelay());
    }
    // for prototype
    public IEnumerator ShowResultWithDelay()
    {
        GameObject UIManager = GameObject.Find("UIManager");
        UIManager_2 _UI = UIManager.GetComponent<UIManager_2>();

        AudioManager.Instance.StopAudioAfterGame();
        
        yield return new WaitForSeconds(1.0f);

        yield return new WaitForSeconds(1.5f);
        string str1 = Mathf.FloorToInt(currentSusVal*100) + "%";
        _UI.PrintText(1, str1);
        AudioManager.Instance.PlayTextPop();

        yield return new WaitForSeconds(1.5f);
        string str2 = monsterTagged + " / " + monsterCount;
        _UI.PrintText(2, str2);
        AudioManager.Instance.PlayTextPop();

        yield return new WaitForSeconds(1.5f);
        string str3 = humanTagged + "";
        _UI.PrintText(3, str3);
        AudioManager.Instance.PlayTextPop();
        
        
        yield return new WaitForSeconds(2.5f);
        _UI.ShowResultImage(monsterTagged == monsterCount);
        AudioManager.Instance.PlayStamp();
        
    }
}
