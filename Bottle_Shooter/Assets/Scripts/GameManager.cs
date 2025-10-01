using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject youWonPanel;
    public GameObject gameOverPanel;
    public AudioSource audioSource;
    public Button nextLevelButton;
    public Button replayButton;

    [Header("Level Settings")]
    public int startingAmmo = 6;
    public int bottlesToWin = 3;
    public float levelTime = 10f;

    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI ammoText;

    public int bottlesShot = 0;
    private int currentAmmo;
    private bool gameEnded = false;
    private float remainingTime;

    void Start()
    {
        currentAmmo = startingAmmo;
        remainingTime = levelTime;

        if (youWonPanel) youWonPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);

        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(LoadNextLevel);

        if (replayButton != null)
            replayButton.onClick.AddListener(ReloadScene);

        UpdateAmmoUI();
    }

    void Update()
    {
        if (gameEnded) return;

        remainingTime -= Time.deltaTime;
        if (timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(remainingTime).ToString();

        if (remainingTime <= 0f && bottlesShot < bottlesToWin)
        {
            GameOver();
        }
    }

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

    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = "Bullets: " + currentAmmo.ToString();
    }
}
