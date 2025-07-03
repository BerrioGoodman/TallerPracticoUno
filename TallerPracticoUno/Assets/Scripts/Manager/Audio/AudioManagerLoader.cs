using UnityEngine;

public class AudioManagerLoader : MonoBehaviour
{
    public GameObject audioManagerPrefab; // Prefab del AudioManager

    private void Awake()
    {
        if (AudioManager.Instance == null) 
        {
            GameObject audioManager = Instantiate(audioManagerPrefab); // Instanciar el prefab del AudioManager
            DontDestroyOnLoad(audioManager); // No destruir al cargar una nueva escena
        }
    }
}
