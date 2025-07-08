using UnityEngine;

public class WindBehavior : MonoBehaviour
{
    private Vector3 windDirection;
    [SerializeField] private float windSpeed;
    [SerializeField] private bool applyContinous = false;
    [SerializeField] private ParticleSystem windParticles;
    [SerializeField] private float particlesRate;
    private ParticleSystem.EmissionModule emission;
    private void Start()
    {
        windDirection = transform.forward;

        if (windParticles != null)
        {
            emission = windParticles.emission;
            //Activate wind particles
            emission.rateOverTime = windSpeed * particlesRate;
            windParticles.Play();
        }
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
