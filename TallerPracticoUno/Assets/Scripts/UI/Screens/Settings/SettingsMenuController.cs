using System;
using UnityEngine;

public class SettingsMenuController : BaseController
{
    [Header("Dependencies")] 
    [SerializeField] private SettingsMenuView settingsMenuView;
    [SerializeField] private SettingsData_SO settingsData;

    private void OnEnable()
    {
        //eventos para el volumen
        settingsMenuView.OnMasterVolumeChanged += HandleMasterVolumeChanged;
        settingsMenuView.OnSfxVolumeChanged += HandleSfxVolumeChanged;
        settingsMenuView.OnMusicVolumeChanged += HandleMusicVolumeChanged;

        //eventos para salir e ir atras
        settingsMenuView.OnBackButtonClicked += HandleBack;
        settingsMenuView.OnQuitButtonClicked += HandleQuit;
        
        InitializeView();
    }

    private void OnDisable()
    {
        //desuscripcion a los eventos
        settingsMenuView.OnMasterVolumeChanged -= HandleMasterVolumeChanged;
        settingsMenuView.OnSfxVolumeChanged -= HandleSfxVolumeChanged;
        settingsMenuView.OnMusicVolumeChanged -= HandleMusicVolumeChanged;

        settingsMenuView.OnBackButtonClicked -= HandleBack;
        settingsMenuView.OnQuitButtonClicked -= HandleQuit;
    }

    //Vista inicial del guardado de los datos
    private void InitializeView()
    {
        settingsData.LoadAndApplySettings();
        
        settingsMenuView.MasterVolume = settingsData.masterVolume;
        settingsMenuView.SfxVolume = settingsData.sfxVolume;
        settingsMenuView.MusicVolume = settingsData.musicVolume;
    }

    private void HandleQuit()
    {
        
    }

    private void HandleBack()
    {
        UIManager.Instance.Hide(ScreenType.SettingsMenu);
        UIManager.Instance.Show<PauseMenuController>(ScreenType.PauseMenu);
        settingsData.SaveSettings();
    }

    private void HandleMusicVolumeChanged(float value)
    {
        settingsData.SetVolume("MusicVolume", value);
    }

    private void HandleSfxVolumeChanged(float value)
    {
        settingsData.SetVolume("SFXVolume", value);
    }

    private void HandleMasterVolumeChanged(float value)
    {
        settingsData.SetVolume("MasterVolume", value);
    }
}
