using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuView : BaseView
{
    [Header("UI References")]
    [SerializeField]private Button resumeButton;
    [SerializeField]private Button settingsButton;
    [SerializeField]private Button mainMenuButton;

    public event Action OnResumeClicked;
    public event Action OnSettingsClicked;
    public event Action OnMainMenuClicked;

    protected override void Awake()
    {
        base.Awake();
        resumeButton.onClick.AddListener(() => OnResumeClicked?.Invoke());
        settingsButton.onClick.AddListener(() => OnSettingsClicked?.Invoke());
        mainMenuButton.onClick.AddListener(() => OnMainMenuClicked?.Invoke());

    }
}
