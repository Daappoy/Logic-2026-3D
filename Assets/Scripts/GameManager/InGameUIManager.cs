using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance;
    public PauseMenu pauseMenu;
    public Slider Healthbar;
    public GameObject GameUI;
    public TextMeshProUGUI AmmoText;
    public TextMeshProUGUI ReloadText;
    public TextMeshProUGUI currentScoreText;
    [Header("Stats on Lose Game UI")]
    public TextMeshProUGUI FinaltScoreText;
    public TextMeshProUGUI EnemyKilledText;
    public TextMeshProUGUI HealUsedText;
    public TextMeshProUGUI UltimateUsedText;
    public GameObject LoseGameUI;
    private void Awake()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        ShowReloadText("", false);
        // Subscribe to player health changes if PlayerDisplay already exists
        if (PlayerDisplay.Instance != null)
        {
            PlayerDisplay.Instance.OnHealthChanged += HandlePlayerHealthChanged;
        }
    }
    private void OnDestroy()
    {
        if (PlayerDisplay.Instance != null)
        {
            PlayerDisplay.Instance.OnHealthChanged -= HandlePlayerHealthChanged;
        }
    }
    private void HandlePlayerHealthChanged(int current, int max)
    {
        Healthbar.maxValue = max;
        Healthbar.value = current;
    }
    // Fallback manual update if needed externally
    public void UpdateHealthBar()
    {
        if (PlayerDisplay.Instance != null)
        {
            HandlePlayerHealthChanged(PlayerDisplay.Instance.currentHealth, PlayerDisplay.Instance.MaxHealth);
        }
    }
    public void UpdateAmmoText(int currentAmmo, int maxAmmo)
    {
        AmmoText.text = currentAmmo.ToString() + " / " + maxAmmo.ToString();
    }
    public void ShowReloadText(string message,bool show)
    {
            ReloadText.text = message;
            ReloadText.gameObject.SetActive(show);
    }
    public void InitializeStatsText()
    {
        FinaltScoreText.text = "Score: " + GameManager.Instance.scoreManager.TotalScore.ToString();
        EnemyKilledText.text = "Enemies Killed: " + GameManager.Instance.EnemyKilled.ToString();
        HealUsedText.text = "Heals Used: " + GameManager.Instance.HealUsed.ToString();
        UltimateUsedText.text = "Ultimates Used: " + GameManager.Instance.UltimateUsed.ToString();
    }
    public void UpdateUIBasedOnState()
    {
        if(GameManager.Instance.currentState == GameManager.GameState.InGame)
        {
            GameUI.SetActive(true);
            LoseGameUI.SetActive(false);
            pauseMenu.pauseMenuUI.SetActive(false);
        }
        else if(GameManager.Instance.currentState == GameManager.GameState.Paused)
        {
            GameUI.SetActive(false);
            LoseGameUI.SetActive(false);
            pauseMenu.pauseMenuUI.SetActive(true);
        }
        else if(GameManager.Instance.currentState == GameManager.GameState.GameOver)
        {
            GameUI.SetActive(false);
            LoseGameUI.SetActive(true);
            pauseMenu.pauseMenuUI.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
