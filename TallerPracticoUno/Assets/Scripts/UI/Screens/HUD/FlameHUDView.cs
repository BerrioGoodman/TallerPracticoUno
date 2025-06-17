using UnityEngine;
using UnityEngine.UI;

public class FlameHUDView: BaseView
{
    [Header("UI References")]
    [SerializeField] private Slider durabilityBar;

    public void UpdateDurabilityBar(float current, float max)
    {
        if (durabilityBar != null) 
        {
            durabilityBar.value = current / max;
        }
    }
}
