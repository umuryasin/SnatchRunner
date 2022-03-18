using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Canvas MainMenu;
    [SerializeField] private Canvas GameMenu;
    [SerializeField] private Canvas LoseMenu;
    [SerializeField] private Canvas WinMenu;
    [SerializeField] private Canvas AdsMenu;

    [SerializeField] private TextMeshProUGUI txtScoreGame;
    [SerializeField] private TextMeshProUGUI txtScoreLose;
    [SerializeField] private TextMeshProUGUI txtScoreWin;

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
        InputManager.OnShowAds += InputManager_OnShowAds;
    }

    

    void OnDisable()
    {
        // Unsubscribe to the event
        InputManager.OnShowAds -= InputManager_OnShowAds;
        GameManager.onGameStateChanged -= GameManager_onGameStateChanged;
    }

    private void GameManager_onGameStateChanged(GameStates GameState)
    {
        if (GameState == GameStates.Started)
        {
            StartGame();
            AdsMenu.gameObject.SetActive(true);
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
        //else if(GameState==GameStates.ShowInterAds)
        //    AdsMenu.gameObject.SetActive(true);

    }
    private void InputManager_OnShowAds()
    {
        ShowAdsCanvas();
    }
    private void StartGame()
    {
        Debug.Log("Menu Manager start game");
        MainMenu.gameObject.SetActive(false);
        GameMenu.gameObject.SetActive(true);
        LoseMenu.gameObject.SetActive(false);
        //CameraMainMenu.SetActive(false);
    }

    private void EndGameFinish()
    {
        //MainMenu.SetActive(false);
        //CameraEndGame.SetActive(true);


    }

    private void RestartGame()
    {
        LoseMenu.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(true);
        GameMenu.gameObject.SetActive(false);
    }

    private void LoseGameMenu()
    {
        LoseMenu.gameObject.SetActive(true);
        GameMenu.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(false);
    }

    private void WinGameMenu()
    {
        WinMenu.gameObject.SetActive(true);
        GameMenu.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(false);

    }
    private void ShowAdsCanvas()
    {
        LoseMenu.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(false);
        GameMenu.gameObject.SetActive(false);
    }

}
