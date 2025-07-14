using UnityEngine;
using System.Collections.Generic;
public class DeliveryPoint : MonoBehaviour
{
    [Header("Delivery")]
    [SerializeField] private List<Transform> dropPositions = new List<Transform>();
    [SerializeField] private AudioClip deliverSfx;
    private int currentIndex = 0;
    private void OnTriggerEnter(Collider other)
    {
        PlayerPickup player = other.GetComponentInParent<PlayerPickup>();
        if (player != null && player.IsCarrying() && currentIndex < dropPositions.Count)
        {
            Pickable pick = player.Drop();
            Transform dropPoint = dropPositions[currentIndex];

            pick.DeliverTo(dropPoint);
            AudioManager.Instance.PlaySFX("Rune");
            GameManager.Instance.RegisterDelivery();
            CameraManager.Instance.FocusOnCinematicPoint();
            currentIndex++;
        }
    }
}
