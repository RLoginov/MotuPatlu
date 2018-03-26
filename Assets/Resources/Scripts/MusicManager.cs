using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public int stageNumber = 0;
    private GameObject audioGuy;

    // Use this for initialization
    void Start () {
        audioGuy = GameObject.Find("AudioManager");

        if (stageNumber == 1)
        {
            audioGuy.GetComponent<AudioManager>().Play("Puyo");
        }

        if (stageNumber == 2)
        {
            audioGuy.GetComponent<AudioManager>().Play("Egypt");
        }

        if (stageNumber == 3)
        {
            audioGuy.GetComponent<AudioManager>().Play("WoodField");
        }

        if (stageNumber == 4)
        {
            audioGuy.GetComponent<AudioManager>().Stop("Puyo");
            audioGuy.GetComponent<AudioManager>().Stop("Egypt");
            audioGuy.GetComponent<AudioManager>().Play("BossTheme");
        }
        if (stageNumber == 5)
        {
            audioGuy.GetComponent<AudioManager>().Stop("WoodField");
            audioGuy.GetComponent<AudioManager>().Play("HermieBoss");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
