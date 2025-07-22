using UnityEngine;
public class FinalPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.DisableUnderwaterFilter();//If we´re getting in from water
                AudioManager.Instance.PlaySFX("Portal");
                CamerasManager.Instance.StartFinalPanoramic();//Start Panoramic Scene
                gameObject.SetActive(false);
            }
        }
    }
}
