using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerRespawn respawn = collision.gameObject.GetComponent<PlayerRespawn>();

            if (respawn != null)
            {
                respawn.KillPlayer();
            }

            Destroy(gameObject);
        }
    }
}