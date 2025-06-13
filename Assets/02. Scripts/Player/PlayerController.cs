using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    private bool isRunningInput = false;
    private Vector2 curMovementInput;
    public float jumpPower;
    public LayerMask groundLayerMask;

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
    

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
    }

    public void OnMoveInputCanceled(InputAction.CallbackContext context)
    {
        curMovementInput = Vector2.zero;
    }

    public void OnRunInputPerformed(InputAction.CallbackContext context)
    {
        isRunningInput = true;
    }

    public void OnRunInputCanceled(InputAction.CallbackContext context)
    {
        isRunningInput = false;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        // 인벤토리 실행 함수
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
       // 상호작용 함수
    }

    private void Move()
    {
        bool canRun = isRunningInput && IsGrounded() && curMovementInput.magnitude > 0.1f;
        float speed = canRun ? runSpeed : moveSpeed;

        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= speed;
        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    bool IsGrounded()
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
        if(isDead)
        {
            return;
        }

        //  조작 정지
        canLook = false;
        curMovementInput = Vector2.zero;
        rigidbody.velocity = Vector3.zero;
        ToggleCursor(true);

        //  사망 연출 호출
        if(deathEffectManager != null)
        {
            deathEffectManager.PlayDeathSequence();
        }
    }
}
