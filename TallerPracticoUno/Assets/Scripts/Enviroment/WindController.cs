using UnityEngine;
using System.Collections;
[RequireComponent(typeof(WindBehavior))]
public class WindController : MonoBehaviour
{
    [Header("Direction Change Settings")]
    [SerializeField] private bool enableDirectionChange = false;
    [SerializeField] private float directionChangeInterval = 5f;
    [SerializeField] private Vector3 rotationPerStep = new Vector3(0f, 90f, 0f);//XYZ
    [Header("Speed Change Settings")]
    [SerializeField] private bool enableSpeedChange = false;
    [SerializeField] private float speedChangeInterval = 4f;
    [SerializeField] private Vector2 speedRange = new Vector2(3f, 10f);//min/max speed
    private WindBehavior wind;
    private ParticleSystem windParticles;
    private float particlesRate = 1f;
    private void Start()
    {
        wind = GetComponent<WindBehavior>();
        // Obtener referencia al sistema de partículas y tasa
        var windField = typeof(WindBehavior).GetField("windParticles", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var rateField = typeof(WindBehavior).GetField("particlesRate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (windField != null && rateField != null)
        {
            windParticles = windField.GetValue(wind) as ParticleSystem;
            particlesRate = (float)rateField.GetValue(wind);
        }
        if (enableDirectionChange) StartCoroutine(RotateWindRoutine());

        if (enableSpeedChange) StartCoroutine(ChangeSpeedRoutine());
    }
    private IEnumerator RotateWindRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(directionChangeInterval);
            transform.Rotate(rotationPerStep);
            //UpdateParticleDirection();
        }
    }
    private IEnumerator ChangeSpeedRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(speedChangeInterval);
            float newSpeed = Random.Range(speedRange.x, speedRange.y);
            SetWindSpeed(newSpeed);
        }
    }
    private void SetWindSpeed(float newSpeed)
    {
        var windField = typeof(WindBehavior).GetField("windSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (windField != null)
        {
            windField.SetValue(wind, newSpeed);
            //UpdateParticleEmission(newSpeed);
        }
    }
    /*private void UpdateParticleEmission(float speed)
    {
        if (windParticles == null) return;
        var emission = windParticles.emission;
        emission.rateOverTime = speed * particlesRate;
    }
    private void UpdateParticleDirection()
    {
        if (windParticles == null) return;
        //Particles in the direction of the wind
        var shape = windParticles.shape;
        shape.rotation = transform.eulerAngles;
        var main = windParticles.main;
        main.startRotation3D = true;
        //Adjust direction
        main.startRotationX = Mathf.Deg2Rad * transform.eulerAngles.x;
        main.startRotationY = Mathf.Deg2Rad * transform.eulerAngles.y;
        main.startRotationZ = Mathf.Deg2Rad * transform.eulerAngles.z;
    }*/
}