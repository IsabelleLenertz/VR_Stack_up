using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour {

    private const double BOX_HEIGHT_CORRECTION = 0.15;
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
        double height = this.FindHight();
        // Update score if new maximum height was found
        if (height > double.Parse(currentScoreGT.text))
        {
            currentScoreGT.text = height.ToString();
        }
	}

    private double FindHight()
    {
        double maxheight = 0.0;
        // Get a list of all the boxes
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        
        // Goes through the list to find the highest
        foreach(GameObject box in boxes)
        {
            double height = box.transform.position.y - BOX_HEIGHT_CORRECTION;
            if(height > maxheight)
            {
                maxheight = height;
            }
        }

        // Return max height
        return maxheight;
    }
}
