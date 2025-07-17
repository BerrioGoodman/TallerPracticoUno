using UnityEngine;

public class MainMenuController : BaseController
{
    [SerializeField] private MainMenuView mainMenuView;

    private void OnEnable()
    {
        mainMenuView.OnStartButtonClicked += HandleStart;
        mainMenuView.OnCreditsButtonClicked += HandleCredits;
        mainMenuView.OnExitButtonClicked += HandleExit;
    }

    private void OnDisable()
    {
        mainMenuView.OnStartButtonClicked -= HandleStart;
        mainMenuView.OnCreditsButtonClicked -= HandleCredits;
        mainMenuView.OnExitButtonClicked -= HandleExit;
    }

    private void HandleStart()
    {
        GameManager.Instance.LoadScene(SceneType.GamePab);
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