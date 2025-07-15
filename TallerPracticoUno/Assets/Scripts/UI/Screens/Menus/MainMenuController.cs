using UnityEngine;

public class MainMenuController : BaseController
{
    [SerializeField] private MainMenuView mainMenuView;

    private void OnEnable()
    {
        mainMenuView.OnStartButtonClicked += HandleStart;
        mainMenuView.OnSettingsButtonClicked += HandleSettings;
        mainMenuView.OnCreditsButtonClicked += HandleCredits;
        mainMenuView.OnExitButtonClicked += HandleExit;
    }

    private void OnDisable()
    {
        mainMenuView.OnStartButtonClicked -= HandleStart;
        mainMenuView.OnSettingsButtonClicked -= HandleSettings;
        mainMenuView.OnCreditsButtonClicked -= HandleCredits;
        mainMenuView.OnExitButtonClicked -= HandleExit;
    }

    private void HandleStart()
    {
        GameManager.Instance.LoadScene(SceneType.TestGame);
    }

    private void HandleSettings()
    {
        // Usamos el UIManager para mostrar el menú de opciones que ya creamos
        UIManager.Instance.Show<SettingsMenuController>(ScreenType.SettingsMenu);
    }

    private void HandleCredits()
    {
        GameManager.Instance.LoadScene(SceneType.Credits);
    }

    private void HandleExit()
    {
        GameManager.Instance.QuitGame();
    }
}