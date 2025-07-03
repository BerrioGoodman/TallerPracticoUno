using UnityEngine;

public interface IMovementSystem
{
    void Move(PlayerController player);
    void ApplyGravity(PlayerController player);
    void HandleJump(PlayerController player);
    void Crouch(PlayerController player);
}
