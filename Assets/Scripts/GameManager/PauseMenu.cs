using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public Button[] BackToMainMenuButton;
    public Button RetryButton;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (GameIsPaused && !(GameManager.Instance.currentState == GameManager.GameState.GameOver))
        {
            GameIsPaused = false;
            Debug.Log("Resuming Game");
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
            GameManager.Instance.currentState = GameManager.GameState.InGame; 
            InGameUIManager.Instance.UpdateUIBasedOnState();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!GameIsPaused && !(GameManager.Instance.currentState == GameManager.GameState.GameOver))
        {
            GameIsPaused = true;
            Debug.Log("Pausing Game");
            Time.timeScale = 0f;
            pauseMenuUI.SetActive(true);
            GameManager.Instance.currentState = GameManager.GameState.Paused;
            InGameUIManager.Instance.UpdateUIBasedOnState();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void BackToMainMenu()
    {
        foreach (Button button in BackToMainMenuButton)
        {
            button.interactable = false;
        }
        Time.timeScale = 1f;
        FindObjectOfType<ScoreManager>().SaveScore();
        SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.TransisionToScene(1, "MainMenu"));
        Debug.Log("Loading Main Menu");
        // GameIsPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Retry()
    {
        FindObjectOfType<ScoreManager>().SaveScore();
        GameManager.Instance.currentState = GameManager.GameState.InGame;
        RetryButton.interactable = false;
        SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.TransisionToScene(2, "GameScene"));
        Debug.Log("Retrying Game");
        // GameIsPaused = false;
    }
}
