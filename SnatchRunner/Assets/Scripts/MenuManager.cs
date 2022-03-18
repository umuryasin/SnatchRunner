using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityCore.Menu;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI txtScoreGame;
    [SerializeField] private TextMeshProUGUI txtScoreLose;
    [SerializeField] private TextMeshProUGUI txtScoreWin;
    [SerializeField] private PageController pages;

    //[SerializeField] private GameObject EngGameMenuSuccess;
    //[SerializeField] private GameObject EngGameMenuFail;

    //private static MenuManager instance;
    //public static MenuManager Instance => instance ?? (instance = FindObjectOfType<MenuManager>());

    public delegate void MenuManagerDelegate();
    public static event MenuManagerDelegate OnMenuLoaded;

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

    private void GameManager_onGameStateChanged(GameStates GameState)
    {
        if (GameState == GameStates.Started)
        {
            StartGame();
            OnMenuLoaded?.Invoke();
        }
        else if (GameState == GameStates.EndGameFinished)
        {
            EndGameFinish();
        }
        else if (GameState == GameStates.RestartGame)
        {
            RestartGame();
        }
        else if (GameState == GameStates.LoseGame)
        {
            LoseGameMenu();
        }
        else if (GameState == GameStates.WinGame)
        {
            WinGameMenu();
        } 
        else if (GameState == GameStates.ScoreUp)
        {
            int score = SkorManager.Instance.GetScore();

            txtScoreGame.text = score.ToString();
            txtScoreWin.text = score.ToString();
            txtScoreLose.text = score.ToString();
        }

    }
    private void StartGame()
    {
        Debug.Log("Menu Manager start game");
        pages.TurnPageOff(UnityCore.Menu.PageType.Menu);
        //CameraMainMenu.SetActive(false);
    }

    private void EndGameFinish()
    {
        //MainMenu.SetActive(false);
        //CameraEndGame.SetActive(true);


    }

    private void RestartGame()
    {
        pages.TurnPageOff(UnityCore.Menu.PageType.GameOver);
        pages.TurnPageOn(UnityCore.Menu.PageType.Menu);
    }

    private void LoseGameMenu()
    {
        pages.TurnPageOn(UnityCore.Menu.PageType.GameOver);
    }

    private void WinGameMenu()
    {
        pages.TurnPageOn(UnityCore.Menu.PageType.GameWin);
    }
}
