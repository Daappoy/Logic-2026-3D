using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState
    {
        InGame,
        Paused,
        GameOver
    }
    public GameState currentState;
    public ScoreManager scoreManager;
    public bool LoseGameisTriggered = false;
    
    [Header("Statistics")]
    
    public int EnemyKilled;
    public int HealUsed;
    public int UltimateUsed;
    
    void Start()
    {
        currentState = GameState.InGame;
        InGameUIManager.Instance.UpdateUIBasedOnState();
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        
        scoreManager = FindObjectOfType<ScoreManager>();

        ResetGameStats();
    }
    public void ResetGameStats()
    {
        scoreManager.TotalScore = 0;
        EnemyKilled = 0;
        HealUsed = 0;
        UltimateUsed = 0;
    }
    public void ScoreIncrease()
    {
        scoreManager.TotalScore += 100;
        InGameUIManager.Instance.currentScoreText.text = "Score: " + scoreManager.TotalScore.ToString();
    }

    public void LoseGame()
    {
        if (!LoseGameisTriggered)
        {
            currentState = GameState.GameOver;
            InGameUIManager.Instance.UpdateUIBasedOnState();
            InGameUIManager.Instance.InitializeStatsText();
            LoseGameisTriggered = true;
            Time.timeScale = 0f;
        } else return;
    }
    
}
