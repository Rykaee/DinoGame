using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    private static TMP_Text text;
    public static int score;
    public static int numChickensInHome;
    public static int numChickensInGame;

    void Start()
    {
        Score.score = 0;
        Score.numChickensInHome = 0;
        Score.numChickensInGame = 4;
    }

    void Update()
    {

    }

    public static void AddChickenInHome()
    {
        Score.numChickensInHome++;
        FindObjectOfType<AudioManager>().PlaySound("chickenGetsHome");
    }
    public static void RemoveChickenInHome()
    {
        Score.numChickensInGame--;
    }

    // Find ScoreText component and add value to it.
    public static void AddScore(int value)
    {
        Score.score += value;
        GameObject go = GameObject.Find("ScoreText");
        if (go)
        {
          Score.text = go.GetComponent<TMP_Text>();
          Score.text.text = Score.score.ToString();
        }
    }

    // Find ScoreText component and reduce score if chicken gets eaten
    public static void RemoveScore(int value)
    {
        Score.score -= value;
        GameObject go = GameObject.Find("ScoreText");
        if (go)
        {
            Score.text = go.GetComponent<TMP_Text>();
            Score.text.text = Score.score.ToString();
        }
    }

    public static void ResetScore()
    {
        score = 0;
        GameObject go = GameObject.Find("ScoreText");
        if (go)
        {
            Score.text = go.GetComponent<TMP_Text>();
            Score.text.text = Score.score.ToString();
        }
    }
}
