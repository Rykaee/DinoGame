using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverScore : MonoBehaviour
{
    // Player avatar image and text for name and score
    public Image avatarImage;
    public TMP_Text nameAndScore;

    // Array of avatar sprites
    public Sprite[] sprites;

    public TMP_Text scoreUI;
    private int displayScore;
    private AudioManager audiomanager;

    // Start is called before the first frame update
    void Start()
    {
        audiomanager = FindObjectOfType<AudioManager>();
        displayScore = 0;
        StartCoroutine(ScoreUpdater());

        // Hide avatar image and player text
        nameAndScore.enabled = false;
        avatarImage.enabled = false;
    }

    private IEnumerator ScoreUpdater()
    {
        bool isDone = false;
        while (!isDone)
        {
            if (displayScore < Score.score)
            {
                displayScore++;
                audiomanager.PlaySound("totalCoins");
                scoreUI.text = displayScore.ToString();
            } else
            {
                audiomanager.PlaySound("chickenGetsHome");
                isDone = true;
            }
            yield return new WaitForSeconds(0.1f);
        }

        // Save scores to database
        Player player = DatabaseLoader.GetCurrentPlayer();
        if(player != null)
        {
            player.Score += Score.score;
            DatabaseLoader.SaveCurrentPlayer();

            // Show player image and total score after counting new scores
            nameAndScore.enabled = true;
            avatarImage.enabled = true;

            int avatarId = player.AvatarId;
            avatarImage.sprite = sprites[avatarId];

            string text = player.PlayerName + ", " + player.Score + " pistettä";
            nameAndScore.text = text;
        }
    }
}