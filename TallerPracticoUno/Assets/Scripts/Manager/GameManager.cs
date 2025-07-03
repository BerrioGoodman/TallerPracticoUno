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
        inputReader.PauseEvent += SetPause;
        inputReader.ResumeEvent += SetResume;
    }

    private void OnDisable()
    {
        inputReader.PauseEvent -= SetPause;
        inputReader.ResumeEvent -= SetResume;
    }

    private void SetPause()
    {
        isGamePaused = true;

        UIManager.Instance.Show<PauseMenuController>(ScreenType.PauseMenu);
    }

    private void SetResume()
    { 
        isGamePaused = false;
        
        UIManager.Instance.Hide(ScreenType.PauseMenu);
    }
}
