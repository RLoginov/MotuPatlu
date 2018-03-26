using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHitboxScript : MonoBehaviour {

    public int right = 0;

	// Use this for initialization
	void Start () {
        Invoke("MassDestruction", 0.1f);
	}

    

    // Update is called once per frame
    void Update () {
		
	}

    void MassDestruction()
    {
        Destroy(gameObject);
    }
}
