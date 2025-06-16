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

    public override void Show()
    {
        base.Show();
        if (!isGameLogicPaused)
        {
            Time.timeScale = 0;
            isGameLogicPaused = true;
        }
    }

    public override void Hide()
    {
        base.Hide();
        if (isGameLogicPaused)
        {
            Time.timeScale = 1;
            isGameLogicPaused = false;
        }
    }

    private void HandleMainMenu()
    {
        throw new NotImplementedException();
    }

    private void HandleSettings()
    {
        throw new NotImplementedException();
    }

    private void HandleResume()
    {
        UIManager.Instance.Hide(ScreenType.PauseMenu);
    }
}
