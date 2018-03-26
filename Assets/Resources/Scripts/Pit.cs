using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pit : MonoBehaviour
{
    public GameObject rock;
    Vector3 rockStartPos;

    private void Start()
    {
        rockStartPos = rock.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player2")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (collision.gameObject.tag == "Rock")
        {
            rock.transform.position = rockStartPos;
            rock.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        }
    }
}
