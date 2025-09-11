
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject youWonPanel;    // shows when enough bottles are hit
    public GameObject gameOverPanel;  // shows when time ends or ammo ends
    public AudioSource audioSource;
    public Button nextLevelButton;    // assign in inspector (only level 1)
    public Button replayButton;       // assign in inspector

    [Header("Level Settings")]
    public int startingAmmo = 6;      // bullets per level
    public int bottlesToWin = 3;      // Level1 = 3, Level2 = 4
    public float levelTime = 10f;     // Level1 = 10, Level2 = 30

    [Header("UI Elements")]
    public TextMeshProUGUI timerText;   // assign in inspector
    public TextMeshProUGUI ammoText;    // NEW: assign in inspector for bullet count

    public int bottlesShot = 0;
    private int currentAmmo;
    private bool gameEnded = false;
    private float remainingTime;

    void Start()
    {
        currentAmmo = startingAmmo;
        remainingTime = levelTime;

        // Hide panels initially
        if (youWonPanel) youWonPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);

        // Button listeners
        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(LoadNextLevel);

        if (replayButton != null)
            replayButton.onClick.AddListener(ReloadScene);

        UpdateAmmoUI(); // initialize bullet UI
    }

    void Update()
    {
        if (gameEnded) return;

        // Countdown timer
        remainingTime -= Time.deltaTime;
        if (timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(remainingTime).ToString();

        // Time’s up
        if (remainingTime <= 0f && bottlesShot < bottlesToWin)
        {
            GameOver();
        }
    }

    // ----------- BULLET & BOTTLE EVENTS -----------

    public void OnBulletFired()
    {
        if (gameEnded) return;

        currentAmmo--;
        UpdateAmmoUI();

        if (currentAmmo <= 0 && bottlesShot < bottlesToWin)
        {
            GameOver();
        }
    }

    public void OnBottleShot()
    {
        if (gameEnded) return;

        bottlesShot++;

        if (bottlesShot >= bottlesToWin)
        {
            StartCoroutine(ShowWinWithDelay(1f));
        }
    }

    IEnumerator ShowWinWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        YouWon();
    }

    // ----------- WIN / LOSE METHODS -----------

    void YouWon()
    {
        if (gameEnded) return;

        gameEnded = true;
        if (youWonPanel) youWonPanel.SetActive(true);
        if (audioSource) audioSource.Play();

        Time.timeScale = 0f;
    }

    void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;
        if (gameOverPanel) gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // ----------- SCENE MANAGEMENT -----------

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public int GetRemainingAmmo()
    {
        return currentAmmo;
    }

    // ----------- UI UPDATES -----------

    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = "Bullets: " + currentAmmo.ToString();
    }
}
