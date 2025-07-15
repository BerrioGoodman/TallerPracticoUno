using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : BaseView
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button creditsButton;

    public event Action OnStartButtonClicked; 
    public event Action OnExitButtonClicked;
    public event Action OnCreditsButtonClicked;

    protected override void Awake()
    {
        base.Awake();
        startButton.onClick.AddListener(() => OnStartButtonClicked?.Invoke());
        exitButton.onClick.AddListener(() => OnExitButtonClicked?.Invoke());
        creditsButton.onClick.AddListener(() => OnCreditsButtonClicked?.Invoke());
    }
}
