using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Transform[] spawnPoints;

    private CharacterController controller;
    private PlayerMovement movement;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        movement = GetComponent<PlayerMovement>();

        GameObject[] spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");

        spawnPoints = new Transform[spawns.Length];

        for (int i = 0; i < spawns.Length; i++)
        {
            spawnPoints[i] = spawns[i].transform;
        }
    }

    public void KillPlayer()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        if (movement != null)
            movement.canMove = false;

        controller.enabled = false;

        transform.position = new Vector3(0, -999f, 0);

        yield return new WaitForSeconds(1f);

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points found! Make sure objects are tagged 'SpawnPoint'");
            yield break;
        }

        int index = Random.Range(0, spawnPoints.Length);
        Transform spawn = spawnPoints[index];

        transform.position = spawn.position;
        transform.rotation = spawn.rotation;

        controller.enabled = true;

        if (movement != null)
            movement.canMove = true;
    }
}
