using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    public bool isRunningInput = false;
    public Vector2 curMovementInput;
    public float jumpPower;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody rigidbody;
    private PlayerCondition condition;

    [HideInInspector]
    public bool isActuallyRunning = false;

    //private Condition con;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        condition = GetComponent<PlayerCondition>();
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
        if (CharacterManager.Instance.Player.condition.uiCondition.stamina.curValue < 1f) return;
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

    public void OnInventoryStarted(InputAction.CallbackContext context)
    {
        // 인벤토리 실행 함수
    }

    public void OnInteractionStarted(InputAction.CallbackContext context)
    {
       // 상호작용 함수
    }

    public void OnFlashStarted(InputAction.CallbackContext context)
    {
        // 후레시 함수
    }

    public void OnMenu(InputAction.CallbackContext context)
    {

    }

    private void Move()
    {
        //bool staminaAvailable = CharacterManager.Instance.Player.condition.uiCondition.stamina.curValue > 0.1f;


        bool canRun = isRunningInput && IsGrounded() && curMovementInput.magnitude > 0.1f;
            //&& staminaAvailable;
        isActuallyRunning = canRun;
        float speed = canRun ? runSpeed : moveSpeed;

        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir.Normalize();
        dir *= speed;
        Vector3 movement = dir * speed * Time.fixedDeltaTime;

        rigidbody.MovePosition(rigidbody.position + movement);
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
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

    
}
