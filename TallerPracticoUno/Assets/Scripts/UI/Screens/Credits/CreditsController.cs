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
        // El GameManager es el �nico que sabe c�mo cambiar de escena.
        GameManager.Instance.LoadScene(SceneType.MainMenuScene);
    }
}
