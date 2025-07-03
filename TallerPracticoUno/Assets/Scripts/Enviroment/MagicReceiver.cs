using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagicReceiver : MonoBehaviour
{
    [Header("Settings")]
    public bool hasMagic;
    [SerializeField] private float transition;
    [SerializeField] private float delay;
    [Header("Initial State")]
    [SerializeField] private Material darkMaterial;
    [SerializeField] private Material originalMaterial;
    private Renderer rend;
    private Coroutine MagicRoutine;
    private List<MagicReceiver> nearObjects = new List<MagicReceiver>();
    private void Awake()
    {
        rend = GetComponent<Renderer>();
        ApplyMaterial(darkMaterial);
        hasMagic = false;
    }
    public void SetMagic(bool magic, bool instant = false)
    {
        if (hasMagic == magic) return;
        hasMagic = magic;
        if (MagicRoutine != null) StopCoroutine(MagicRoutine);
        MagicRoutine = StartCoroutine(GradualMagicUp(magic ? originalMaterial : darkMaterial, instant));
    }
    private IEnumerator GradualMagicUp(Material targetMaterial, bool instant)
    {
        if (instant)
        {
            ApplyMaterial(targetMaterial);
            if (hasMagic) StartCoroutine(PropagateMagic());
            yield break;
        }
        Material currentMaterial = rend.material;
        float t = 0;
        Color startColor = currentMaterial.color;
        Color startEmission = currentMaterial.GetColor("_EmissionColor");
        Color targetColor = targetMaterial.color;
        Color targetEmission = targetMaterial.GetColor("_EmissionColor");
        while (t < 1)
        {
            t += Time.deltaTime / transition;
            Color lerpedColor = Color.Lerp(startColor, targetColor, t);
            Color lerpEmission = Color.Lerp(startEmission, targetEmission, t);
            currentMaterial.color = lerpedColor;
            currentMaterial.SetColor("_EmissionColor", lerpEmission);
            yield return null;
        }
        ApplyMaterial(targetMaterial);
        if (hasMagic)
        {
            StartCoroutine(PropagateMagic());
        }
    }
    private void ApplyMaterial(Material mat)
    {
        rend.material = new Material(mat);
    }
    public IEnumerator PropagateMagic()
    {
        yield return new WaitForSeconds(delay);
        foreach (MagicReceiver neighbor in nearObjects)
        {
            if (neighbor != null && !neighbor.hasMagic)
            {
                neighbor.SetMagic(true);
                yield return new WaitForSeconds(delay);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        MagicReceiver neighbor = other.GetComponent<MagicReceiver>();
        if (neighbor != null && neighbor != this)
        {
            if (!nearObjects.Contains(neighbor)) nearObjects.Add(neighbor);
            if (!neighbor.nearObjects.Contains(this)) neighbor.nearObjects.Add(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        MagicReceiver neighbor = other.GetComponent<MagicReceiver>();
        if (neighbor != null)
        {
            nearObjects.Remove(neighbor);
            neighbor.nearObjects.Remove(this);
        }
    }
}
