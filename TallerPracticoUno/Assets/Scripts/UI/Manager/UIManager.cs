using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } //autoproperty

    [Serializable] //Registramos pantallas en el inspector de Unity
    public class ScreenEntry
    {
        public ScreenType screenType;
        public BaseController screenPrefab;
    }

    [Header("UI Screen Prefabs")] 
    [SerializeField]
    private List<ScreenEntry> screenEntries;
    
    private Dictionary<ScreenType, BaseController> screenPrefabs = new();//Diccionario de prefabs
    private Dictionary<ScreenType, BaseController> screenInstances = new(); //Diccionario para instancias

    private void Awake()
    {
        //singleton
        if (Instance != null)
        {
            Destroy(gameObject);
            return; 
        }
        Instance = this;

        //Llenado de diccionario para busquedas rápidas
        foreach (var entry in screenEntries)
        {
            if (entry.screenPrefab != null)
            {
                screenPrefabs.Add(entry.screenType, entry.screenPrefab);
            }
        }
    }

    //Generico para cualquier pantalla
    public T Show<T>(ScreenType screenType) where T : BaseController
    {
        //Buscamos si tenemos una instancia de la pantalla
        if (screenInstances.TryGetValue(screenType, out BaseController instance))
        {
            instance.Show();
            return instance as T;
        }

        if (screenPrefabs.TryGetValue(screenType, out BaseController prefab)) //si no esta instanciada buscamos el prefab correspondiente
        {
            BaseController newInstance = Instantiate(prefab, transform); //Creamos una instancia del Prefab
            
            screenInstances[screenType] = newInstance;//Lo guardamos en el diccionario de instancias
            
            newInstance.Show();
            
            return newInstance as T;
        }
        
        Debug.LogError($"UIManager: No se encontró un prefab para el screenType: {screenType}");
        return null;
    }

    //Ocultar cualquier pantalla
    public void Hide(ScreenType screenType)
    {
        if (screenInstances.TryGetValue(screenType, out BaseController instance))
        {
            instance.Hide();
        }
    }
}
