using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckController : MonoBehaviour
{

    public Vector3 velocity;

    public float radius = 0.5f;

    public float constantY = 0.1f;

    public FieldLimits fieldLimits;
    void Start()
    {

    }

    void Update()
    {
        Vector3 newPos = transform.position;

        newPos.x = newPos.x + (velocity.x * Time.deltaTime);
        newPos.z = newPos.z + (velocity.z * Time.deltaTime);
        newPos.y = constantY;


        if ((newPos.x - radius) < fieldLimits.xMin)
        {
            newPos.x = fieldLimits.xMin + radius;

            velocity.x = -velocity.x;
        }

        if ((newPos.x + radius) > fieldLimits.xMax)
        {
            newPos.x = fieldLimits.xMax - radius;

            velocity.x = -velocity.x;
        }

        if ((newPos.z - radius) < fieldLimits.zMin)
        {
            newPos.z = fieldLimits.zMin + radius;

            velocity.z = -velocity.z;
        }

        if ((newPos.z + radius) > fieldLimits.zMax)
        {
            newPos.z = fieldLimits.zMax - radius;

            velocity.z = -velocity.z;
        }

        transform.position = newPos;

    }

}

