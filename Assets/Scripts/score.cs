using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour {

    private const double BOX_HEIGHT_CORRECTION = 0.15;
    private const float DELTA_TIME = 0.25f;
    private const float DELTA_H = 0.01f;
    private float startCounter;

    private decimal maxHeight = 0.0m;

    public Text currentScoreGT;


    // Use this for initialization
    void Start () {
        // Initializes the score to 0
        GameObject currentScoreGO = GameObject.Find("ScoreDisplay");
        currentScoreGT = currentScoreGO.GetComponent<Text>();
        currentScoreGT.text = "0";
	}
	
	// Update is called once per frame
	void Update () {
        decimal height = this.FindHight();
        // Update score if new maximum height was found
        if (height > decimal.Parse(currentScoreGT.text))
        {
            checkNewHeight(height);
        }
	}

    void checkNewHeight(decimal tempHeight)
    {
        if (Mathf.Abs((float)(tempHeight - maxHeight)) < DELTA_H)
        {
            if ((Time.time - startCounter) > DELTA_TIME) {
                // Update the new score.
                currentScoreGT.text = tempHeight.ToString("N2");
                if (tempHeight > (decimal)HighScore.score)
                {
                    HighScore.score = (float)tempHeight;
                }
            }
        } else
        {
            // See if the height stabilises to the current value now.
            startCounter = Time.time;
            maxHeight = tempHeight;
        }

    }

    private decimal FindHight()
    {
        decimal maxheight = 0.0m;
        // Get a list of all the boxes
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        
        // Goes through the list to find the highest
        foreach(GameObject box in boxes)
        {
            decimal height = (decimal)(box.transform.position.y - BOX_HEIGHT_CORRECTION);
            if(height > maxheight)
            {
                maxheight = height;
            }
        }

        // Return max height
        return maxheight;
    }
}
