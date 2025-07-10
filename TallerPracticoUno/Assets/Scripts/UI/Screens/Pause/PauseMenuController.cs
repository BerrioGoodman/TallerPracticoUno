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
            Debug.Log("Hola hide");
        }
    }

    private void HandleMainMenu()
    {
        throw new NotImplementedException();
    }

    private void HandleSettings()
    {
        UIManager.Instance.Hide(ScreenType.PauseMenu);
        UIManager.Instance.Show<SettingsMenuController>(ScreenType.SettingsMenu);
    }

    private void HandleResume()
    {
        UIManager.Instance.Hide(ScreenType.PauseMenu);
    }
}
