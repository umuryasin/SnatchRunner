using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Canvas ParentCanvas;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject GameMenu;
    [SerializeField] private GameObject LoseMenu;
    [SerializeField] private GameObject WinMenu;
    //[SerializeField] private GameObject EngGameMenuSuccess;
    //[SerializeField] private GameObject EngGameMenuFail;

    private static MenuManager instance;
    public static MenuManager Instance => instance ?? (instance = FindObjectOfType<MenuManager>());

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            //Destroy(MainMenu);
            //Destroy(GameMenu);
            //Destroy(LoseMenu);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //DontDestroyOnLoad(ParentCanvas.gameObject);
        }


        GameManager.onGameStateChanged += GameManager_onGameStateChanged;
    }

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
        MainMenu.SetActive(false);
        GameMenu.SetActive(true);

        //CameraMainMenu.SetActive(false);
    }

    private void EndGameFinish()
    {
        //MainMenu.SetActive(false);
        //CameraEndGame.SetActive(true);

     
    }

    private void RestartGame()
    {
        Debug.Log(" burada");

        MainMenu.SetActive(true);
        GameMenu.SetActive(false);
    }

    private void LoseGameMenu()
    {
        LoseMenu.SetActive(true);
        GameMenu.SetActive(false);
    }

    private void WinGameMenu()
    {
        WinMenu.SetActive(true);
        GameMenu.SetActive(false);
    }

}
