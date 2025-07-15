using UnityEngine;

public class GameOverController : BaseController
{
    [SerializeField] private GameOverView gameOverView;

    private void OnEnable()
    {
        gameOverView.OnRestartClicked += HandleRestart;
        //gameOverView.OnMainMenuClicked += HandleMainMenu;
    }

    private void OnDisable()
    {
        gameOverView.OnRestartClicked -= HandleRestart;
        //gameOverView.OnMainMenuClicked -= HandleMainMenu;
    }

    private void HandleRestart()
    {
        // Le decimos al GameManager que cargue la escena de juego.
        // El GameManager, en su m�todo OnSceneLoaded, se encargar� de
        // poner al jugador en el �ltimo checkpoint.
        GameManager.Instance.LoadScene(SceneType.TestGame);
    }

    /*private void HandleMainMenu()
    {
        GameManager.Instance.LoadScene(SceneType.MainMenuScene);
    }*/
}