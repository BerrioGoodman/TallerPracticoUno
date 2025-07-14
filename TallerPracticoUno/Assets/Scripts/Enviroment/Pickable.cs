using UnityEngine;

public class Pickable : MonoBehaviour
{
    private bool isPicked = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isPicked) return;
        PlayerPickup player = other.GetComponent<PlayerPickup>();
        if (player != null && !player.IsCarrying())
        {
            isPicked = true;
            player.PickUp(this);
        }
    }
    public void AttachTo(Transform carryPoint)
    {
        transform.SetParent(carryPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        GetComponent<Collider>().enabled = false;
    }
    public void DeliverTo(Transform dropPoint)
    {
        transform.SetParent(dropPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        GetComponent<Collider>().enabled = true;
        isPicked = false;
    }
    public void ResetToOriginal()
    {
        transform.SetParent(null);
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        GetComponent<Collider>().enabled = true;
        isPicked = false;
    }
}
