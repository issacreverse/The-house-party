using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using DG.Tweening;

public class NPC : MonoBehaviour
{
    public bool isTalkable;
    public bool moveEnabled;
    public bool isDoneTalking;
    public bool isMoving;
    public bool navigationPaused;

    [SerializeField] private float duration = 1.2f;

    [SerializeField] public NPC_Data data;
    public int currentPOIId;
    public int currentPOISlot;
    public float givenDwellTime = 1f;
    public float currentTime = 0f;
    public bool isTimeRunning = true;

    private NavMeshAgent agent;
    private Rigidbody rb;   //회전 고정용. 도착하고 왜 서서히 돌아가는지 도저히 모르겠음. 그냥 고정할란다. 

    //태그 정보 런타임 저장 
    public bool isTagged = false;

    //태그 시 색깔 바꿈 (테스트 용)
    MeshRenderer rend;
    Color ogColor;

    //dialog
    public string currentDialog;

    //npc animation
    public bool animOn = true;
    public bool isCustomAsset = true;

    public NPCAnim _npcAnim;
    public float currentActTime = 0f;
    public bool isActing = false;
    public float actTime = 5f; //한 번 act 하면 얼마나 오래 하는지 
    public int talkFrequency = 4;  //얼마나 자주 act에 가는지. "작을수록" 자주 갑니다! (1이상) 예) 4-> 1/4  0-> 항상 

    void Start()
    {
        isTalkable = false;
        moveEnabled = true;
        isDoneTalking = true;
        isMoving = false;
        navigationPaused = false;

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        if(!isCustomAsset)
        {
            rend = GetComponent<MeshRenderer>();
            ogColor = rend.material.color;
        }
        

        Vector3 initDest = POIManager.Instance.InitGetDestination(this);
        if(initDest == Vector3.zero)
        {
            Debug.Log("Not Enough POI slots for total NPCs.");
        }
        agent.SetDestination(initDest);
        isMoving = true;
        isTimeRunning = false;
        Debug.Log("kk");
        Debug.Log("animOn1:" + animOn);
        //목적지 설정되면 walk animation 재생.
        if(animOn)
        {
            _npcAnim.Walk();
            Debug.Log("Init WalkAnim");
        }
            

        //act 시도 시작 (1초마다 한 번)
        if(animOn)
        {
            StartCoroutine(TryAct());
            Debug.Log("Start coroutine");
        }
        Debug.Log("animOn2:" + animOn);

        //agent.avoidancePriority = Random.Range(20, 80);
    }

