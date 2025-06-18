using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MagicReceiver : MonoBehaviour
{
    [Header("Settings")]
    public bool hasMagic;
    [SerializeField] private float propagationRadius;
    [SerializeField] private float transitionDuration;
    [SerializeField] private float delay;
    [Header("Visuals")]
    [SerializeField] private Color darkColor = Color.black;
    [SerializeField] private Color lightColor = Color.white;
    [SerializeField] private Color darkEmission = Color.black;
    [SerializeField] private Color lightEmission = Color.white;
    private Material material;
    private Renderer rend;
    private Coroutine MagicRoutine;
    private void Awake()
    {
        rend = GetComponent<Renderer>();
        material = new Material(rend.material);
        rend.material = material;
        SetMagic(false, instant: true);
    }
    public void SetMagic(bool magic, bool instant = false)
    {
        if (hasMagic && magic) return;
        hasMagic = magic;
        if (MagicRoutine != null)
        {
            StopCoroutine(MagicRoutine);
        }
        MagicRoutine = StartCoroutine(magic ? GradualMagic(instant) : GradualMagicDown(instant));
    }
    private IEnumerator GradualMagic(bool instant)
    {
        float t = 0;
        Color startColor = material.color;
        Color startEmission = material.GetColor("_EmissionColor");
        if (instant)
        {
            material.color = lightColor;
            material.SetColor("_EmissionColor", lightEmission);
            yield break;
        }
        while (t < 1)
        {
            t += Time.deltaTime / transitionDuration;
            material.color = Color.Lerp(startColor, lightColor, t);
            material.SetColor("_EmissionColor", Color.Lerp(startEmission, lightEmission, t));
            yield return null;
        }
        StartCoroutine(PropagateMagic());
    }
    private IEnumerator GradualMagicDown(bool instant)
    {
        float t = 0;
        Color startColor = material.color;
        Color startEmission = material.GetColor("_EmissionColor");
        if (instant)
        {
            material.color = darkColor;
            material.SetColor("_EmissionColor", darkEmission);
            yield break;
        }
        while (t < 1)
        {
            t += Time.deltaTime / transitionDuration;
            material.color = Color.Lerp(startColor, darkColor, t);
            material.SetColor("_EmissionColor", Color.Lerp(startEmission, darkEmission, t));
            yield return null;
        }
    }
    public IEnumerator PropagateMagic()
    {
        yield return new WaitForSeconds(delay);
        Collider[] close = Physics.OverlapSphere(transform.position, propagationRadius);
        foreach (Collider col in close)
        {
            MagicReceiver near = col.GetComponent<MagicReceiver>();
            if (near != null && !near.hasMagic)
            {
                near.SetMagic(true);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
