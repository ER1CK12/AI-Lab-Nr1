using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    public GameObject movementDirectionObject;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float _staminaAmount = 100f;
    [SerializeField] private float maxStamina = 100f;

    private float staminaAmount
    {
        get => _staminaAmount;
        set => _staminaAmount = math.max(0, value);
    }

    [SerializeField] private float staminaDrain = 7f;
    [SerializeField] private float staminaRegeneration = 3f;
    [SerializeField] private float rotationSmoothness = 0.1f;

    private CharacterController controller;
    private InputAction moveAction;
    private InputAction sprintAction;
    private Vector3 velocity;
    private float gravity = -9.8f;
    private bool isSprinting = false;
    private Vector3 lastMoveDirection = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        var inputMap = InputSystem.actions;
        moveAction = inputMap.FindAction("Move");
        sprintAction = inputMap.FindAction("Sprint");
    }

    void Update()
    {
        HandleMovement();
        HandlePlayerRotation();
        HandleStamina();
        ApplyGravity();
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMovement()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        bool sprintInput = sprintAction.IsPressed();

        isSprinting = sprintInput && staminaAmount > 0;
        float currentSpeed = isSprinting ? sprintSpeed : speed;

        Vector3 moveDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;

        velocity.x = moveDirection.x * currentSpeed;
        velocity.z = moveDirection.z * currentSpeed;

        if (moveDirection != Vector3.zero)
        {
            lastMoveDirection = moveDirection;
        }
    }

    void HandlePlayerRotation()
    {

        Vector3 directionToTarget = movementDirectionObject.transform.position - transform.position;

        directionToTarget.y = 0;
        directionToTarget.Normalize();

        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                rotationSmoothness
            );
        }
    }

    void HandleStamina()
    {
        if (isSprinting)
        {
            staminaAmount -= staminaDrain * Time.deltaTime;
        }
        else
        {
            if (staminaAmount < maxStamina)
            {
                staminaAmount += staminaRegeneration * Time.deltaTime;
            }
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    public float GetStaminaPercent()
    {
        return staminaAmount / maxStamina;
    }

    public bool IsSprinting()
    {
        return isSprinting;
    }
}