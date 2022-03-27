using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkorManager : MonoBehaviour
{
    private int Score = 0;

    private static SkorManager instance;
    public static SkorManager Instance => instance ?? (instance = FindObjectOfType<SkorManager>());


    void OnEnable()
    {
        // Subscribe to the event
        GameManager.onGameStateChanged += GameManager_onGameStateChanged;
    }

    void OnDisable()
    {
        // Unsubscribe to the event
        GameManager.onGameStateChanged -= GameManager_onGameStateChanged;
    }

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

    }

    private void GameManager_onGameStateChanged(GameStates GameState)
    {
        if (GameState == GameStates.NextLevel || GameState==GameStates.RestartGame)
        {
            ResetScore();
        }
       
    }

    public void ScoreUp(int score=1)
    {
        Score+= score;
      
        GameManager.Instance.ScoreUp();
    }

    public int GetScore()
    {
        return Score;
    }

    public void ResetScore()
    {
        Score = 0;
    }

}
