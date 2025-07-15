using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverView : BaseView
{
    [SerializeField] private Button restartButton;
    //[SerializeField] private Button mainMenuButton;

    public event Action OnRestartClicked;
    public event Action OnMainMenuClicked;

    protected override void Awake()
    {
        base.Awake();
        restartButton.onClick.AddListener(() => OnRestartClicked?.Invoke());
        //mainMenuButton.onClick.AddListener(() => OnMainMenuClicked?.Invoke());
    }
}
