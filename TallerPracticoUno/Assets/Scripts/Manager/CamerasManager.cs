using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
public class CamerasManager : MonoBehaviour
{
    public static CamerasManager Instance;
    [Header("Cameras")]
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineCamera cinematicCamera;
    [Header("Player")]
    [SerializeField] private PlayerController player;
    [Header("Zoom Settings")]
    [SerializeField] private float zoomDistance = 10f;
    [SerializeField] private float zoomDuration = 2f;
    [Header("Points of Interest")]
    [SerializeField] private Transform originalCameraPosition;
    [SerializeField] private Transform pointOne;
    [SerializeField] private Transform pointTwo;
    [SerializeField] private Transform pointThree;
    [SerializeField] private Transform pointFour;
    [SerializeField] private Transform pointFive;
    [Header("Final Panoramic Path")]
    [SerializeField] private Transform[] finalCameraPath;
    [SerializeField] private float panoramicSpeed;
    [Header("Final Portal")]
    [SerializeField] private GameObject finalPortal;
    private Transform[] deliveryPoints;
    [Header("Focus Point for Curve")]
    [SerializeField] private Transform panoramicFocusPoint;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        playerCamera.enabled = false;
        cinematicCamera.enabled = true;
        deliveryPoints = new Transform[] { pointTwo, pointThree, pointFour, pointFive };
        if (finalPortal != null)
            finalPortal.SetActive(false); // asegúrate de que arranque desactivado
        StartCoroutine(StartGameSequence());
    }
    private IEnumerator StartGameSequence()
    {
        yield return StartCoroutine(ZoomToPoint(pointOne));
        yield return StartCoroutine(ZoomToPosition(originalCameraPosition.position, originalCameraPosition.rotation));
        SwitchToPlayerCamera();
    }
    public void SwitchToCinematicCamera()
    {
        playerCamera.enabled = false;
        cinematicCamera.enabled = true;
    }
    public void SwitchToPlayerCamera()
    {
        cinematicCamera.enabled = false;
        playerCamera.enabled = true;
    }
    public void FocusOnDeliveryPoint(int index)
    {
        if (index >= 0 && index < deliveryPoints.Length)
        {
            StartCoroutine(FocusOnPointSequence(deliveryPoints[index]));
        }
    }
    private IEnumerator FocusOnPointSequence(Transform point)
    {
        //ChatGPT Coroutine
        if (player != null)
            player.EnableControl(false);

        // 1. Activar cámara cinemática
        SwitchToCinematicCamera();

        // 2. Mover de forma suave a la posición original
        yield return StartCoroutine(ZoomToPosition(originalCameraPosition.position, originalCameraPosition.rotation));

        // 3. Espera para que se note visualmente el cambio
        yield return new WaitForSeconds(1f);

        // 4. Zoom al punto (pointTwo, pointThree, etc.)
        yield return StartCoroutine(ZoomToPoint(point));

        // 5. Espera
        yield return new WaitForSeconds(2f);

        // 6. Volver al punto original con interpolación
        yield return StartCoroutine(ZoomToPosition(originalCameraPosition.position, originalCameraPosition.rotation));

        // 7. Volver a cámara del jugador
        SwitchToPlayerCamera();

        if (player != null)
            player.EnableControl(true);
    }
    private IEnumerator ZoomToPoint(Transform point)
    {
        Vector3 direction = (cinematicCamera.transform.position - point.position).normalized;
        Vector3 targetPos = point.position + direction * zoomDistance;
        Quaternion targetRot = Quaternion.LookRotation(point.position - targetPos);

        return ZoomToPosition(targetPos, targetRot);
    }
    private IEnumerator ZoomToPosition(Vector3 position, Quaternion rotation)
    {
        float elapsed = 0f;
        Vector3 startPos = cinematicCamera.transform.position;
        Quaternion startRot = cinematicCamera.transform.rotation;

        while (elapsed < zoomDuration)
        {
            float t = elapsed / zoomDuration;
            cinematicCamera.transform.position = Vector3.Lerp(startPos, position, t);
            cinematicCamera.transform.rotation = Quaternion.Slerp(startRot, rotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cinematicCamera.transform.position = position;
        cinematicCamera.transform.rotation = rotation;
    }
    public void ActivateFinalPortal()
    {
        if (finalPortal != null)
            finalPortal.SetActive(true);
    }
    public void StartFinalPanoramic()
    {
        StartCoroutine(FinalPanoramicSequence());
    }
    private IEnumerator FinalPanoramicSequence()
    {
        if (player != null) player.EnableControl(false);
        SwitchToCinematicCamera();
        for (int i = 0; i < finalCameraPath.Length - 1; i++)
        {
            Transform start = finalCameraPath[i];
            Transform end = finalCameraPath[i + 1];

            if (panoramicFocusPoint != null)
            {
                float distance = Vector3.Distance(start.position, end.position);
                float duration = distance / panoramicSpeed;
                yield return StartCoroutine(BezierMove(start, panoramicFocusPoint, end, duration));
            }
            else
            {
                yield return StartCoroutine(ZoomToPosition(end.position, end.rotation));
            }

            yield return new WaitForSeconds(2f);
        }
        yield return new WaitForSeconds(2f);
        GameManager.Instance.LoadScene(SceneType.Credits);
    }
    private IEnumerator BezierMove(Transform start, Transform control, Transform end, float duration)
    {
        float elapsed = 0f;
        Vector3 p0 = start.position;
        Vector3 p1 = control.position;
        Vector3 p2 = end.position;
        Quaternion startRot = cinematicCamera.transform.rotation;
        Quaternion endRot = Quaternion.LookRotation(end.position - p2); // Mirar hacia el destino
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Vector3 bezierPos = Mathf.Pow(1 - t, 2) * p0 +
                                2 * (1 - t) * t * p1 +
                                Mathf.Pow(t, 2) * p2;
            cinematicCamera.transform.position = bezierPos;
            cinematicCamera.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cinematicCamera.transform.position = p2;
        cinematicCamera.transform.rotation = endRot;
    }
}
