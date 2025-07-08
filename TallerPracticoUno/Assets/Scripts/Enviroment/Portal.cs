using UnityEngine;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = GetComponent<PlayerController>();
            if (player != null)
            {
                AudioManager.Instance.PlaySFX("Portal_Small");
                player.StartTeleport();
            }
        }
    }
}
