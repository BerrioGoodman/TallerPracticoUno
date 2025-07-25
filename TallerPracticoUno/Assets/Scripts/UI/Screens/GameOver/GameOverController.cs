using UnityEngine;

public class GameOverController : BaseController
{
    [SerializeField] private GameOverView gameOverView;

    private void OnEnable()
    {
        gameOverView.OnRestartClicked += HandleRestart;
        gameOverView.OnMainMenuClicked += HandleMainMenu;
    }

    private void OnDisable()
    {
        gameOverView.OnRestartClicked -= HandleRestart;
        gameOverView.OnMainMenuClicked -= HandleMainMenu;
    }

    private void HandleRestart()
    {
        GameManager.Instance.RestartGame();
        GameManager.Instance.LoadScene(SceneType.Game);
    }

    private void HandleMainMenu()
    {
        GameManager.Instance.RestartGame();
        GameManager.Instance.LoadScene(SceneType.MainMenuScene);
    }
}