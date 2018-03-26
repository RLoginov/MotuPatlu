using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Vector3 pos;

    public float zoomSpeed = 15;
    public float targetOrtho;
    public float smoothSpeed = 5.0f;
    public float minOrtho = 1.0f;
    public float maxOrtho = 100.0f;
    private int waitWin = 0;
    private GameObject audioGuy;
    public int player = 1;
    private int cameraType = 0;
    public int noVerticalFollow = 0;

    public float leftBoundary = -20f;
    public float rightBoundary = 9999f;

    // Use this for initialization
    void Start () {
        audioGuy = GameObject.Find("AudioManager");
        pos = transform.position;

        targetOrtho = Camera.main.orthographicSize;


        if (GameObject.Find("Number2") != null)
        {
            cameraType = 1;
        }
        if (GameObject.Find("House") != null)
        {
            cameraType = 2;
        }
        if (GameObject.Find("John") != null)
        {
            cameraType = 3;
        }
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 player1 = transform.position;
        Vector3 player2 = transform.position;

        player1 = GameObject.FindGameObjectWithTag("Player").transform.position;
        player2 = GameObject.FindGameObjectWithTag("Player2").transform.position;

        pos.x = player1.x + (player2.x - player1.x) / 5;
        pos.y = player1.y + (player2.y - player1.y) / 5;

        if (noVerticalFollow == 0)
        {
            if (player == 1)
            {
                transform.position = new Vector3(player1.x, player1.y + 3, -10); ;
            }
            else
            {
                transform.position = new Vector3(player2.x, player2.y + 3, -10);
            }
        }
        else
        {
            if (player == 1)
            {
                transform.position = new Vector3(player1.x, transform.position.y, -10); ;
            }
            else
            {
                transform.position = new Vector3(player2.x, transform.position.y, -10);
            }
        }
        


        float playerDistance;
        playerDistance = Vector3.Distance(player1, player2);


        targetOrtho = 12;
        if (cameraType == 3)
        {
            targetOrtho = 8;

            GameObject boss3 = GameObject.Find("John");
            if (boss3.GetComponent<JohnScript>().phase == 7)
            {
                targetOrtho = 3;
                if (player == 1)
                {
                    transform.position = new Vector3(player1.x, player1.y, -10); ;
                }
                else
                {
                    transform.position = new Vector3(player2.x, player2.y, -10);
                }
            }
            else
            {
                transform.position = new Vector3(0, 2, -10);
            }

            targetOrtho = Mathf.Clamp(targetOrtho, 3, maxOrtho);
        }

        if (cameraType == 2)
        {
            targetOrtho = 8;

            GameObject boss2 = GameObject.Find("House");
            if (boss2.GetComponent<HouseScript>().phase == 7)
            {
                targetOrtho = 3;
                if (player == 1)
                {
                    transform.position = new Vector3(player1.x, player1.y, -10); ;
                }
                else
                {
                    transform.position = new Vector3(player2.x, player2.y, -10);
                }
            }
            else
            {
                transform.position = new Vector3(0, 2, -10);
            }

            targetOrtho = Mathf.Clamp(targetOrtho, 3, maxOrtho);
        }


        if (cameraType == 1)
        {
            targetOrtho = 8;

            GameObject boss = GameObject.Find("Number2");
            if (boss.GetComponent<Number2Script>().phase == 7)
            {
                targetOrtho = 3;
                if (player == 1)
                {
                    transform.position = new Vector3(player1.x, player1.y, -10); ;
                }
                else
                {
                    transform.position = new Vector3(player2.x, player2.y, -10);
                }
            }
            else
            {
                transform.position = new Vector3(0, 2, -10);
            }
            
            targetOrtho = Mathf.Clamp(targetOrtho, 3, maxOrtho);
        }
        if (cameraType == 0)
        {
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        }
        
        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);




        if (transform.position.x < leftBoundary)
        {
            transform.position = new Vector3(leftBoundary, transform.position.y, transform.position.z);
        }
        if (transform.position.x > rightBoundary)
        {
            transform.position = new Vector3(rightBoundary, transform.position.y, transform.position.z);
        }
    }
}
