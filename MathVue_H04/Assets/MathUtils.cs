using UnityEngine;

public static class MathUtils
{
    /// <summary>
    /// Produit scalaire de deux vecteurs 3D.
    /// </summary>
    public static float GetDotProduct(Vector3 A, Vector3 B)
    {
        return A.x * B.x + A.y * B.y + A.z * B.z;
    }

    /// <summary>
    /// Soustraction vectorielle (B - A).
    /// </summary>
    public static Vector3 GetVector(Vector3 A, Vector3 B)
    {
        return new Vector3(B.x - A.x, B.y - A.y, B.z - A.z);
    }

    /// <summary>
    /// Norme (longueur) d'un vecteur 3D : sqrt(x² + y² + z²).
    /// </summary>
    public static float GetNorm(Vector3 AB)
    {
        return Mathf.Sqrt(AB.x * AB.x + AB.y * AB.y + AB.z * AB.z);
    }

    /// <summary>
    /// Normalisation d'un vecteur 3D : V / ||V||.
    /// </summary>
    public static Vector3 GetNormalVector(Vector3 AB)
    {
        float length = GetNorm(AB);
        if (length < 1e-6f)
        {
            // Pour éviter la division par zéro
            return Vector3.zero;
        }
        return new Vector3(AB.x / length, AB.y / length, AB.z / length);
    }

    /// <summary>
    /// Réflexion d'une vitesse V par rapport à une normale N :
    /// V' = V - 2 * (V·N) * N
    /// </summary>
    public static Vector3 ReflectVelocity(Vector3 velocity, Vector3 normal)
    {
        float dot = GetDotProduct(velocity, normal);
        // V' = V - 2 * (V·N) * N
        return velocity - 2f * dot * normal;
    }
}
