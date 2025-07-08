using UnityEngine;
[CreateAssetMenu(menuName = "Player/Movement/Landed")]
public class LandedMovement : ScriptableObject, IMovementSystem
{
    [SerializeField] private float jumpForce;
    [SerializeField] private int maxJumps;
    [SerializeField] private float damagePerSecond;
    public void Move(PlayerController player)
    {
        Vector3 dir = player.Cam.forward * player.MoveInput.y + player.Cam.right * player.MoveInput.x;
        dir.y = 0f;
        dir.Normalize();
        float finalSpeed = player.BaseSpeed;
        Vector3 move = dir * finalSpeed + player.ExternalForce;
        move.y = player.Velocity.y;
        player.Controller.Move(move * Time.deltaTime);
    }
    public void ApplyGravity(PlayerController player)
    {
        player.Velocity += Vector3.up * player.Gravity * Time.deltaTime;
        if (player.Controller.isGrounded && player.Velocity.y < 0)
        {
            player.Velocity = new Vector3(player.Velocity.x, -2, player.Velocity.z);
            player.JumpsRemaining = maxJumps;
        }
    }
    public void HandleJump(PlayerController player)
    {
        if (player.JumpsRemaining > 0)
        {
            player.Velocity = new Vector3(player.Velocity.x, jumpForce, player.Velocity.z);
            player.JumpsRemaining--;
            AudioManager.Instance.PlaySFX("Jump");
        }
    }
    public void Crouch(PlayerController player)
    {
        //Pending this mechaninc
    }
    public float GetDurabilityDamage()
    {
        return damagePerSecond;
    }
    public void OnEnter(PlayerController player)
    {
        AudioManager.Instance.PlayMusic("Ambience");
        AudioManager.Instance.PlayMusic("Character_Loop");
    }
    public void OnExit(PlayerController player)
    {
        
    }
}
