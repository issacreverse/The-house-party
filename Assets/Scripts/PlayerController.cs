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

    [SerializeField] private GameObject UIManager;
    private UIManager _UIManager;

    // private float targetFOV;


    void Awake()
    {
        cc = GetComponent<CharacterController>();

        _UIManager = UIManager.GetComponent<UIManager>();

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

        if(Input.GetKeyDown(KeyCode.Space))
            jumpRequested = true;
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
        if(Input.GetMouseButton(1))
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, Time.deltaTime * zoomSpeed);
            _UIManager.UI_ZoomScopeEnter();
        }
        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, Time.deltaTime * zoomSpeed);
            _UIManager.UI_ZoomScopeExit();
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
        _UIManager.UI_ZoomScopeExit();

        moveEnabled = false;
        lookEnabled = false;
        zoomEnabled = false;

        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, talkFOV, Time.deltaTime * talkLerpSpeed);
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
#region FOV
    /*
    private void UpdateFov()
    {
        if (playerCamera == null) return;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFov, Time.deltaTime * fovLerpSpeed);
    }


    // =========================
    // Public API (대화/연출용)
    // =========================

    /// <summary>대화 시작 등으로 시점 전환을 막고 싶을 때</summary>
    public void SetLookEnabled(bool enabled) => lookEnabled = enabled;

    /// <summary>대화 시작 등으로 이동을 막고 싶을 때</summary>
    public void SetMoveEnabled(bool enabled) => moveEnabled = enabled;

    /// <summary>커서 잠금/해제</summary>
    public void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    /// <summary>FOV 목표값 설정 (줌 연출)</summary>
    public void SetTargetFov(float fov)
    {
        targetFov = Mathf.Clamp(fov, 30f, 120f);
    }

    /// <summary>기본 FOV로 복귀</summary>
    public void ResetFov() => targetFov = normalFov;

    /// <summary>
    /// 대화 시작용 편의 함수: 이동/시점 잠그고, 커서 풀고, 줌
    /// </summary>
    public void EnterDialogueMode(float dialogueFov = 55f, bool unlockCursor = true)
    {
        SetMoveEnabled(false);
        SetLookEnabled(false);
        SetTargetFov(dialogueFov);
        if (unlockCursor) LockCursor(false);
    }

    /// <summary>
    /// 대화 종료용 편의 함수: 원복
    /// </summary>
    public void ExitDialogueMode(bool lockCursor = true)
    {
        ResetFov();
        SetMoveEnabled(true);
        SetLookEnabled(true);
        if (lockCursor) LockCursor(true);
    }
    */
#endregion

}
