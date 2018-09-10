using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {

    static public float score = 0;
    void Update()
    {
        Text gt = this.GetComponent<Text>();
        gt.text = "High Score: " + ((decimal)score).ToString("N2");

        // Update the PlayerPrefs HighScore if necessary
        if (score > PlayerPrefs.GetFloat("HighScore"))
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }
    }

    void Awake()
    {
        // If the PlayerPrefs HighScore already exists, read it
        if (PlayerPrefs.HasKey("HighScore"))
        {
            score = PlayerPrefs.GetFloat("HighScore");
        }
        // Assign the high score to HighScore
        PlayerPrefs.SetFloat("HighScore", score);
    }
}
