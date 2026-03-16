using System.Collections;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    [Header("Dummy Stats")]
    public float maxHealth = 100f;
    private float currentHealth;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // This is the function Kyle will call when he punches!
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Suggestion Box took " + damageAmount + " damage! Health: " + currentHealth);

        // Trigger the flash effect
        StartCoroutine(FlashRed());

        // If it dies, just reset it since it's a training dummy
        if (currentHealth <= 0)
        {
            Debug.Log("Suggestion Box Destroyed! Resetting...");
            currentHealth = maxHealth;
        }
    }

    // A simple Coroutine to make the sprite flash red for a split second
    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}