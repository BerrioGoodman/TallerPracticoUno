using System;
using UnityEngine;

public class FlameHUDController : BaseController
{
    [Header("Dependencies")] [Tooltip("El scriptableObject con los datos de la llama")] [SerializeField]
    private FlameStats_SO flameModel;

    [Tooltip("La vista que contiene los elementos de la UI")] [SerializeField]
    private FlameHUDView flameView;

    private void OnEnable()
    {
        flameModel.OnDurabilityChanged += HandleDurabilityChanged;
        
        //actualizamos la vista con el estado inicial
        var initialState = flameModel.GetCurrentState();
        flameView.UpdateDurabilityBar(initialState.Item1, initialState.Item2);
    }

    private void OnDisable()
    {
        flameModel.OnDurabilityChanged -= HandleDurabilityChanged;
    }

    private void HandleDurabilityChanged(float current, float max)
    {
        flameView.UpdateDurabilityBar(current, max);
    }
}
