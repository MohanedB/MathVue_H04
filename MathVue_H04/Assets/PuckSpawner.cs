using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckSpawner : MonoBehaviour
{
    public GameObject puckPrefab;      // Préfab de la poque
    public FieldLimits fieldLimits;    // Référence globale aux limites du terrain
    public float spawnInterval = 2f;   // Intervalle de spawn en secondes

    public float minSpeed = 2f;
    public float maxSpeed = 5f;

    private float timer = 0f;
    // Liste pour suivre tous les pucks présents sur le terrain
    private List<GameObject> spawnedPucks = new List<GameObject>();

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnPuck();
            timer = 0f;
        }
    }

    void SpawnPuck()
    {
        // Génération d'une position aléatoire dans la zone d'attaque
        float randomX = Random.Range(fieldLimits.xMin, fieldLimits.xMax);
        float randomZ = Random.Range(fieldLimits.zMin, fieldLimits.zMax);
        Vector3 spawnPosition = new Vector3(randomX, 0.1f, randomZ);

        // Instanciation de la poque
        GameObject newPuck = Instantiate(puckPrefab, spawnPosition, Quaternion.identity);

        // Configuration de la vitesse
        PuckController puckController = newPuck.GetComponent<PuckController>();
        float speed = Random.Range(minSpeed, maxSpeed);
        // On choisit une direction en fonction de la position du spawner
        Vector3 direction = (transform.position.x < 0) ? Vector3.right : Vector3.left;
        puckController.velocity = direction * speed;
        // Assurer que le spawner est attribué
        puckController.fieldLimits = fieldLimits;

        // Ajout du nouveau puck à la liste
        spawnedPucks.Add(newPuck);

        // Affiche le nombre de poques actuels
        Debug.Log("Nombre de poques actuels: " + spawnedPucks.Count);

        // Si le nombre de poques atteint 20 ou plus, on les détruit
        if (spawnedPucks.Count >= 20)
        {
            Debug.Log("20 poques atteints, destruction de toutes les poques.");
            foreach (GameObject puck in spawnedPucks)
            {
                Destroy(puck);
            }
            spawnedPucks.Clear();
            Debug.Log("Toutes les poques ont été détruites. Nombre de poques actuels: " + spawnedPucks.Count);
        }
    }
}
