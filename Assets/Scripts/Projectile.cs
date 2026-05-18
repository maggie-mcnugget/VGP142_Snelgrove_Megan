using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn respawn = other.GetComponentInParent<PlayerRespawn>();

            if (respawn != null)
            {
                respawn.KillPlayer();
            }

            Destroy(gameObject);
        }
    }
}