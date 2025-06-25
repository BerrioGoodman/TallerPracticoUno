using UnityEngine;
[CreateAssetMenu(menuName = "Player/Movement/Underwater")]
public class UnderwaterMovement : ScriptableObject, IMovementSystem
{
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float bouyancy;
    [SerializeField] private float verticalDrag;
    [SerializeField] private float horizontalDrag;
    public void Move(PlayerController player)
    {
        Vector3 dir = player.Cam.forward * player.MoveInput.y + player.Cam.right * player.MoveInput.x;
        dir.y = 0f;
        dir.Normalize();
        Vector3 move = dir * player.BaseSpeed + player.ExternalForce;
        move.y = player.Velocity.y;
        move.x = Mathf.Lerp(move.x, 0, horizontalDrag * Time.deltaTime);
        move.z = Mathf.Lerp(move.z, 0, horizontalDrag * Time.deltaTime);
        player.Controller.Move(move * Time.deltaTime);
    }
    public void ApplyGravity(PlayerController player)
    {
        player.Velocity += Vector3.up * bouyancy * Time.deltaTime;
        player.Velocity = new Vector3(player.Velocity.x, Mathf.Lerp(player.Velocity.y, 0, verticalDrag * Time.deltaTime), player.Velocity.z);
    }
    public void HandleJump(PlayerController player)
    {
        player.Velocity = new Vector3(player.Velocity.x, verticalSpeed, player.Velocity.z);
    }
    public void Crouch(PlayerController player)
    {
        player.Velocity = new Vector3(player.Velocity.x, -verticalSpeed, player.Velocity.z);
    }
}
