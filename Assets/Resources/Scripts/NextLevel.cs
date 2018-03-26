using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {

    public float xPosition;
    public string levelName;

    // player one and two
    private GameObject playerOne;
    private GameObject playerTwo;

    // Use this for initialization
    void Start () {
        // get script for player one health
        playerOne = GameObject.Find("P1");
        playerTwo = GameObject.Find("P2");

    }
	
	// Update is called once per frame
	void Update () {
		if (playerOne.transform.position.x >= xPosition || playerTwo.transform.position.x >= xPosition)
        {
            SceneManager.LoadScene("Level 2-2");
        }
	}
}
