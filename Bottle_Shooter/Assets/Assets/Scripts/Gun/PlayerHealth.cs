using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public GameObject deathPanel; // Panel with "Death" text and restart button

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (deathPanel != null)
            deathPanel.SetActive(false);

        Time.timeScale = 1f; // Ensure game runs normally at start
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth / maxHealth;

        if (healthText != null)
            healthText.text = $"{(int)currentHealth} / {(int)maxHealth}";
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player Died!");
        StartCoroutine(ShowDeathPanelWithDelay());
    }



    public void RestartGame()
    {
        Time.timeScale = 1f;    
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator ShowDeathPanelWithDelay()
    {
        yield return new WaitForSecondsRealtime(0.2f); // Wait in unscaled time
        if (deathPanel != null)
            deathPanel.SetActive(true);

        Time.timeScale = 0f; // Freeze game AFTER UI is visible and interactive
    }

}
