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
        if (GameIsPaused && !(GameManager.currentState == GameManager.GameState.GameOver))
        {
            InGameUIManager.Instance.GameUI.SetActive(true);
            Debug.Log("Resuming Game");
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
            GameIsPaused = false;
            GameManager.currentState = GameManager.GameState.InGame; 
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!GameIsPaused && !(GameManager.currentState == GameManager.GameState.GameOver))
        {
            InGameUIManager.Instance.GameUI.SetActive(false);
            Debug.Log("Pausing Game");
            Time.timeScale = 0f;
            pauseMenuUI.SetActive(true);
            GameIsPaused = true;
            GameManager.currentState = GameManager.GameState.Paused;
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
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Retry()
    {
        RetryButton.interactable = false;
        Time.timeScale = 1f;
        SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.TransisionToScene(2, "GameScene"));
        Debug.Log("Retrying Game");
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
