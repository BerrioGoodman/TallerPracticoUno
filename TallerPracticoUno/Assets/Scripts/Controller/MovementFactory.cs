using UnityEngine;

public class MovementFactory
{
    public static IMovementSystem GetMovement(
        MovementType type,
        LandedMovement landed,
        GlideMovement glide,
        UnderwaterMovement underwater)
    {
        if (type == MovementType.Landed)
        {
            return landed;
        }
        else if (type == MovementType.Glide)
        {
            return glide;
        }
        else if (type == MovementType.Underwater)
        {
            return underwater;
        }
        else
        {
            return landed; // Fallback por seguridad
        }
    }
}
