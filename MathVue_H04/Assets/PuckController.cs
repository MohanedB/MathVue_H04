using UnityEngine;

public class PuckController : MonoBehaviour
{
    public Vector3 velocity;
    public float radius = 0.5f;
    public float constantY = 0.1f;
    public FieldLimits fieldLimits;

    void Update()
    {
        // 1) Intégration manuelle : P_new = P + V * dt
        Vector3 newPos = transform.position + velocity * Time.deltaTime;
        newPos.y = constantY;

        // 2) Collision avec le mur gauche
        if ((newPos.x - radius) < fieldLimits.xMin)
        {
            // Corrige la position
            newPos.x = fieldLimits.xMin + radius;


            // La normale du mur gauche pointe vers la droite : (1,0,0)
            Vector3 wallNormal = new Vector3(1, 0, 0);
            velocity = MathUtils.ReflectVelocity(velocity, wallNormal);
        }

        // 3) Collision avec le mur droit
        if ((newPos.x + radius) > fieldLimits.xMax)
        {
            newPos.x = fieldLimits.xMax - radius;
            // Mur droit -> normale = (-1, 0, 0)
            Vector3 wallNormal = new Vector3(-1, 0, 0);
            velocity = MathUtils.ReflectVelocity(velocity, wallNormal);
        }

        // 4) Collision mur en bas
        if ((newPos.z - radius) < fieldLimits.zMin)
        {
            newPos.z = fieldLimits.zMin + radius;
            // Mur bas -> normale = (0, 0, 1)
            Vector3 wallNormal = new Vector3(0, 0, 1);
            velocity = MathUtils.ReflectVelocity(velocity, wallNormal);
        }

        // 5) Collision mur en haut
        if ((newPos.z + radius) > fieldLimits.zMax)
        {
            newPos.z = fieldLimits.zMax - radius;
            // Mur haut -> normale = (0, 0, -1)
            Vector3 wallNormal = new Vector3(0, 0, -1);
            velocity = MathUtils.ReflectVelocity(velocity, wallNormal);
        }

        // 6) Mise à jour de la position
        transform.position = newPos;
    }
}
