using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicReceiver : MonoBehaviour
{
    [Header("Settings")]
    public bool hasMagic;
    [SerializeField] private float transition = 1f;

    [Header("Glow Settings")]
    [Tooltip("Intensidad del glow respecto al color base")]
    [SerializeField] private float glowMultiplier = 0.3f;

    private Renderer rend;
    private Coroutine magicRoutine;
    private List<Material> materials = new List<Material>();

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        //Instance for materials just once
        foreach (Material mat in rend.materials)
        {
            Material matInstance = new Material(mat);
            matInstance.DisableKeyword("_EMISSION");//Start with no emission
            materials.Add(matInstance);
        }
        rend.materials = materials.ToArray();
        hasMagic = false;
    }
    public void SetMagic(bool magic, bool instant = false)
    {
        if (hasMagic == magic) return;
        hasMagic = magic;
        if (magicRoutine != null) StopCoroutine(magicRoutine);
        magicRoutine = StartCoroutine(TransitionGlow(magic, instant));
    }
    private IEnumerator TransitionGlow(bool enable, bool instant)
    {
        float t = 0;
        List<Color> startEmissionColors = new List<Color>();
        List<Color> targetEmissionColors = new List<Color>();
        for (int i = 0; i < materials.Count; i++)
        {
            Material mat = materials[i];
            Color baseColor = mat.GetColor("_Color");//We use the color of the material
            Color currentEmission = mat.GetColor("_EmissionColor");
            startEmissionColors.Add(currentEmission);
            Color targetEmission = enable ? baseColor * glowMultiplier : Color.black;
            targetEmissionColors.Add(targetEmission);
        }
        if (instant)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                Material mat = materials[i];
                mat.SetColor("_EmissionColor", targetEmissionColors[i]);
                if (enable) mat.EnableKeyword("_EMISSION");
                else mat.DisableKeyword("_EMISSION");
            }
            yield break;
        }
        while (t < 1f)
        {
            t += Time.deltaTime / transition;
            for (int i = 0; i < materials.Count; i++)
            {
                Material mat = materials[i];
                Color newEmission = Color.Lerp(startEmissionColors[i], targetEmissionColors[i], t);
                mat.SetColor("_EmissionColor", newEmission);

                if (newEmission.maxColorComponent > 0.01f)
                    mat.EnableKeyword("_EMISSION");
            }
            yield return null;
        }
        if (!enable)
        {
            foreach (Material mat in materials)
                mat.DisableKeyword("_EMISSION");
        }
    }
}
