using UnityEngine;
using UnityEngine.UI;

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

    public bool isTalkable = false;
    public bool isTalking = false;

    public NPC currentTarget { get; private set; }

    //public Text text;

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

        //text.text = "isTalkable: " + isTalkable;
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

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, npcMask))
        {
            NPC npc = hit.collider.GetComponentInParent<NPC>();

            if (npc != null && npc.isNPCTalkable)
            {
                currentTarget = npc;
                isTalkable = true;
                return;
            }
        }

        currentTarget = null;
        isTalkable = false;
        /*
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
        Debug.Log($"[Talk] hitCollider={hit.collider.name}, hitObject={hit.collider.gameObject.name}, npc={(npc ? npc.name : "null")}");
        currentTarget = npc; // npc가 null이면(레이어 잘못 지정 등) null로 들어감

        if(currentTarget == null)
        {
            Debug.Log("[Talk] currentTarget null");
            isTalkable = false;
            return;
        }
        
        Debug.Log($"[Talk] target={currentTarget.name}, npcTalkable={currentTarget.isNPCTalkable}");
        isTalkable = currentTarget.isNPCTalkable;
        */
    }
}
