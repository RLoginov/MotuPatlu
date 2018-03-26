using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button4SpawnBlockRock : MonoBehaviour
{
    public GameObject spawner1;
    public GameObject spawner2;
    bool spawned1;
    int spawnCounter;
    public int maxSpawns;

    private void Start()
    {
        spawnCounter = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player2" ||
            collision.gameObject.tag == "Rock")
        {
            if (spawnCounter < maxSpawns)
            {
                GameObject blockRock1 = (GameObject)Instantiate(Resources.Load("Prefabs/blockRock"));
                blockRock1.transform.position = spawner1.transform.position;
                spawnCounter = spawnCounter + 1;

                GameObject blockRock2 = (GameObject)Instantiate(Resources.Load("Prefabs/blockRock"));
                blockRock2.transform.position = spawner2.transform.position;
                spawnCounter = spawnCounter + 1;
            }
        }
        
    }
}
