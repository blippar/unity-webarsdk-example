using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShooterCore : MonoBehaviour
{
    public int health;
    public float defendTime;
    private float curDefendTime;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI defendTimeText;

    public GameObject winScreen;
    public GameObject loseScreen;

    public bool gameOver, targetfound= false;

    private Camera cam;

    // instance
    public static ShooterCore instance;
    void Awake () { instance = this; }

    void Start ()
    {
        //healthText.text = "Health: " + health;
        cam = Camera.main;
        curDefendTime = defendTime;
        //Shooting.instance.PlaceCore();
    }


    public void TargetFound()
    {
        healthText.text = "Health: " + health;
        Shooting.instance.PlaceCore();
        targetfound = true;
    }
    void Update ()
    {
        // rotate the health text to look at the camera
        healthText.transform.rotation = Quaternion.LookRotation(healthText.transform.position - cam.transform.position);

        if(gameOver)
            return;

        // update defend time text and current time
        if (!targetfound)
            return;

        defendTimeText.text = "Defend for " + Mathf.RoundToInt(curDefendTime) + "s";
        curDefendTime -= Time.deltaTime;

        if(curDefendTime <= 0.0f)
        {
            WinGame();
        }
    }

    // called when an enemy hits the core
    public void TakeDamage (int damage)
    {
        health -= damage;

        healthText.text = "Health: " + health;

        if(health <= 0)
            GameOver();
    }

    // called when the defend time runs out
    void WinGame ()
    {
        gameOver = true;
        winScreen.SetActive(true);
        EnemySpawner.instance.canSpawnEnemies = false;
    }

    // called when our health reaches 0
    void GameOver ()
    {
        gameOver = true;
        loseScreen.SetActive(true);
        EnemySpawner.instance.canSpawnEnemies = false;
    }

    // called when the "Restart" button is pressed
    public void RestartGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}