using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SettingsData_SO", menuName = "Scriptable Objects/Settings Data")]
public class SettingsData_SO : ScriptableObject
{
    [Header("Audio")] 
    [SerializeField] private AudioMixer mainMixer;
    
    //Tipos de volumenes
    public float masterVolume = 1f;
    public float sfxVolume = 1f;
    public float musicVolume = 1f;

    //Guardar la configuracion del volumen aunque el juego se cierre
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    //Aplica las configuraciones hechas en el volumen
    public void LoadSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
   
    }

    public void ApplySettings() 
    {
        SetVolume("MasterVolume", masterVolume);
        SetVolume("SFXVolume", sfxVolume);
        SetVolume("MusicVolume", musicVolume);
    }

    //cambiamos el tipo de volumen
    public void SetVolume(string parameterName, float linearValue)
    {
        if(parameterName == "MasterVolume") masterVolume = linearValue;
        if(parameterName == "SFXVolume") sfxVolume = linearValue;
        if(parameterName == "MusicVolume") musicVolume = linearValue;
        
        //usamos log porque hay que traducir el 0 a decibelios
        float dbValue = linearValue > 0.001f ? Mathf.Log10(linearValue) * 20f : -80f;
        mainMixer.SetFloat(parameterName, dbValue);
    }
}
