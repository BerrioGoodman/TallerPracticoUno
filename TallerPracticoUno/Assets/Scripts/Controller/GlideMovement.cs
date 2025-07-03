using UnityEngine;
[CreateAssetMenu(menuName = "Player/Movement/Glide")]
public class GlideMovement : ScriptableObject, IMovementSystem
{
    public void Move(PlayerController player)
    {
        Vector3 dir = player.Cam.forward * player.MoveInput.y + player.Cam.right * player.MoveInput.x;
        dir.y = 0f;
        dir.Normalize();
        Vector3 move = dir * player.BaseSpeed + player.ExternalForce;
        move.y = player.Velocity.y;
        player.Controller.Move(move * Time.deltaTime);
    }
    public void ApplyGravity(PlayerController player)
    {
        // Soft glide gravity
        player.Velocity += Vector3.up * player.Gravity * Time.deltaTime * -0.3f;
    }
    public void HandleJump(PlayerController player)
    {
    }
    public void Crouch(PlayerController player)
    {
    }
}
