using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader input;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private Transform cam;
    private Vector2 moveDirection;
    private Vector2 lookDirection;
    private bool isJumping;
    void Start()
    {
        input.MoveEvent += HandleMove;
        input.LookEvent += HandleLook;
        input.JumpEvent += HandleJump;
        input.JumpCancelledEvent += HandleCancelledJump;
    }
    void Update()
    {
        Move();
        Look();
        Jump();
    }
    private void HandleMove(Vector2 dir)
    {
        moveDirection = dir;
    }
    private void HandleLook(Vector2 di)
    {
        lookDirection = di;
    }
    private void HandleJump()
    {
        isJumping = true;
    }
    private void HandleCancelledJump()
    {
        isJumping = false;
    }
    private void Move()
    {
        if (moveDirection == Vector2.zero) return;
        /*transform.position += new Vector3(x: moveDirection.x, y: 0, z: moveDirection.y) * (speed * Time.deltaTime);
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();*/
        Vector3 movementDirection = cam.forward * moveDirection.y + cam.right * moveDirection.x;
        movementDirection.y = 0;
        movementDirection.Normalize();
        transform.position += (movementDirection) * (speed * Time.deltaTime);
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
    private void Jump()
    {
        if (isJumping)
        {
            transform.position += new Vector3(x: 0, y: 1, z: 0) * (jumpSpeed * Time.deltaTime);
        }
    }
}
