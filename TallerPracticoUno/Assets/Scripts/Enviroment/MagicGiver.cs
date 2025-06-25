using UnityEngine;

public class MagicGiver : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        MagicReceiver receiver = other.GetComponent<MagicReceiver>();
        if (receiver != null && !receiver.hasMagic) receiver.SetMagic(true);
    }
}
