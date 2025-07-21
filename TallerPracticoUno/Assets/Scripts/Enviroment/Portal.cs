using UnityEngine;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.DisableUnderwaterFilter();//Deactivate filter
                AudioManager.Instance.PlaySFX("Portal_Small");
                player.StartTeleport();
            }
        }
    }
}
