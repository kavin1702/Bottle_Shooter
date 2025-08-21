using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class BottleManager : MonoBehaviour
{
    public GameObject winPanel;
    public AudioSource audioSource;
    public Button nextLevelButton; // 🎯 Assign this in inspector

    private GameObject[] bottles;
    private bool levelCompleted = false;

    void Start()
    {
        bottles = GameObject.FindGameObjectsWithTag("Bottle");
        winPanel.SetActive(false);

        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(LoadNextLevel);
    }

    void Update()
    {
        if (!levelCompleted && AreAllBottlesDestroyed())
        {
            levelCompleted = true;
            StartCoroutine(ShowPanelWithDelay(1.5f)); // Call coroutine with 2 sec delay
        }
    }

    IEnumerator ShowPanelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // wait 2 seconds
        ShowPanel();
    }

    void ShowPanel()
    {
        winPanel.SetActive(true);
        audioSource.Play();
    }

    bool AreAllBottlesDestroyed()
    {
        foreach (GameObject bottle in bottles)
        {
            if (bottle != null)
                return false;
        }
        return true;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadNextLevel()
    {
        Debug.Log("Next Level button clicked");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level2");
    }
}
