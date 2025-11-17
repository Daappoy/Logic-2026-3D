using System.Collections;
using System.Collections.Generic;
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
    public int EnemyKilled;
    public static GameState currentState = GameState.InGame;
    
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
        currentState = GameState.InGame;
    }

}
