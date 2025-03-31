using UnityEngine;
using UnityEngine.U2D;

public class CameraAnimation : MonoBehaviour
{
    // Affectez vos points de contr�le dans l'Inspector (par exemple, 12 points)
    public Transform[] controlPoints;
    // Vitesse de progression sur chaque segment
    public float speed = 0.2f;

    // Mode d'interpolation : 0 = lin�aire, 1 = B�zier cubique, 2 = Catmull Rom
    int currentMode = 0;
    // Quel segment est en utilisation
    int currentSegment = 0;
    float t = 0f;

    // Angle d'inclinaison vers le bas (en degr�s)
    public float pitchAngle = 30f;

    void Update()
    {
     
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentMode++; 
            if (currentMode >= 3)
            {
                currentMode = 0; 
            }

            currentSegment = 0;
            t = 0f;

            Debug.Log("Nouveau mode: " + currentMode); 
        }

        Vector3 newPos = transform.position;
        Vector3 tangent = Vector3.forward; 

        switch (currentMode)
        {
            case 0:
                {
                    // --- Mode Lin�aire ---
                    // V�rifie qu'on a au moins 2 points
                    if (controlPoints.Length < 2)
                    {
                        break;
                    }

                    // Nombre de segments (chaque segment relie deux points)
                    int linearSegments = controlPoints.Length - 1;

                    //s'assure que le nombre de segment reste entre 0 et 10 vue qu,il y a 11 segement total donc 0-10
                    currentSegment = Mathf.Clamp(currentSegment, 0, linearSegments - 1);

                    // Points de d�part et d'arriv�e du segment
                    Vector3 p0 = controlPoints[currentSegment].position;
                    Vector3 p1 = controlPoints[currentSegment + 1].position;

                    // Interpolation lin�aire entre p0 et p1 en autre ot ce qui fait avancer la camera entre 2 points
                    //newPos = Vector3.Lerp(p0, p1, t);
                    newPos = (1f - t) * p0 + t * p1;

                    // La direction que la cam regarde Ce qui fait flicker la cam quand on change de segement
                    //tangent = (p1 - p0).normalized;

                    break;
                }

            case 1:
                {
                    // --- Mode B�zier Cubique ---
                    // Pour B�zier cubique, on prend 4 points pour d�crire un segment
                    int bezierSegments = (controlPoints.Length - 1) / 3;
                    //Permet de determiner dans quel segment on est
                    int bezierIndex = currentSegment * 3;

                    // V�rifie qu'on a assez de points pour ce segment
                    if (controlPoints.Length < bezierIndex + 4)
                    {
                        break;
                    }

                    // R�cup�re les 4 points pour la courbe de B�zier
                    Vector3 p0 = controlPoints[bezierIndex].position;
                    Vector3 p1 = controlPoints[bezierIndex + 1].position;
                    Vector3 p2 = controlPoints[bezierIndex + 2].position;
                    Vector3 p3 = controlPoints[bezierIndex + 3].position;

                    // Calcule la position avec la formule de B�zier
                    newPos = CubicBezier(p0, p1, p2, p3, t);

                    // Calcule la tangente (d�riv�e) pour orienter la cam�ra
                    //tangent = CubicBezierTangent(p0, p1, p2, p3, t).normalized;

                    break;
                }

            case 2:
                {
                    // --- Mode Catmull-Rom ---
                    // Pour Catmull-Rom, chaque segment est d�fini par 4 points cons�cutifs
                    int catmullSegments = controlPoints.Length - 3;
                    int catmullIndex = currentSegment; // Parcourt de 0 � catmullSegments - 1

                    // V�rifie qu'on a assez de points pour ce segment
                    if (controlPoints.Length < catmullIndex + 4)
                    {
                        break;
                    }

                    // R�cup�re les 4 points pour Catmull-Rom
                    Vector3 p0 = controlPoints[catmullIndex].position;
                    Vector3 p1 = controlPoints[catmullIndex + 1].position;
                    Vector3 p2 = controlPoints[catmullIndex + 2].position;
                    Vector3 p3 = controlPoints[catmullIndex + 3].position;

                    // Calcule la position sur la courbe Catmull-Rom
                    newPos = CatmullRom(p0, p1, p2, p3, t);

                    // Calcule la tangente (d�riv�e) pour l'orientation
                    tangent = CatmullRomTangent(p0, p1, p2, p3, t).normalized;

                    break;
                }
        }


        // Met � jour la position
        transform.position = newPos;

        // Si la tangente est valide, on calcule la rotation puis on ajoute un pitch (inclinaison)
        if (tangent != Vector3.zero)
        {
            // Rotation de base (regarde dans la direction de 'tangent')
            Quaternion baseRotation = Quaternion.LookRotation(tangent, Vector3.up);
            // Inclinaison suppl�mentaire vers le bas (pitch n�gatif)
            Quaternion pitchDown = Quaternion.Euler(-pitchAngle, 0f, 0f);
            // Combine les deux rotations
            transform.rotation = baseRotation * pitchDown;
        }

        // Progression du param�tre t
        t += speed * Time.deltaTime;
        if (t >= 1f)
        {
            t = 0f;
            currentSegment++;
            // Bouclage selon le nombre de segments pour chaque mode
            if (currentMode == 0)
            {
                if (currentSegment >= controlPoints.Length - 1)
                    currentSegment = 0;
            }
            else if (currentMode == 1)
            {
                if (currentSegment >= (controlPoints.Length - 1) / 3)
                    currentSegment = 0;
            }
            else if (currentMode == 2)
            {
                if (currentSegment >= controlPoints.Length - 3)
                    currentSegment = 0;
            }
        }
    }

    // Courbe B�zier cubique
    Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        return uuu * p0 + 3f * uu * t * p1 + 3f * u * tt * p2 + ttt * p3;
    }

    Vector3 CubicBezierTangent(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1f - t;
        return 3f * u * u * (p1 - p0)
             + 6f * u * t * (p2 - p1)
             + 3f * t * t * (p3 - p2);
    }

    // Courbe Catmull-Rom
    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
        );
    }

    Vector3 CatmullRomTangent(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f * (
            (-p0 + p2)
          + 2f * (2f * p0 - 5f * p1 + 4f * p2 - p3) * t
          + 3f * (-p0 + 3f * p1 - 3f * p2 + p3) * t * t
        );
    }
}
