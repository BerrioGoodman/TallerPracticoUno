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

    private MovementHandler movementHandler;
    private ExternalForceManager forceManager;
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
    private bool isTeleporting = false;
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
        movementHandler = new MovementHandler(transform, cam, rotationSpeed);
        forceManager = new ExternalForceManager();
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
        if (isTeleporting || !controller.enabled) return;
        currentMovement.ApplyGravity(this);
        currentMovement.Move(this);
        movementHandler.Rotate(moveDirection);
        externalForce = forceManager.Decay(externalForce, inWindZone);
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
    public void ApplyExternalForce(Vector3 wind, float windSpeed)
    {
        Vector3 force = forceManager.ApplyWind(wind, windSpeed, ref velocity);
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
        currentMovement = MovementFactory.GetMovement(type, landed, glide, underwater);
        RestartDurabilityCoroutine();
    }
    private void RestartDurabilityCoroutine()
    {
        if (durabilityCoroutine != null)
        {
            StopCoroutine(durabilityCoroutine);
        }
        durabilityCoroutine = StartCoroutine(DurabilityCoroutine());
    }
    private void OnTriggerEnter(Collider other)
    {
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
        //Debug.Log($"Durabilidad actual: {current} / {max}");
    }
    public void StartTeleport()
    {
        if (teleportCoroutine != null) StopCoroutine(teleportCoroutine);
        teleportCoroutine = StartCoroutine(TeleportLerp(startPoint.position, startPoint.rotation));
    }
    private IEnumerator TeleportLerp(Vector3 targetPos, Quaternion targetRot)
    {
        isTeleporting = true;
        controller.enabled = false;
        SetPlayerVisible(false);
        yield return new WaitForSeconds(teleportTime);
        transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);
        velocity = Vector3.zero;
        externalForce = Vector3.zero;
        SetPlayerVisible(true);
        controller.enabled = true;
        SetMovement(MovementType.Landed);
        currentMovement.OnEnter(this);
        isTeleporting = false;
    }
    private void SetPlayerVisible(bool visible)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = visible;
        }
    }
    public void ResetPickupIfAny()
    {
        GetComponent<PlayerPickup>()?.DropOnDeath();
    }
}
