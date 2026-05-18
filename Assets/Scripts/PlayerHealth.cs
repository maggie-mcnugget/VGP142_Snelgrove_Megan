using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;

    private PlayerRespawn respawn;
    private Animator animator;

    void Awake()
    {
        respawn = GetComponent<PlayerRespawn>();
        animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int damage, string hitType)
    {
        if (health <= 0) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
            return;
        }

        if (animator != null)
        {
            if (hitType == "Punch")
                animator.SetTrigger("HitPunch");

            else if (hitType == "Kick")
                animator.SetTrigger("HitKick");
        }

    }

    void Die()
    {
        if (animator != null)
            animator.SetTrigger("Die");

        if (respawn != null)
            respawn.KillPlayer();
    }
}
