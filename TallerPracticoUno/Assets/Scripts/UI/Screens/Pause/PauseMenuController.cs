using System;
using UnityEngine;

public class PauseMenuController : BaseController
{
    [Header("Dependencies")]
    [SerializeField] private PauseMenuView pauseMenuView;
    
    private bool isGameLogicPaused = false;

    private void OnEnable()
    {
        pauseMenuView.OnResumeClicked += HandleResume;
        pauseMenuView.OnSettingsClicked += HandleSettings;
        pauseMenuView.OnMainMenuClicked += HandleMainMenu;
    }

    private void OnDisable()
    {
        pauseMenuView.OnResumeClicked -= HandleResume;
        pauseMenuView.OnSettingsClicked -= HandleSettings;
        pauseMenuView.OnMainMenuClicked -= HandleMainMenu;
    }

    private void HandleMainMenu()
    {
        GameManager.Instance.TogglePause();
        GameManager.Instance.ReturnToMainMenu();
    }

    private void HandleSettings()
    {
        UIManager.Instance.Hide(ScreenType.PauseMenu);
        UIManager.Instance.Show<SettingsMenuController>(ScreenType.SettingsMenu);
    }

    private void HandleResume()
    {
        GameManager.Instance.TogglePause();
    }

    
}
