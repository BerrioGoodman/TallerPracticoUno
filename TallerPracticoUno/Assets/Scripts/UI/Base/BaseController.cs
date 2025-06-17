using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    protected CanvasGroup mainCanvasGroup;

    protected virtual void Awake()
    {
        mainCanvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void Show()
    {
        ToggleCanvas(true);
    }

    public virtual void Hide()
    {
        ToggleCanvas(false);
    }

    private void ToggleCanvas(bool visible)
    {
        if (mainCanvasGroup == null) return;
        mainCanvasGroup.alpha = visible ? 1 : 0; //transparencia
        mainCanvasGroup.interactable = visible; //interactuable
        mainCanvasGroup.blocksRaycasts = visible; //bloquea raycasts
    }
}
