using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject playerPrefab;

    void Start()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

       

        GameObject playerObj = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        playerObj.name = "PlayerRuntime";

        SimpleFollowAI[] enemies = FindObjectsOfType<SimpleFollowAI>();

        foreach (var enemy in enemies)
        {
            enemy.SetTarget(playerObj.transform);
        }
    }
}
