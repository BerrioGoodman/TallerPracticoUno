using UnityEngine;

public class GiveMagic : MonoBehaviour
{
    [SerializeField] private float radius;
    void Update()
    {
        GiveMagicToObjects();
    }
    public void GiveMagicToObjects()
    {
        Collider[] near = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider c in near)
        {
            MagicBehavior receiver = c.GetComponent<MagicBehavior>();
            if (receiver != null && !receiver.hasMagic)
            {
                receiver.SetMagic(true);
            }
        }
    }
}
