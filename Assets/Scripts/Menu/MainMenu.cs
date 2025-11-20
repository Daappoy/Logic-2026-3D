using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;
    public Button PlayButton;
    public GameObject settingsUI;
    public TextMeshProUGUI highScoreText;

    void Start()
    {   
        //set cursor 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        OpenMainMenu();
        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = highscore.ToString();
    }

    public void StartGame()
    {
        SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.TransisionToScene(1, "GameScene"));
        PlayButton.interactable = false;
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }

    public void OpenMainMenu()
    {
        settingsUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void OpenSettings()
    {
        mainMenuUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    public void UpdateHighScore()
    {
        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = highscore.ToString();
    }
}
