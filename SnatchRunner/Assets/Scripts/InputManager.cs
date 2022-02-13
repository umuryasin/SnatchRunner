using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("yater artýk ya");
        GameManager.Instance.StartGame();
    }

    public void ResetGame()
    {
        GameManager.Instance.RestartGame();
    }
}
