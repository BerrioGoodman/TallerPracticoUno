using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    [Header("Cameras")]
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineCamera cinematicCamera;
    [Header("Player")]
    [SerializeField] private PlayerController player;
    [Header("Focus Duration")]
    [SerializeField] private float focusDuration = 3f;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        playerCamera.enabled = true;
        cinematicCamera.enabled = false;
    }
    public void FocusOnCinematicPoint()
    {
        StartCoroutine(FocusCoroutine());
    }
    private IEnumerator FocusCoroutine()
    {
        if (player != null) player.EnableControl(false);//Stop inputs
        playerCamera.enabled = false;
        cinematicCamera.enabled = true;
        yield return new WaitForSeconds(focusDuration);
        cinematicCamera.enabled = false;
        playerCamera.enabled = true;
        if (player != null) player.EnableControl(true);//Inputs back
    }
}
