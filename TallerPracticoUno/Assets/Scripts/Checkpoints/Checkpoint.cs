using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("un ID unico para este checkpoint")]
    [SerializeField] private string checkpointID;

    private bool isActive = false;

    private void OnTriggerEnter(Collider other)
    {
        //if (isActive) return;

        if (other.CompareTag("Player"))
        {
            //isActive = true;
            SaveManager.Instance.UpdateLastCheckpoint(checkpointID);
            Debug.Log($"Checkpoint {checkpointID} activado.");
        }
    }
}
