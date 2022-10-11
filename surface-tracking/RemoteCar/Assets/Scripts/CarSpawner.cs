using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject car;
    public GameObject placementIndicator;
    public GameObject carControls;
    public GameObject placeCarButton;

    void Start ()
    {
        //car.SetActive(false);
        //carControls.SetActive(false);
        //placeCarButton.SetActive(true);
        //placementIndicator.SetActive(true);

        car.SetActive(true);

        carControls.SetActive(true);
    }

    public void OnPlaceCarButton ()
    {
        //car.SetActive(true);
        //car.transform.position = placementIndicator.transform.position;

        //carControls.SetActive(true);
        //placeCarButton.SetActive(false);
        //placementIndicator.SetActive(false);
    }
}