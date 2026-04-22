using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraPivot;

    
    [Tooltip("Attach main camera. (For FOV)")]
    [SerializeField] private Camera playerCamera;

    [Header("Look Mouse")]
    [SerializeField] private float sensitivityX = 2.0f; // yaw
    [SerializeField] private float sensitivityY = 2.0f; // pitch
    [SerializeField] private float minPitch = -85f;
    [SerializeField] private float maxPitch = 85f;
    
    [Header("Move (WASD)")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 1.8f;
    [SerializeField] private float terminalVelocity = -20f;

    private bool jumpRequested;

    
    [Header("FOV (Optional)")]
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float zoomFOV = 35f;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float talkFOV = 40f;
    [SerializeField] private float talkLerpSpeed = 10f;

    

    private CharacterController cc;

    private float pitch;
    private float verticalVelocity;

    private bool lookEnabled = true;
    private bool moveEnabled = true;
    private bool zoomEnabled = true;
    private bool tagEnabled = true;


    // private float targetFOV;

    //aim and tag
    [SerializeField] private float tagRange = 500f;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private SuspicionMeter _suspicionMeter;



    void Awake()
    {
        cc = GetComponent<CharacterController>();

        if(cameraPivot == null)
            Debug.Log("Error: camera pivot missing");
        /*
        if(playerCamera = null)
            playerCamera = Camera.main;
        
        targetFOV = normalFOV;
        if(playerCamera != null)
            playerCamera.fieldOfView = normalFov;

        LockCursor(true);
        */

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void EnablePlayerControl()
    {
        lookEnabled = true;
        moveEnabled = true;
        zoomEnabled = true;
    }
    public void DisablePlayerControl()
    {
        lookEnabled = false;
        moveEnabled = false;
        zoomEnabled = false;
    }
    void Update()
    {
        // UpdateFOV();

        if(lookEnabled)
            HandleLook();
        if(moveEnabled)
            HandleMove();
        if(zoomEnabled)
            HandleZoom();
        else
            ApplyGravityOnly();
        /*
        if(Input.GetKeyDown(KeyCode.Space))
            jumpRequested = true;
        */
    }

    private void HandleLook()
    {
        float mx = Input.GetAxis("Mouse X") * sensitivityX;
        float my = Input.GetAxis("Mouse Y") * sensitivityY;

        transform.Rotate(0f, mx, 0f);

        pitch += my * -1f;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        if (cameraPivot != null)
            cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
    private void HandleMove()
    {
        float h = Input.GetAxisRaw("Horizontal"); 
        float v = Input.GetAxisRaw("Vertical");   

        Vector3 input = new Vector3(h, 0f, v);
        input = Vector3.ClampMagnitude(input, 1f);

        Vector3 move = transform.right * input.x + transform.forward * input.z;

        if(cc.isGrounded)
        {
            if(verticalVelocity < 0f)
                verticalVelocity = -2f;

            if(jumpRequested)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        

        verticalVelocity += gravity * Time.deltaTime;
        verticalVelocity = Mathf.Max(verticalVelocity, terminalVelocity);

        Vector3 velocity = move * moveSpeed;
        velocity.y = verticalVelocity;

        cc.Move(velocity * Time.deltaTime);

        jumpRequested = false;
    }
    private void HandleZoom()
    {
        if(Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftShift))
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, Time.deltaTime * zoomSpeed);
            UIManager.Instance.UI_ZoomScopeEnter();

            //aim and tag
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out RaycastHit hit, tagRange, hitMask, QueryTriggerInteraction.Ignore))
            {
                // hit.collider / hit.transform 으로 “조준 중인 대상” 인식
                if(hit.collider.gameObject.CompareTag("NPC"))
                {
                    if(Input.GetMouseButtonDown(0) && tagEnabled)
                    {
                        hit.collider.GetComponent<NPC>().TagNPC();
                    }
                
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                // 예: 타겟 하이라이트, 이름 표시, lock-on 등
                }
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * tagRange, Color.red);
            }
        }
        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, Time.deltaTime * zoomSpeed);
            UIManager.Instance.UI_ZoomScopeExit();
        }
        
    }
    private void ApplyGravityOnly()
    {
        if (cc.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;
        cc.Move(new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime);
    }
    public void BeginConversation(NPC currentTarget)
    {
        //Exit Zoom
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, Time.deltaTime * zoomSpeed);
        UIManager.Instance.UI_ZoomScopeExit();

        moveEnabled = false;
        lookEnabled = false;
        zoomEnabled = false;

        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, talkFOV, Time.deltaTime * talkLerpSpeed);
        _suspicionMeter.TalkNPC(); // meter rises when talking to NPC.
        StartCoroutine(currentTarget.StartTalking(gameObject.transform));

    }
    public void EndConversation(NPC currentTarget)
    {
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, Time.deltaTime * talkLerpSpeed);
        StartCoroutine(currentTarget.NPCAfterConversation());

        currentTarget.moveEnabled = true;
        moveEnabled = true;
        lookEnabled = true;
        zoomEnabled = true;
        

    }
    public void DisableZoom()
    {
        zoomEnabled = false;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, Time.deltaTime * zoomSpeed);
        UIManager.Instance.UI_ZoomScopeExit();
        tagEnabled = false;
    }
    public void DisableAll()
    {
        DisableZoom();
        lookEnabled = false;
        moveEnabled = false;
    }

}
