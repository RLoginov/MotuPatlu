using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class P1HealthManager : MonoBehaviour
{
    Image Health;
    float maxHealth = 99f;
    public static float health;

    void Start()
    {
        Health = GetComponent<Image>();
        health = maxHealth;
    }

    void Update()
    {
        Health.fillAmount = health / maxHealth;

        if (health <= 0.0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
