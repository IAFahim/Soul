using Alchemy.Inspector;
using Pancake.Component;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HealthBar healthBar; // Assign this in the Inspector
    public float maxHealth = 100f;

    void Start()
    {
        // Initialize the HealthBar
        healthBar.Initialize(maxHealth, OnHealthChange, OnDeath);
    }

    [Button]
    public void TakeDamage(float damage)
    {
        healthBar.TakeDamage(damage);
    }

    // Called when health changes
    private void OnHealthChange(float newHealth, bool isIncrease)
    {
        // You could update a UI element here based on newHealth
        Debug.Log("Enemy health changed to: " + newHealth);
    }

    // Called when health reaches zero
    private void OnDeath()
    {
        Debug.Log("Enemy died!");
        // Handle enemy death, e.g., play death animation, destroy the object
        Destroy(gameObject);
    }
}
