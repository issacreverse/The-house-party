using UnityEngine;

public class Talk : MonoBehaviour
{
    private PlayerController _playerController;

    [Header("Aim Source")]
    [SerializeField] private Camera cam;

    [Header("Cast Settings")]
    [SerializeField] private float maxDistance = 3.0f;
    [SerializeField] private float radius = 0.05f; // 좁게: 0.03~0.08 추천
    [SerializeField] private LayerMask npcMask;     // NPC 레이어만 체크
    [SerializeField] private QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;

    [Header("Debug")]
    [SerializeField] private bool drawDebug = true;

    private bool isTalkable = false;
    private bool isTalking = false;

    public NPC currentTarget { get; private set; }

    void Reset()
    {
        cam = Camera.main;
        npcMask = LayerMask.GetMask("NPC"); 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTalking)
        {
            UpdateTarget();
        }


        if(Input.GetKeyDown(KeyCode.E) && isTalkable && !isTalking)
        {
            //Debug.Log("A");
            _playerController.BeginConversation(currentTarget);
            //Debug.Log("A_");
            isTalking = true;
            currentTarget.isDoneTalking = false;
        }
        if(Input.GetKeyDown(KeyCode.E) && isTalking && currentTarget.isDoneTalking)
        {
            //Debug.Log("B");
            _playerController.EndConversation(currentTarget);
            //Debug.Log("B_");
            isTalking = false;
        }
    }
    void UpdateTarget()
    {
        if (!cam) return;

        Vector3 origin = cam.transform.position;
        Vector3 dir = cam.transform.forward;

        if (drawDebug)
        {
            Debug.DrawRay(origin, dir * maxDistance, Color.yellow);
        }

        // "한 번" 캐스트해서 가장 앞에 걸린 것 1개만 받음
        bool hitAny = Physics.SphereCast(
            origin,
            radius,
            dir,
            out RaycastHit hit,
            maxDistance,
            npcMask,
            triggerInteraction
        );

        if (!hitAny)
        {
            currentTarget = null;
            isTalkable = false;
            return;
        }

        // 콜라이더가 NPC의 자식일 수 있으니 부모에서 찾기
        var npc = hit.collider.GetComponentInParent<NPC>();
        currentTarget = npc; // npc가 null이면(레이어 잘못 지정 등) null로 들어감

        if(currentTarget.isTalkable)
        {
            this.isTalkable = true;
        }
        else
        {
            this.isTalkable = false;
        }
    }
}
