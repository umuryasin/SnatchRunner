using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance ?? (instance = FindObjectOfType<GameManager>());
    public int LevelCount = 5;

    private GameStates gameState;

    public static event Action<GameStates> onGameStateChanged;

    private int Level = 1;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        gameState = GameStates.Initilazed;
    }

    public void updateGameState(GameStates newState)
    {
        gameState = newState;
        onGameStateChanged?.Invoke(newState);
    }

    public void StartGame()
    {
        updateGameState(GameStates.Started);
    }

    public void WinGame()
    {
        updateGameState(GameStates.WinGame);
    }

    public void LoseGame()
    {
        updateGameState(GameStates.LoseGame);
    }

    public void EndGameStart()
    {
        updateGameState(GameStates.EndGameStarted);
    }

    public void EndGameFinish()
    {
        updateGameState(GameStates.EndGameFinished);
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, UnityEngine.SceneManagement.LoadSceneMode.Single);
        updateGameState(GameStates.RestartGame);
    }

    public void NextLevel()
    {
        Level++;
        if (Level > LevelCount)
            Level = LevelCount;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene"+ Level, UnityEngine.SceneManagement.LoadSceneMode.Single);
        updateGameState(GameStates.NextLevel);
    }

    public void ScoreUp()
    {
        updateGameState(GameStates.ScoreUp);
    }
    public void AdsContinue()
    {
        updateGameState(gameState);
    }
}
