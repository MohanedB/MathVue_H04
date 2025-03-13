using UnityEngine;

public class PuckCollisionManager : MonoBehaviour
{
    void Update()
    {
        // On récupère tous les PuckController actifs dans la scène.
        PuckController[] pucks = FindObjectsOfType<PuckController>();

        // Pour chaque paire de poques, on vérifie et résout la collision.
        for (int i = 0; i < pucks.Length; i++)
        {
            for (int j = i + 1; j < pucks.Length; j++)
            {
                CheckAndResolveCollision(pucks[i], pucks[j]);
            }
        }
    }

    void CheckAndResolveCollision(PuckController a, PuckController b)
    {
        // Calcul de la distance entre les centres des poques
        Vector3 delta = b.transform.position - a.transform.position;
        float distance = delta.magnitude;
        float minDistance = a.radius + b.radius;

        // Si la distance est inférieure à la somme des rayons, une collision est détectée.
        if (distance < minDistance)
        {
            // Calcul de la normale de collision (direction entre les centres)
            Vector3 normal = delta.normalized;

            // Calcul de la vitesse relative le long de la normale
            float relativeVelocity = Vector3.Dot(b.velocity - a.velocity, normal);

            // On ne résout la collision que si les poques se rapprochent l'une de l'autre.
            if (relativeVelocity < 0)
            {
                // Projection des vitesses sur la normale pour obtenir les composantes normales
                float vA = Vector3.Dot(a.velocity, normal);
                float vB = Vector3.Dot(b.velocity, normal);

                // Pour des masses égales, on échange les composantes normales
                float newVA = vB;
                float newVB = vA;

                // La composante tangentielle (perpendiculaire à la normale) reste inchangée
                Vector3 tangentA = a.velocity - normal * vA;
                Vector3 tangentB = b.velocity - normal * vB;

                // Mise à jour des vitesses après collision
                a.velocity = tangentA + normal * newVA;
                b.velocity = tangentB + normal * newVB;
            }

            // Correction de la position pour éviter que les poques ne se superposent
            float overlap = minDistance - distance;
            a.transform.position -= normal * (overlap / 2f);
            b.transform.position += normal * (overlap / 2f);
        }
    }
}
