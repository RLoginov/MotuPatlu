using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class startUpLogos : MonoBehaviour {

    public int fader = 0;
    private float currentAlpha = 0;
    private int currentFrame = 0;

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, currentAlpha);
        int aestheticMagiku = fader;
        if (currentFrame > aestheticMagiku && currentFrame < aestheticMagiku + 100)
        {
            currentAlpha = currentAlpha + 0.05f;
        }
 
        if (currentFrame > aestheticMagiku + 160 && aestheticMagiku > 10)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        currentFrame++;
	}
}
