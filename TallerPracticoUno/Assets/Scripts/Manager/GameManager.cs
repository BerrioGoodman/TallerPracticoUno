using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Dependecies")]
    [Tooltip("Arrastra aquí tu asset InputReader")]
    [SerializeField] private InputReader inputReader;
    [Header("Data Dependencies")] 
    [SerializeField] private FlameStats_SO flameStats;
    [SerializeField] private SettingsData_SO settingsData;

    private bool isGamePaused = false;
    private int deliveredCount = 0;
    private const int totalToDeliver = 5;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return; 
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnEnable()
    {
        inputReader.PauseEvent += TogglePause;
    }

    private void OnDisable()
    {
        inputReader.PauseEvent -= TogglePause;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SceneType.Game.ToString())
        {
            inputReader.SetGameplay();
            AudioManager.Instance.PlayMusic("Ambience");
            UIManager.Instance.Show<FlameHUDController>(ScreenType.FlameHUD);
        }
        else if (scene.name == SceneType.GameOver.ToString())
        {
            inputReader.SetUI();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (scene.name == SceneType.Credits.ToString()) 
        {
            inputReader.SetUI();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (scene.name == SceneType.MainMenuScene.ToString())
        {
            inputReader.SetUI();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            inputReader.SetUI();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void TogglePause()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0;
            inputReader.SetUI();
            UIManager.Instance.Show<PauseMenuController>(ScreenType.PauseMenu);
        }
        else 
        {
            UIManager.Instance.Hide(ScreenType.SettingsMenu);
            UIManager.Instance.Hide(ScreenType.PauseMenu);
            Time.timeScale = 1;
            inputReader.SetGameplay();
        }
    }

    public void LoadScene(SceneType loadScene) 
    {
        SceneManager.LoadScene(loadScene.ToString());
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void RestartGame()
    {
        Debug.Log("Reiniciando el juego...");
        ResetGameState();
        LoadScene(SceneType.Game);
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Volviendo al Menú Principal...");
        ResetGameState();
        LoadScene(SceneType.MainMenuScene);
    }

    private void ResetGameState()
    {
        flameStats.Reset();

        deliveredCount = 0;
    }
    public void RegisterDelivery()
    {
        deliveredCount++;
        Debug.Log($"Objects delivered: {deliveredCount} / {totalToDeliver}");
        /*if (deliveredCount >= totalToDeliver)
        {
            Debug.Log("To be continued...");
            StartCoroutine(WaitEndGame());
        }*/
        if (deliveredCount >= totalToDeliver)
        {
            Debug.Log("All deliveries complete. Activating final portal...");
            CamerasManager.Instance.ActivateFinalPortal();//Activate panoramic scene
        }
    }
    //Hacer coorutina para el cambio de escena
    private IEnumerator WaitEndGame()
    {
        yield return new WaitForSeconds(5f);
        LoadScene(SceneType.Credits);
    }
}
