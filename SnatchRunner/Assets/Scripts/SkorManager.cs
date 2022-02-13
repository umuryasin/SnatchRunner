using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkorManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtScoreGame;
    [SerializeField] private TextMeshProUGUI txtScoreLose;
    [SerializeField] private TextMeshProUGUI txtScoreWin;

    private int Score = 0;

    private static SkorManager instance;
    public static SkorManager Instance => instance ?? (instance = FindObjectOfType<SkorManager>());

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
       
    public void ScoreUp()
    {
        Score++;
        txtScoreGame.text = Score.ToString();
        txtScoreWin.text = Score.ToString();
        txtScoreLose.text = Score.ToString();
        GameManager.Instance.ScoreUp();
    }

    public int GetScore()
    {
        return Score;
    }
}
