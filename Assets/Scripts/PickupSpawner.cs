using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject pickupPrefab;

    void Start()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        Instantiate(pickupPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
