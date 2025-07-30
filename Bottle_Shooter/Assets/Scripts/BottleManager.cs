using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BottleManager : MonoBehaviour
{
    public GameObject winPanel;
    public float delayBeforeNextLevel = 3f;
    public AudioSource audioSource;

    private GameObject[] bottles;
    private bool levelCompleted = false;

    void Start()
    {
        bottles = GameObject.FindGameObjectsWithTag("Bottle");
        winPanel.SetActive(false);
    }

    void Update()
    {
        if (!levelCompleted && AreAllBottlesDestroyed())
        {
            levelCompleted = true;
            Invoke("LoadNextLevel", delayBeforeNextLevel);
            Invoke("ShowPanel", 1.5f);
        }
    }

    public void ShowPanel()
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

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextSceneIndex);
        else
            Debug.Log("No more levels!");
    }
}
