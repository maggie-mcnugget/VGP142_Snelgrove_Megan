using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float lifeTime = 3f;
    public float damage = 20f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        MeleeEnemy enemy = collision.gameObject.GetComponentInParent<MeleeEnemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage, "Shoot");
        }

        Destroy(gameObject);
    }
}
