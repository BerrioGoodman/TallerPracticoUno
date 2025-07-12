using UnityEngine;
using System.Collections.Generic;
public class DeliveryPoint : MonoBehaviour
{
    [Header("Entrega")]
    [SerializeField] private List<Transform> dropPositions = new List<Transform>();
    [SerializeField] private AudioClip deliverSfx;
    private int currentIndex = 0;
    private void OnTriggerEnter(Collider other)
    {
        PlayerPickup player = other.GetComponent<PlayerPickup>();
        if (player != null && player.IsCarrying() && currentIndex < dropPositions.Count)
        {
            Pickable pick = player.Drop();
            Transform dropPoint = dropPositions[currentIndex];

            pick.DeliverTo(dropPoint);
            AudioSource.PlayClipAtPoint(deliverSfx, dropPoint.position);
            GameManager.Instance.RegisterDelivery();
            currentIndex++;
        }
    }
}
