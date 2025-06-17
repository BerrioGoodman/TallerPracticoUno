using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader input;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float gravity;//Must be soft
    [SerializeField] private float multiplierSpeed;
    [SerializeField] private Transform cam;
    private Vector2 moveDirection;
    private Vector2 lookDirection;
    private Vector3 velocity;
    private Vector3 externalForce;
    private CharacterController controller;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    void Start()
    {
        input.MoveEvent += HandleMove;
        input.LookEvent += HandleLook;
    }
    void Update()
    {
        ApplyGravity();
        Move();
        Look();
        DecayExternalForce();
    }
    private void HandleMove(Vector2 dir)
    {
        moveDirection = dir;
    }
    private void HandleLook(Vector2 di)
    {
        lookDirection = di;
    }
    private void ApplyGravity()
    {

        velocity.y += gravity * Time.deltaTime;
    }
    private void Move()
    {
        Vector3 movement = Vector3.zero;
        Vector3 movementDirection = cam.forward * moveDirection.y + cam.right * moveDirection.x;
        movementDirection.y = 0;
        movementDirection.Normalize();
        Vector3 direction = movementDirection * speed;

        //Combining gravity, movement and wind
        Vector3 finalMove = direction;
        finalMove.y = velocity.y;
        finalMove += externalForce;
        controller.Move(finalMove * Time.deltaTime);
    }
    private void Look()
    {
        if (moveDirection == Vector2.zero) return;
        Vector3 lookDirection = cam.forward * moveDirection.y + cam.right * moveDirection.x;
        lookDirection.y = 0;
        lookDirection.Normalize();
        if (lookDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }
    private void DecayExternalForce()
    {
        externalForce = Vector3.Lerp(externalForce, Vector3.zero, Time.deltaTime * 0.5f);
    }
    //Wind
    public void ApplyExternalForce(Vector3 wind, float windSpeed)
    {
        if (wind == Vector3.zero) return;
        Vector3 speed = wind.normalized * windSpeed;
        //We use the y component of the wind
        velocity.y = speed.y;
        //Only use horizontal components for external force
        speed.y = 0;
        externalForce += speed;
    }
}
