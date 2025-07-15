using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;
    private Pickable current;
    public void PickUp(Pickable obj)
    {
        current = obj;
        obj.AttachTo(carryPoint);
    }
    public Pickable Drop()
    {
        Pickable dropped = current;
        current = null;
        return dropped;
    }
    public bool IsCarrying() => current != null;
    public void DropOnDeath()
    {
        if (current != null)
        {
            current.ResetToOriginal();
            current = null;
        }
    }
}
