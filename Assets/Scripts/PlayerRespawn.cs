using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Transform[] spawnPoints;

    private CharacterController controller;
    private PlayerMovement movement;
    private Animator animator;

    private bool isDead = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        movement = GetComponent<PlayerMovement>();
        animator = GetComponentInChildren<Animator>();

        GameObject[] spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");

        spawnPoints = new Transform[spawns.Length];

        for (int i = 0; i < spawns.Length; i++)
        {
            spawnPoints[i] = spawns[i].transform;
        }
    }

    public void KillPlayer()
    {
        if (isDead) return;

        isDead = true;

        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        // disable movement
        if (movement != null)
            movement.canMove = false;

        // play death animation
        if (animator != null)
            animator.SetTrigger("Die");

        // wait for animation to finish
        yield return new WaitForSeconds(2f);

        controller.enabled = false;

        // temporarily hide player
        transform.position = new Vector3(0, -999f, 0);

        yield return new WaitForSeconds(0.2f);

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

        // re-enable movement
        if (movement != null)
            movement.canMove = true;

        isDead = false;
    }
}