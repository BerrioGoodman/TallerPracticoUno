using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; //instancia del AudioManager

    [System.Serializable]
    public class Sound
    {
        public string name; //nombre del sonido
        public AudioClip clip; //clip de audio
    }

    [Header("Audio Sources")]
    public AudioSource musicSource; //musica de fondo
    public AudioSource sfxSource; //sonidos de efectos

    [Header("Audio clips")]
    public Sound[] musicClips; //lista de musica
    public Sound[] sfxClips; //lista de efectos de sonido

    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixerGroup musicMixerGroup; //grupo de mezcla de musica
    [SerializeField] private AudioMixerGroup sfxMixerGroup; //grupo de mezcla de efectos de sonido

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; //asignar la instancia
            DontDestroyOnLoad(gameObject); //no destruir al cargar una nueva escena
        }
        else
        {
            Destroy(gameObject); //destruir si ya existe una instancia
        }

        musicSource.outputAudioMixerGroup = musicMixerGroup; //asignar grupo de mezcla a la musica
        sfxSource.outputAudioMixerGroup = sfxMixerGroup; //asignar grupo de mezcla a los efectos de sonido
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicClips, sound => sound.name == name);//buscar el sonido por nombre

        if (s == null) 
        {
            Debug.LogWarning("AudioManager: Sonido de música no encontrado: " + name);
            return;   
        }

        musicSource.clip = s.clip; //asignar el clip de audio
        musicSource.Play(); //reproducir la musica
    }

    public void PlaySFX(string name) 
    {
        Sound s = Array.Find(sfxClips, sound => sound.name == name);

        if (s == null) 
        {
            Debug.LogWarning("AudioManager: SFX no encontrado: " + name);
            return;
        }

        sfxSource.PlayOneShot(s.clip); //reproducir el efecto de sonido
    }
}
