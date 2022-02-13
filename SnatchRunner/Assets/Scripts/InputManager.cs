using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //private static InputManager instance;
    //public static InputManager Instance => instance ?? (instance = FindObjectOfType<InputManager>());

    //// Start is called before the first frame update
    //void Awake()
    //{
    //    if (instance != null)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //}

    public void StartGame()
    {
        Debug.Log("starttttt");
        GameManager.Instance.StartGame();
    }

    public void ResetGame()
    {
        GameManager.Instance.RestartGame();
    }
}
