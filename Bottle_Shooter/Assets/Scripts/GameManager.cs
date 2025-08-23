////using UnityEngine;
////using UnityEngine.SceneManagement;

////public class GameManager : MonoBehaviour
////{
////    [Header("UI Panels")]
////    public GameObject youWonPanel;
////    public GameObject gameOverPanel;

////    [Header("Level Settings")]
////    public int startingAmmo = 6;   // 👈 Level 1 setting
////    public int bottlesToWin = 3;

////    private int bottlesShot = 0;

////    void Start()
////    {
////        if (youWonPanel) youWonPanel.SetActive(false);
////        if (gameOverPanel) gameOverPanel.SetActive(false);
////    }

////    public int GetStartingAmmo()
////    {
////        return startingAmmo;
////    }

////    public void OnBulletFired(int currentAmmo)
////    {
////        if (currentAmmo <= 0 && bottlesShot == 0)
////        {
////            GameOver();
////        }
////    }

////    public void OnBottleShot()
////    {
////        bottlesShot++;
////        if (bottlesShot >= bottlesToWin)
////        {
////            YouWon();
////        }
////    }

////    void YouWon()
////    {
////        if (youWonPanel) youWonPanel.SetActive(true);
////        Time.timeScale = 0f;
////    }

////    void GameOver()
////    {
////        if (gameOverPanel) gameOverPanel.SetActive(true);
////        Time.timeScale = 0f;
////    }
////    public void ReloadScene()
////    {
////        Time.timeScale = 1f; // reset time scale before reload
////        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
////    }

////}
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using System.Collections;

//public class GameManager : MonoBehaviour
//{
//    [Header("UI Panels")]
//    public GameObject youWonPanel;
//    public GameObject gameOverPanel;
//    public GameObject winPanel; // when all bottles destroyed
//    public AudioSource audioSource;
//    public Button nextLevelButton; // 🎯 Assign in inspector

//    [Header("Level Settings")]
//    public int startingAmmo = 6;   // 👈 Level 1 ammo
//    public int bottlesToWin = 3;   // win by shooting X bottles

//    private int bottlesShot = 0;
//    private bool levelCompleted = false;
//    private GameObject[] bottles;

//    void Start()
//    {
//        // Bottle setup
//        bottles = GameObject.FindGameObjectsWithTag("Bottle");
//        if (winPanel) winPanel.SetActive(false);

//        if (nextLevelButton != null)
//            nextLevelButton.onClick.AddListener(LoadNextLevel);

//        // Panels setup
//        if (youWonPanel) youWonPanel.SetActive(false);
//        if (gameOverPanel) gameOverPanel.SetActive(false);
//    }

//    void Update()
//    {
//        // Check if all bottles destroyed (old BottleManager logic)
//        if (!levelCompleted && AreAllBottlesDestroyed())
//        {
//            levelCompleted = true;
//            StartCoroutine(ShowPanelWithDelay(1.5f));
//        }
//    }

//    // ----------- AMMO & BOTTLE WIN/LOSE LOGIC -----------

//    public int GetStartingAmmo()
//    {
//        return startingAmmo;
//    }

//    public void OnBulletFired(int currentAmmo)
//    {
//        if (currentAmmo <= 0 && bottlesShot == 0)
//        {
//            GameOver();
//        }
//    }

//    public void OnBottleShot()
//    {
//        bottlesShot++;
//        if (bottlesShot >= bottlesToWin)
//        {
//            YouWon();
//        }
//    }

//    // ----------- WIN / LOSE METHODS -----------

//    void YouWon()
//    {
//        if (youWonPanel) youWonPanel.SetActive(true);
//        Time.timeScale = 0f;
//    }

//    void GameOver()
//    {
//        if (gameOverPanel) gameOverPanel.SetActive(true);
//        Time.timeScale = 0f;
//    }

//    // ----------- BOTTLE DESTROY LOGIC -----------

//    IEnumerator ShowPanelWithDelay(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        ShowPanel();
//    }

//    void ShowPanel()
//    {
//        if (winPanel) winPanel.SetActive(true);
//        if (audioSource) audioSource.Play();
//    }

//    bool AreAllBottlesDestroyed()
//    {
//        foreach (GameObject bottle in bottles)
//        {
//            if (bottle != null)
//                return false;
//        }
//        return true;
//    }

//    // ----------- SCENE MANAGEMENT -----------

//    public void BackToMenu()
//    {
//        SceneManager.LoadScene(0);
//    }

//    public void LoadNextLevel()
//    {
//        Debug.Log("Next Level button clicked");
//        Time.timeScale = 1f;
//        SceneManager.LoadScene("Level2");
//    }

//    public void ReloadScene()
//    {
//        Time.timeScale = 1f; // reset before reload
//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//    }
//}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject youWonPanel;   // shows when 3 bottles are hit
    public GameObject gameOverPanel; // shows when bullets are over & bottles < 3
    public AudioSource audioSource;
    public Button nextLevelButton;   // assign in inspector
    public Button replayButton;      // assign in inspector

    [Header("Level Settings")]
    public int startingAmmo = 6;     // max bullets per level
    public int bottlesToWin = 3;     // need 3 bottles to win

    private int bottlesShot = 0;
    private int currentAmmo;
    private bool gameEnded = false;

    void Start()
    {
        currentAmmo = startingAmmo;

        // Hide panels initially
        if (youWonPanel) youWonPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);

        // Button listeners
        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(LoadNextLevel);

        if (replayButton != null)
            replayButton.onClick.AddListener(ReloadScene);
    }

    // ----------- BULLET & BOTTLE EVENTS -----------

    public void OnBulletFired()
    {
        if (gameEnded) return;

        currentAmmo--;

        // Check if out of ammo and not enough bottles shot
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
            YouWon();
        }
    }

    // ----------- WIN / LOSE METHODS -----------

    void YouWon()
    {
        gameEnded = true;
        if (youWonPanel) youWonPanel.SetActive(true);
        if (audioSource) audioSource.Play();
        Time.timeScale = 0f; // pause
    }

    void GameOver()
    {
        gameEnded = true;
        if (gameOverPanel) gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // pause
    }

    // ----------- SCENE MANAGEMENT -----------

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        Debug.Log("Next Level button clicked");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public int GetRemainingAmmo()
    {
        return currentAmmo;
    }
}
