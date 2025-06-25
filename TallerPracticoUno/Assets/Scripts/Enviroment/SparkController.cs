using UnityEngine;

public class SparkController : MonoBehaviour
{
    [SerializeField] private ParticleSystem sparkParticles;
    [Header("Minimal Speed")]
    [SerializeField] private float movementTrigger;
    [Header("Spark Force idle and moving")]
    [SerializeField] private float idleEmission;
    [SerializeField] private float maxEmission;
    [SerializeField] private float idleUpForce;
    [SerializeField] private float movingForceMultiplier;
    private CharacterController controller;
    private Vector3 lastPosition;
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;
    private ParticleSystem.EmissionModule emissionModule;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
        velocityModule = sparkParticles.velocityOverLifetime;
        velocityModule.enabled = true;
        emissionModule = sparkParticles.emission;
        emissionModule.enabled = true;
        //Be sure particles work in the world space
        var mainModule = sparkParticles.main;
        mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
        mainModule.playOnAwake = true;
        sparkParticles.Play();
    }
    private void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 velocity = (currentPosition - lastPosition) / Time.deltaTime;
        lastPosition = currentPosition;
        float speed = velocity.magnitude;
        if (speed > movementTrigger)
        {
            Vector3 oppositeDirection = -velocity.normalized * movingForceMultiplier;
            velocityModule.x = new ParticleSystem.MinMaxCurve(oppositeDirection.x);
            velocityModule.y = new ParticleSystem.MinMaxCurve(oppositeDirection.y);
            velocityModule.z = new ParticleSystem.MinMaxCurve(oppositeDirection.z);
            float emiisionRate = Mathf.Lerp(idleEmission, maxEmission, speed / 10);
            emissionModule.rateOverTime = emiisionRate;
        }
        else
        {
            velocityModule.x = new ParticleSystem.MinMaxCurve(0);
            velocityModule.y = new ParticleSystem.MinMaxCurve(idleUpForce);
            velocityModule.z = new ParticleSystem.MinMaxCurve(0);
            emissionModule.rateOverTime = idleEmission;
        }
    }
}
