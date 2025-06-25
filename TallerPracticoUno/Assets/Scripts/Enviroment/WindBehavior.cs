using UnityEngine;

public class WindBehavior : MonoBehaviour
{
    private Vector3 windDirection;
    [SerializeField] private float windSpeed;
    [SerializeField] private bool applyContinous = false;
    private void Start()
    {
        //windDirection = transform.forward;
    }
    private void OnTriggerEnter(Collider other)
    {
        windDirection = transform.forward;
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ApplyExternalForce(windDirection, windSpeed);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.SetMovement(MovementType.Glide);
            if (applyContinous)
            {
                player.ApplyExternalForce(transform.forward, windSpeed);
            }
        }
    }
}
