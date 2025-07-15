using System;
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
        if (scene.name == SceneType.ReadyGame.ToString())
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
        LoadScene(SceneType.ReadyGame);
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


    private void LoadPlayerPosition() 
    {
        string lastCheckpointID = SaveManager.Instance.GetLastCheckpointID();

        GameObject checkpointObject = GameObject.Find(lastCheckpointID);

        if (checkpointObject != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {

                player.transform.position = checkpointObject.transform.position;
                Debug.Log($"Jugador movido al checkpoint: {lastCheckpointID}");
            }
        }
        else
        {
            Debug.LogError($"No se pudo encontrar el GameObject del checkpoint con ID: {lastCheckpointID}");
        }
    }
    public void RegisterDelivery()
    {
        deliveredCount++;
        Debug.Log($"Objects delivered: {deliveredCount} / {totalToDeliver}");
        if (deliveredCount >= totalToDeliver)
        {
            Debug.Log("To be continued...");
        }
    }
}
