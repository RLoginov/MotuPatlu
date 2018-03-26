using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearScript : MonoBehaviour {

    public int fadeBuffer = -1;
    private int fadeBufferCounter = 0;
    public float startScale = 1f;
    public float currentAlpha = 1f;
    private int currentFrame = 0;
    public float biggerFactor = 1f;
    public float maxVertical = 0f;
    public float maxHorizontal = 0f;
    private float horizontalSpeed;
    private float verticalSpeed;
    public float rotateSpeed = 0f;

    // Use this for initialization
    void Start () {
        transform.localScale = new Vector3(startScale, startScale, 0);

        verticalSpeed = Random.Range(0, maxVertical);
        horizontalSpeed = Random.Range(-maxHorizontal, maxHorizontal);
        rotateSpeed = Random.Range(0, rotateSpeed);
    }
	
	// Update is called once per frame
	void Update () {
        transform.localScale += new Vector3(biggerFactor, biggerFactor, 0);

        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.z + rotateSpeed);
        transform.position += new Vector3(horizontalSpeed, verticalSpeed, 0);

        if (fadeBufferCounter < fadeBuffer)
        {
            fadeBufferCounter++;
            return;
        }

        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, currentAlpha);
        currentAlpha = currentAlpha - 0.05f;

        if (currentFrame > 30)
        {
            Destroy(gameObject);
        }

        currentFrame++;
    }
}
