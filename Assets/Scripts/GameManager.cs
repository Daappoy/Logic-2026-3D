using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        InGame,
        Paused,
        GameOver
    }
    public static GameState currentState = GameState.InGame;

    [SerializeField]
    private GameState initialState = GameState.InGame;

    private void Awake()
    {
        currentState = initialState;
    }

}
