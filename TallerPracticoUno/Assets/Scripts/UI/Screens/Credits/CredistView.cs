using System;
using UnityEngine;
using UnityEngine.UI;

public class CreditsView : BaseView
{
    [SerializeField] private Button backButton;

    public event Action OnBackClicked;

    protected override void Awake()
    {
        base.Awake();
        backButton.onClick.AddListener(() => OnBackClicked?.Invoke());
    }
}