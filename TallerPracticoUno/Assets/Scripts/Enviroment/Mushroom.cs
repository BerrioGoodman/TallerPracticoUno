using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float bounceForce = 20f;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string bounceSFXName = "Fire_Collision";
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag(playerTag)) return;
        PlayerController player = collision.collider.GetComponentInParent<PlayerController>();
        if (player == null) return;
        //Check contacts points
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                Vector3 newVelocity = player.Velocity;
                newVelocity.y = Mathf.Sqrt(bounceForce * -2f * player.Gravity);
                player.Velocity = newVelocity;
                if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX(bounceSFXName);
                break;
            }
        }
    }
}
