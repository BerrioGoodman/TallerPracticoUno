using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private InputReader input;
    [Header("Movement Scriptable Objects")]
    [SerializeField] private LandedMovement landed;
    [SerializeField] private GlideMovement glide;
    [SerializeField] private UnderwaterMovement underwater;
    [SerializeField] private MovementType movementType;
    [Header("Settings")]
    [SerializeField] private float baseSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float gravity;//Must be soft
    [Header("Multiplier")]
    [SerializeField] private float multiplierSpeed;
    [SerializeField] private float multiplierTime;
    [Header("Camera")]
    [SerializeField] private Transform cam;

    private Vector2 moveDirection;
    private Vector2 lookDirection;
    private Vector3 velocity;
    private Vector3 externalForce;
    private bool isSprinting = false;
    private bool isMultiplierOn = false;
    private bool inWindZone = false;
    private CharacterController controller;
    private int jumpsRemaining = 0;
    private IMovementSystem currentMovement;
    public float BaseSpeed => isSprinting ? baseSpeed * 2 : baseSpeed;
    public float Gravity => gravity;
    public Vector2 MoveInput => moveDirection;
    public Vector2 LookInput => lookDirection;
    public Vector3 Velocity { get => velocity; set => velocity = value; }
    public Vector3 ExternalForce { get => externalForce; set => externalForce = value; }
    public Transform Cam => cam;
    public CharacterController Controller => controller;
    public int JumpsRemaining { get => jumpsRemaining; set => jumpsRemaining = value; }
    public bool IsGrounded => controller.isGrounded;
    public bool IsSprinting => isSprinting;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    void Start()
    {
        SetMovement(movementType);
    }
    private void OnEnable()
    {
        input.MoveEvent += OnMove;
        input.LookEvent += OnLook;
        input.JumpEvent += OnJump;
        input.SprintEvent += OnSprintStart;
        input.SprintCancelledEvent += OnSprintStop;
        input.CrouchEvent += OnCrouch;
    }
    private void OnDisable()
    {
        input.MoveEvent -= OnMove;
        input.LookEvent -= OnLook;
        input.JumpEvent -= OnJump;
        input.SprintEvent -= OnSprintStart;
        input.SprintCancelledEvent -= OnSprintStop;
        input.CrouchEvent -= OnCrouch;
    }
    void Update()
    {
        currentMovement.ApplyGravity(this);
        currentMovement.Move(this);
        HandleLookRotation();
        DecayExternalForce();
    }
    private void OnMove(Vector2 direction)
    {
        moveDirection = direction;
    }
    private void OnLook(Vector2 direction)
    {
        lookDirection = direction;
    }
    private void OnJump()
    {
        currentMovement.HandleJump(this);
    }
    private void OnSprintStart()
    {
        isSprinting = true;
    }
    private void OnSprintStop()
    {
        isSprinting = false;
    }
    private void OnCrouch()
    {
        currentMovement.Crouch(this);
    }
    private void HandleLookRotation()
    {
        if (moveDirection == Vector2.zero) return;

        Vector3 dir = cam.forward * moveDirection.y + cam.right * moveDirection.x;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
    private void DecayExternalForce()
    {
        float decayRate = inWindZone ? 0.1f : 0.5f;
        externalForce = Vector3.Lerp(externalForce, Vector3.zero, Time.deltaTime * decayRate);
    }
    public void ApplyExternalForce(Vector3 wind, float windSpeed)
    {
        if (wind == Vector3.zero) return;

        Vector3 force = wind.normalized * windSpeed;
        velocity.y = force.y;
        force.y = 0;
        externalForce += force;
    }
    public void ActivateMultiplier()
    {
        if (!isMultiplierOn)
            StartCoroutine(MultiplierRoutine());
    }
    private IEnumerator MultiplierRoutine()
    {
        isMultiplierOn = true;
        float originalSpeed = baseSpeed;
        baseSpeed *= multiplierSpeed;
        yield return new WaitForSeconds(multiplierTime);
        baseSpeed = originalSpeed;
        isMultiplierOn = false;
    }
    public void SetMovement(MovementType type)
    {
        switch (type)
        {
            case MovementType.Landed:
                currentMovement = landed;
                break;
            case MovementType.Glide:
                currentMovement = glide;
                break;
            case MovementType.Underwater:
                currentMovement = underwater;
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            SetMovement(MovementType.Underwater);
        }
        else if (other.CompareTag("Wind"))
        {
            SetMovement(MovementType.Glide);
            inWindZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            SetMovement(MovementType.Landed);
        }
        else if (other.CompareTag("Wind"))
        {
            SetMovement(MovementType.Landed);
            inWindZone = false;
            if (!IsGrounded)
            {
                Velocity = new Vector3(Velocity.x, 0f, Velocity.z);
                Velocity += Vector3.up * Gravity * Time.deltaTime;
            }
        }
    }
}
