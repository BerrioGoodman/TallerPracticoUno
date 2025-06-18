using System.Collections;
using UnityEngine;

public class MagicBehavior : MonoBehaviour
{
    [Header("Settings")]
    public bool hasMagic;
    [SerializeField] private float transition;
    [SerializeField] private float radius;
    [SerializeField] private float delay;
    [Header("InitialState")]
    [SerializeField] private Color darkColor = Color.black;
    [SerializeField] private Color darkEmission = Color.black;
    private Renderer rend;
    private Material material;
    private Color originalColor;
    private Color originalEmission;
    private Coroutine MagicRoutine;
    private void Awake()
    {
        rend = GetComponent<Renderer>();
        material = rend.material;
        //Save original settings
        originalColor = material.color;
        originalEmission = material.GetColor("_EmissionColor");
        //We initiate the game
        hasMagic = true;
        SetMagic(false, true);
    }
    public void SetMagic(bool magic, bool instant = false)
    {
        if (hasMagic == magic) return;
        hasMagic = magic;
        if (MagicRoutine != null) StopCoroutine(MagicRoutine);
        MagicRoutine = StartCoroutine(LerpToState(magic, instant));
    }
    private IEnumerator LerpToState(bool toMagic, bool instant)
    {
        float t = 0;
        Color startColor = material.color;
        Color startEmission = material.GetColor("_EmissionColor");
        Color targetColor = toMagic ? originalColor : darkColor;
        Color targetEmission = toMagic ? originalEmission : darkEmission;
        if (instant)
        {
            material.color = targetColor;
            material.SetColor("_EmissionColor", targetEmission);
            if (toMagic) StartCoroutine(PropagateMagic());
            yield break;
        }
        while (t < 1)
        {
            t += Time.deltaTime / transition;
            material.color = Color.Lerp(startColor, targetColor, t);
            material.SetColor("_EmissionColor", Color.Lerp(startEmission, targetEmission, t));
            yield return null;
        }
        material.color = targetColor;
        material.SetColor("_EmissionColor", targetEmission);
        if (toMagic) StartCoroutine(PropagateMagic());
    }
    private IEnumerator PropagateMagic()
    {
        yield return new WaitForSeconds(delay);
        Collider[] next = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider col in next)
        {
            MagicBehavior neighbor = col.GetComponent<MagicBehavior>();
            if (neighbor != null && !neighbor.hasMagic)
            {
                neighbor.SetMagic(true);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
