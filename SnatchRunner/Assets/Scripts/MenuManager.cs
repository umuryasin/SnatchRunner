using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Canvas MainMenu;
    [SerializeField] private Canvas GameMenu;
    [SerializeField] private Canvas LoseMenu;
    [SerializeField] private Canvas WinMenu;
    //[SerializeField] private GameObject EngGameMenuSuccess;
    //[SerializeField] private GameObject EngGameMenuFail;

    //private static MenuManager instance;
    //public static MenuManager Instance => instance ?? (instance = FindObjectOfType<MenuManager>());

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

    //void Awake()
    //{
    //    if (instance != null)
    //    {
    //        Destroy(gameObject);
    //        //Destroy(MainMenu.gameObject);
    //        //Destroy(GameMenu.gameObject);
    //        //Destroy(LoseMenu.gameObject);
    //        //Destroy(WinMenu.gameObject);
    //    }
    //    else
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //        //DontDestroyOnLoad(MainMenu.gameObject);
    //        //DontDestroyOnLoad(GameMenu.gameObject);
    //        //DontDestroyOnLoad(LoseMenu.gameObject);
    //        //DontDestroyOnLoad(WinMenu.gameObject);
    //    }
    //}

    private void GameManager_onGameStateChanged(GameStates GameState)
    {
        if (GameState == GameStates.Started)
        {
            StartGame();
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
    }

    private void StartGame()
    {
        Debug.Log("Menu Manager start game");
        MainMenu.gameObject.SetActive(false);
        GameMenu.gameObject.SetActive(true);

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
    }

    private void WinGameMenu()
    {
        WinMenu.gameObject.SetActive(true);
        GameMenu.gameObject.SetActive(false);
    }

}
