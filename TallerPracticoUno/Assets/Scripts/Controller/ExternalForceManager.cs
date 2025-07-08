using UnityEngine;

public class ExternalForceManager
{
    private float _windDecay;
    private float _normalDecay;
    public ExternalForceManager(float normalDecay = 0.5f, float windDecay = 0.1f)
    {
        _normalDecay = normalDecay;
        _windDecay = windDecay;
    }
    public Vector3 Decay(Vector3 currentForce, bool inWindZone)
    {
        float decayRate;

        if (inWindZone)
        {
            decayRate = _windDecay;
        }
        else
        {
            decayRate = _normalDecay;
        }

        return Vector3.Lerp(currentForce, Vector3.zero, Time.deltaTime * decayRate);
    }
    public Vector3 ApplyWind(Vector3 windDirection, float windSpeed, ref Vector3 velocity)
    {
        if (windDirection == Vector3.zero)
        {
            return Vector3.zero;
        }

        Vector3 force = windDirection.normalized * windSpeed;
        velocity.y = force.y;
        force.y = 0;

        return force;
    }
    public void SetWindDecay(float newDecay)
    {
        _windDecay = newDecay;
    }
    public void SetNormalDecay(float newDecay)
    {
        _normalDecay = newDecay;
    }
}
