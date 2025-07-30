
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            ShowPanel();
        }
    }

    //void ShowPanel()
    //{
    //    winPanel.SetActive(true);
    //    audioSource.Play();
    //    Time.timeScale = 0f; // pause game while showing win panel
    //}
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

    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // resume time
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextSceneIndex);
        else
            Debug.Log("No more levels!");
    }
}