    void Update()
    {
        //타이머가 활성화되어있다면 시간을 센다. 
        //DwellTime을 계산하는데 쓴다. 
        if(isTimeRunning)
        {
            currentTime += Time.deltaTime;
            //act 중이라면 해당 타이머도 반영한다. 
            if(isActing)
            {
                currentActTime -= Time.deltaTime;
                //act가 끝났다면 idle 상태로 돌아온다. 
                if(currentActTime <= 0f)
                {
                    isActing = false;
                    Debug.Log("AA");
                    Debug.Log("animOn3:" + animOn);
                    if(animOn)
                        _npcAnim.ResetToIdle();
                }
            }
        }


        //움직이는게 허용됨 && dwellTime을 넘겼음 && navigation중이지 않음
        //navigation을 시도한다
        //성공하면 navigation한다. 실패하면 해당 자리에 한번더 머무른다 (타이머 재활성화)
        if(moveEnabled && currentTime >= givenDwellTime && !isMoving)
        {
            POI prevPOI = POIManager.Instance.FindPOIWithId(currentPOIId);
            //Vector3 prevPos = transform.position;
            float prevGivenDwellTime = givenDwellTime;

            Vector3 nextDest = POIManager.Instance.GetNextDestination(this);

            if(nextDest != Vector3.zero)
            {
                isMoving = true;
                isTimeRunning = false;
                rb.constraints &= ~RigidbodyConstraints.FreezeRotationY;
                agent.SetDestination(nextDest);
                prevPOI.FreeSlot(currentPOISlot);

                Debug.Log("BB");
                //목적지 설정되면 walk animation 재생.
                Debug.Log("animOn4:" + animOn);
                if(animOn) 
                    _npcAnim.Walk();
            }
            else
            {
                    givenDwellTime = prevGivenDwellTime;
                    currentTime = 0f;
            }
        }

        //navigation중임 && 도착했다면 
        //isMoving 해제. 시간을 초기화하고 타이머 재활성화. 
        if(isMoving && HasArrived(agent))
        {
            currentTime = 0f;
            isMoving = false;
            isTimeRunning = true;

            transform.DOKill();
            Quaternion targetRotation = POIManager.Instance.FindPOIWithId(currentPOIId).GetSlotRotation(currentPOISlot);
            Tween t = transform.DORotateQuaternion(targetRotation, 0.5f);
            t.OnComplete(() =>
                {
                    rb.constraints |= RigidbodyConstraints.FreezeRotationY;
                });
            
            //도착하면 idle animation 재생. 
            if(animOn)
                _npcAnim.ResetToIdle();
        }

    }
    public IEnumerator TryAct()
    {
        while(true)
        {
            Debug.Log("C");
            yield return new WaitForSeconds(1.0f);
            if(!isMoving)
            {  
                if(currentTime >= 2f && !isActing && Random.Range(0,talkFrequency) == 0)
                {
                    Debug.Log("D");
                    currentActTime += actTime;
                    isActing = true;
                    //act animation을 실행한다. 
                    Debug.Log("animOn5:" + animOn);
                    if(animOn)
                        _npcAnim.Act();
                }
            }
        }
        
    }
    bool HasArrived(NavMeshAgent agent)
    {
        if (agent.pathPending)
            return false;

        if (agent.remainingDistance > agent.stoppingDistance)
            return false;

        if (agent.hasPath && agent.velocity.sqrMagnitude != 0f)
            return false;

        return true;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isTalkable = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isTalkable = false;
        }
    }
    public IEnumerator StartTalking(Transform target)
    {
        moveEnabled = false;
        //navigation 중이였다면 
        //nav중지 
        if(isMoving)
        {
            agent.isStopped = true;
            navigationPaused = true;
        }
        //Look At Player
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f) yield break;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.LookRotation(dir);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        transform.rotation = endRot;

        //Actual Conversation
        //dialog start
        currentDialog = GetWeightedRandomDialog().text;
        UIManager.Instance.UI_dialogEnter(currentDialog);
        AudioManager.Instance.PlayVoiceSource();
        
        yield return new WaitForSeconds(2f);
        isDoneTalking = true;
    }
    public IEnumerator NPCAfterConversation()
    {
        moveEnabled = true;
        //원래대로 회전
        transform.DOKill();
        rb.constraints &= ~RigidbodyConstraints.FreezeRotationY;
        Quaternion targetRotation = POIManager.Instance.FindPOIWithId(currentPOIId).GetSlotRotation(currentPOISlot);
        Tween t = transform.DORotateQuaternion(targetRotation, 0.5f);
        t.OnComplete(() =>
            {
                rb.constraints |= RigidbodyConstraints.FreezeRotationY;
            });

        if(navigationPaused)
        {
            navigationPaused = false;
            agent.isStopped = false;
        }
        //dialog done
        UIManager.Instance.UI_dialogExit();
        yield return null;
    }
    public void TagNPC()
    {
        GameManager.Instance.UseTagsLeft();
        
        isTagged = !isTagged;
        if(isTagged)
        {
            Debug.Log(data.npcName + " has been tagged!");
            rend.material.color = Color.red;
        }
        else
        {
            Debug.Log(data.npcName + " has been untagged!");
            rend.material.color = ogColor;
        }
    }


    public Dialog GetWeightedRandomDialog()
    {
        if (data.dialogs == null || data.dialogs.Length == 0)
            return null;

        // 1️⃣ 전체 weight 합 구하기
        float totalWeight = 0f;

        for (int i = 0; i < data.dialogs.Length; i++)
        {
            if (data.dialogs[i] == null) continue;

            // weight가 0 이하인 경우 방지
            totalWeight += Mathf.Max(0f, data.dialogs[i].weight);
        }

        if (totalWeight <= 0f)
            return data.dialogs[0]; // fallback

        // 2️⃣ 랜덤 값 생성
        float randomPoint = Random.Range(0f, totalWeight);

        // 3️⃣ 누적하면서 선택
        float current = 0f;

        for (int i = 0; i < data.dialogs.Length; i++)
        {
            if (data.dialogs[i] == null) continue;

            current += Mathf.Max(0f, data.dialogs[i].weight);

            if (randomPoint <= current)
                return data.dialogs[i];
        }

        // 안전 fallback (float 오차 대비)
        return data.dialogs[data.dialogs.Length - 1];
    }
}
