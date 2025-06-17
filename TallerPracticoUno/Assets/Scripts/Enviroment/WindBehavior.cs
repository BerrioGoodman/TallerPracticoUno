using UnityEngine;

public class WindBehavior : MonoBehaviour
{
    [SerializeField] private Vector3 windDirection;
    [SerializeField] private float windSpeed;
    [SerializeField] private bool applyContinous = false;
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ApplyExternalForce(windDirection, windSpeed);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!applyContinous) return;
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ApplyExternalForce(windDirection, windSpeed * Time.deltaTime);
        }
    }
}
