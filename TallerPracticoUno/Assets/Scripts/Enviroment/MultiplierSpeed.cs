using UnityEngine;

public class MultiplierSpeed : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ActivateMultiplier();
            player.RechargeDurability();
            Destroy(gameObject);
        }
    }
}
