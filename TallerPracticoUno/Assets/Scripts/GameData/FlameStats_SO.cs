using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FlameStats_SO", menuName = "Scriptable Objects/Flame Stats")]
public class FlameStats_SO : ScriptableObject
{
    public event Action<float, float> OnDurabilityChanged;

    public float maxDurability = 100f;
    [SerializeField] private float currentDurability;

    //inicializa currentDurability al valor de maxDurability
    private void OnEnable()
    {
        currentDurability = maxDurability;
    }

    //baja la durabilidad en una cantidad especificada
    public void DecreaseDurability(float amount)
    {
        currentDurability -= amount;

        if (currentDurability < 0)
        {
            currentDurability = 0;
        }

        OnDurabilityChanged?.Invoke(currentDurability, maxDurability);
    }

    // aumenta la durabilidad en una cantidad especificada
    public void Recharge() 
    {
        currentDurability = maxDurability;
        OnDurabilityChanged?.Invoke(currentDurability, maxDurability);
    }

    // obtiene la durabilidad actual y la durabilidad m�xima
    public (float, float) GetCurrentState()
    {
        return (currentDurability, maxDurability);
    }

    public void Reset()
    {
        currentDurability = maxDurability;
        // Disparamos el evento para que la UI se actualice al valor máximo si está visible.
        OnDurabilityChanged?.Invoke(currentDurability, maxDurability);
    }
}
