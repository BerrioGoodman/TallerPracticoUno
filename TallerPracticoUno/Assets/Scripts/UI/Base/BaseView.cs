using UnityEngine;

public abstract class BaseView : MonoBehaviour
{
    public CanvasGroup mainCanvasGroup { get; private set; } //autoproperty

    protected virtual void Awake()
    {
        mainCanvasGroup = GetComponent<CanvasGroup>();
        if (mainCanvasGroup == null)
        {
            mainCanvasGroup = gameObject.AddComponent<CanvasGroup>();//si no lo encuentra, lo crea
        }
    }
}
