using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour {

    private const double BOX_HEIGHT_CORRECTION = 0.15;
    private const float DELTA_TIME = 0.25f;
    private const float DELTA_H = 0.01f;
    private float startCounter;

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

    IEnumerator checkNewHeight(decimal tempHeight)
    {
        startCounter = Time.time;
        
        while( (Time.time - startCounter) < DELTA_TIME)
        {
            if( Mathf.Abs((float)(tempHeight - FindHight())) > DELTA_H)
            {
                yield break;
            }
            yield return null;

        }

        // the while loop exits if a certain height was kept for longer than delta time
        currentScoreGT.text = tempHeight.ToString("N2");
        if (tempHeight > (decimal)HighScore.score)
        {
            HighScore.score = (float)tempHeight;
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
