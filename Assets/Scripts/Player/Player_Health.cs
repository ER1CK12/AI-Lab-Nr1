using UnityEditor;
using UnityEngine;

public class Player_Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private ParticleSystem particle;

    private void Awake()
    {
        currentHealth = maxHealth;
        particle = GetComponent<ParticleSystem>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        particle.Emit(20);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
#if UNITY_EDITOR 
       
        EditorApplication.ExitPlaymode();

#endif
    }
}
