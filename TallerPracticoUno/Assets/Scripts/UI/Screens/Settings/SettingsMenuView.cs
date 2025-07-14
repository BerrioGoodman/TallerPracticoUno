using UnityEngine;
using UnityEngine.UI;
using System;

public class SettingsMenuView : BaseView
{
    [Header("UI References")] 
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider sfxVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Button backButton;
    //[SerializeField] private Button quitButton;

    //eventos necesarios
    public event Action<float> OnMasterVolumeChanged;
    public event Action<float> OnSfxVolumeChanged;
    public event Action<float> OnMusicVolumeChanged;
    public event Action OnBackButtonClicked;
    //public event Action OnQuitButtonClicked;
    
    //Accesores
    public float MasterVolume
    {
        get => masterVolume.value;
        set => masterVolume.value = value;
    }
    
    public float SfxVolume
    {
        get => sfxVolume.value;
        set => sfxVolume.value = value;
    }

    public float MusicVolume
    {
        get => musicVolume.value;
        set => musicVolume.value = value;
    }

    //Conexion de eventos a los elementos fisicos
    protected override void Awake()
    {
        base.Awake();
        backButton.onClick.AddListener(() => OnBackButtonClicked?.Invoke());
        //quitButton.onClick.AddListener(() => OnQuitButtonClicked?.Invoke());
        
        masterVolume.onValueChanged.AddListener(value => OnMasterVolumeChanged?.Invoke(value));
        sfxVolume.onValueChanged.AddListener(value => OnSfxVolumeChanged?.Invoke(value));
        musicVolume.onValueChanged.AddListener(value => OnMusicVolumeChanged?.Invoke(value));
    }
}
