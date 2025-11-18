using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int TotalScore = 0;
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.HasKey("PlayerScore"))
        {
            TotalScore = PlayerPrefs.GetInt("PlayerScore");
        }
        else
        {
            TotalScore = 0;
        }
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("PlayerScore", TotalScore);

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (TotalScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", TotalScore);
        }
        PlayerPrefs.Save();
    }
}
