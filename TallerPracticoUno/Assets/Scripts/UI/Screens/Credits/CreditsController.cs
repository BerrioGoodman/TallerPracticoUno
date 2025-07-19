using UnityEngine;

public class CreditsController : BaseController
{
    [SerializeField] private CreditsView creditsView;

    private void OnEnable()
    {
        creditsView.OnBackClicked += HandleBack;
    }

    private void OnDisable()
    {
        creditsView.OnBackClicked -= HandleBack;
    }

    private void HandleBack()
    {
        GameManager.Instance.RestartGame();
        GameManager.Instance.LoadScene(SceneType.MainMenuScene);
    }
}
