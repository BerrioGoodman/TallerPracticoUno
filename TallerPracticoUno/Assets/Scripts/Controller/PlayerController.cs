using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private InputReader input;
    [Header("Player Stats")]
    [SerializeField] private FlameStats_SO stats;
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
    [Header("Teleportation")]
    [SerializeField] private float teleportTime;
    [SerializeField] private Transform startPoint;

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
    private Coroutine durabilityCoroutine;
    private Coroutine teleportCoroutine;
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
        currentMovement.OnEnter(this);
    }
    private void OnEnable()
    {
        input.MoveEvent += OnMove;
        input.LookEvent += OnLook;
        input.JumpEvent += OnJump;
        input.SprintEvent += OnSprintStart;
        input.SprintCancelledEvent += OnSprintStop;
        input.CrouchEvent += OnCrouch;
        stats.OnDurabilityChanged += OnDurabilityChanged;
    }
    private void OnDisable()
    {
        input.MoveEvent -= OnMove;
        input.LookEvent -= OnLook;
        input.JumpEvent -= OnJump;
        input.SprintEvent -= OnSprintStart;
        input.SprintCancelledEvent -= OnSprintStop;
        input.CrouchEvent -= OnCrouch;
        stats.OnDurabilityChanged -= OnDurabilityChanged;
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
        {
            AudioManager.Instance.PlaySFX("PowerUp");
            StartCoroutine(MultiplierRoutine());
        }
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
    private IEnumerator DurabilityCoroutine()
    {
        while (true)
        {
            float damage = currentMovement.GetDurabilityDamage();
            float currentDurability = stats.GetCurrentState().Item1;
            if (currentDurability > 0) stats.DecreaseDurability(damage * Time.deltaTime);
            yield return null;
        }
    }
    public void SetMovement(MovementType type)
    {
        movementType = type;
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
        if (durabilityCoroutine != null) StopCoroutine(durabilityCoroutine);
        durabilityCoroutine = StartCoroutine(DurabilityCoroutine());
    }
    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Water"))
        {
            SetMovement(MovementType.Underwater);
        }
        else if (other.CompareTag("Wind"))
        {
            SetMovement(MovementType.Glide);
            inWindZone = true;
        }*/
        if (other.CompareTag("Water"))
        {
            currentMovement?.OnExit(this);
            SetMovement(MovementType.Underwater);
            underwater.OnEnter(this);
        }
        else if (other.CompareTag("Wind"))
        {
            currentMovement?.OnExit(this);
            SetMovement(MovementType.Glide);
            glide.OnEnter(this);
            inWindZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        /*if (other.CompareTag("Water"))
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
        }*/
        if (other.CompareTag("Water"))
        {
            underwater.OnExit(this);
            SetMovement(MovementType.Landed);
            landed.OnEnter(this);
        }
        else if (other.CompareTag("Wind"))
        {
            glide.OnExit(this);
            SetMovement(MovementType.Landed);
            landed.OnEnter(this);
            inWindZone = false;
            if (!IsGrounded)
            {
                Velocity = new Vector3(Velocity.x, 0f, Velocity.z);
                Velocity += Vector3.up * Gravity * Time.deltaTime;
            }
        }
    }
    public void RechargeDurability()
    {
        stats.Recharge();
    }
    private void OnDurabilityChanged(float current, float max)
    {
        Debug.Log($"Durabilidad actual: {current} / {max}");
    }
    public void StartTeleport()
    {
        if (teleportCoroutine != null)
            StopCoroutine(teleportCoroutine);

        teleportCoroutine = StartCoroutine(TeleportLerp(startPoint.position, startPoint.rotation));
    }
    private IEnumerator TeleportLerp(Vector3 targetPos, Quaternion targetRot)
    {
        controller.enabled = false;

        Vector3 initialPos = transform.position;
        Quaternion initialRot = transform.rotation;
        float elapsed = 0f;

        while (elapsed < teleportTime)
        {
            float t = elapsed / teleportTime;
            transform.position = Vector3.Lerp(initialPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(initialRot, targetRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.SetPositionAndRotation(targetPos, targetRot);
        Velocity = Vector3.zero;
        ExternalForce = Vector3.zero;
        controller.enabled = true;

        SetMovement(MovementType.Landed);
        currentMovement.OnEnter(this);
    }
}
