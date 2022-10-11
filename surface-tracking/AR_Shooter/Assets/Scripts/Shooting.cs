using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject projectilePrefab;

    public float projectileSpeed;
    public float shootRate;
    private float lastShootTime;

    public GameObject core;
    public GameObject placeCoreButton;
    public AudioSource gunAudio;

    private PlacementIndicator placementIndicator;

    private Camera cam;

    public static Shooting instance;
    void Awake () { instance = this; }

    void Start ()
    {
        cam = Camera.main;
        //core.SetActive(false);

        placementIndicator = FindObjectOfType<PlacementIndicator>();
    }

    void Update ()
    {
        // shoot a projectile when we touch the screen
        if(Input.touchCount > 0)
        {
            if(Time.time - lastShootTime >= shootRate)
                Shoot();
        }
    }

    // shoots a new projectile out from the camera
    void Shoot ()
    {
        lastShootTime = Time.time;

        GameObject proj = Instantiate(projectilePrefab, cam.transform.position, Quaternion.identity);

        proj.GetComponent<Rigidbody>().velocity = cam.transform.forward * projectileSpeed;
        Destroy(proj, 3.0f);
        gunAudio.Play();
    }

    // called when the "Place Core" button is pressed
    // places down the core and begins the game
    public void PlaceCore ()
    {
        core.SetActive(true);
        //core.transform.position = placementIndicator.transform.position;
        //placementIndicator.gameObject.SetActive(false);
        placeCoreButton.SetActive(false);
        EnemySpawner.instance.canSpawnEnemies = true;
    }
}