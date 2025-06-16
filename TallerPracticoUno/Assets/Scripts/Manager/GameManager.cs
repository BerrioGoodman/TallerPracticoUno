using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Dependecies")]
    [Tooltip("Arrastra aquí tu asset InputReader")]
    [SerializeField] private InputReader inputReader;
    
    private bool isGamePaused = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return; 
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UIManager.Instance.Show<FlameHUDController>(ScreenType.FlameHUD);
    }

    private void OnEnable()
    {
        inputReader.PauseEvent += TogglePause;
    }

    private void OnDisable()
    {
        inputReader.PauseEvent -= TogglePause;
    }

    private void TogglePause()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            UIManager.Instance.Show<PauseMenuController>(ScreenType.PauseMenu);
        }
        else
        {
            UIManager.Instance.Hide(ScreenType.PauseMenu);
        }
    }
}
