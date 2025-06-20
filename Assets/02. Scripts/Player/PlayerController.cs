using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    public float exhaustionSpeed = 1f;
    public bool isRunningInput = false;
    public Vector2 curMovementInput;
    public float jumpPower;
    public LayerMask groundLayerMask;
    private float addMoveSpeed;
    private Coroutine applyItemEffect;
    public bool isMoving = false;
    private float equipMoveSpeed;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    [Header("Death UI")]
    [SerializeField] private DeathEffectManager deathEffectManager;           //  사망 연출 관리자 연결

    [HideInInspector]
    public bool canLook = true;
    public bool isDead = false;                                               //  죽었는지 확인하기 위한 bool값 변수


    private Rigidbody rigidbody;

    private PlayerCondition condition;

    [HideInInspector]
    public bool isActuallyRunning = false;

    [Header("Interaction Data")]
    public LayerMask interactionMask;
    private Interaction interaction;
    [SerializeField] private float maxInteractionDistance = 5;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        condition = GetComponent<PlayerCondition>();

        interaction = new Interaction(maxInteractionDistance, interactionMask);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InvokeRepeating(nameof(GetInteraction), 0, 0.2f);
        camCurXRot = 0;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }

        if (!isDead)
        {
            SyncPlayerRotationWihtCamera();
        }
    }

    public void OnLookInputPerformed(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    public void OnLookInputCanceled(InputAction.CallbackContext context)
    {
        mouseDelta = Vector2.zero;
    }

    public void OnMoveInputPerformed(InputAction.CallbackContext context)
    {
        curMovementInput = context.ReadValue<Vector2>();
        isMoving = true;
    }

    public void OnMoveInputCanceled(InputAction.CallbackContext context)
    {
        curMovementInput = Vector2.zero;
        isMoving = false;
    }

    public void OnRunInputPerformed(InputAction.CallbackContext context)
    {
        if (CharacterManager.Instance.Player.condition.uiCondition.stamina.curValue < 1f) return;
        isRunningInput = true;
    }

    public void OnRunInputCanceled(InputAction.CallbackContext context)
    {
        isRunningInput = false;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (!IsGrounded()) return;

        if (condition.UseStamina(5))
        {
            condition.NotifyStaminaUsed(); //  회복 딜레이 초기화
            rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnInventoryStarted(InputAction.CallbackContext context)
    {

    }

    public void OnInteractionStarted(InputAction.CallbackContext context)
    {
        interaction.OnInteraction();
    }

    public void GetInteraction()
    {
        interaction.GetInteraction();
    }

    public void OnFlashStarted(InputAction.CallbackContext context)
    {

    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        // ESC 눌렀을때
    }

    public void Move()
    {
        bool canRun = isRunningInput && IsGrounded() && curMovementInput.magnitude > 0.1f;
        isActuallyRunning = canRun;

        float speed;

        if (condition.isExhausted)
        {
            speed = exhaustionSpeed;  // 탈진 이동속도
        }
        else
        {
            speed = canRun ? GetRunTotalSpeed() : GetMoveTotalSpeed();
        }

        //  카메라 방향 기준 이동
        Transform cam = Camera.main.transform;
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.Normalize();
        right.Normalize();

        Vector3 dir = forward * curMovementInput.y + right * curMovementInput.x;
        dir.Normalize();
        dir *= speed;
        Vector3 movement = dir * speed * Time.fixedDeltaTime;

        rigidbody.MovePosition(rigidbody.position + movement);
    }

    private float GetMoveTotalSpeed()
    {
        return moveSpeed + addMoveSpeed/2 + equipMoveSpeed/2;
    }
    private float GetRunTotalSpeed()
    {
        return runSpeed + addMoveSpeed + equipMoveSpeed;
    }

    void CameraLook()
    {
        // 좌우(Y) 회전은 transform(Y축)에만 직접 적용
        //transform.Rotate(Vector3.up * mouseDelta.x * lookSensitivity * Time.deltaTime);

        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);

        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    private void SyncPlayerRotationWihtCamera()
    {
        Transform cameraTransform = Camera.main.transform;

        Vector3 lookDirection = cameraTransform.forward;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    public bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)

        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.65f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    //  즉사 함수
    public void Die()
    {
        if (isDead)
        {
            return;
        }

        //  조작 정지
        canLook = false;
        curMovementInput = Vector2.zero;
        rigidbody.velocity = Vector3.zero;
        //rigidbody.MovePosition(Vector3.zero);
        ToggleCursor(true);

        //  사망 연출 호출
        if (deathEffectManager != null)
        {
            deathEffectManager.PlayDeathSequence();
        }
    }

    public void GetAddItemValue(float amount, float duration)
    {
        if (applyItemEffect != null)
        {
            StopCoroutine(applyItemEffect);
        }

        applyItemEffect = StartCoroutine(ApplyItemValue(amount, duration));
    }

    private IEnumerator ApplyItemValue(float amount, float duration)
    {
        addMoveSpeed = amount;

        yield return new WaitForSeconds(duration);

        addMoveSpeed = 0;
    }

    public void ApplyEquipItem(EquipItemData equipItem)
    {
        equipMoveSpeed += equipItem.moveSpeed;
    }

    public void RemoveEquipItem(EquipItemData equipItem)
    {
        equipMoveSpeed -= equipItem.moveSpeed;
    }
}
