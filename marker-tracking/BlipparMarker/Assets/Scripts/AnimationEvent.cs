using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour {
    public static AnimationEvent Instance;
    public Material shadergold;
    public Material shaderhologram;
    public GameObject[] nozzlegold1;
    public GameObject[] nozzlegold2;
    public GameObject[] nozzlegold3;
    public GameObject[] enginegold;
    // Use this for initialization
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
  
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void changeshadersnozzle()
    {
        foreach (GameObject obj in nozzlegold1)
        {
            obj.GetComponent<Renderer>().material = shaderhologram;
        }
    }

    public void changeshadersnozzle1()
    {
        foreach (GameObject obj in nozzlegold1)
        {
            obj.GetComponent<Renderer>().material = shadergold;
        }

        foreach (GameObject obj in nozzlegold2)
        {
            obj.GetComponent<Renderer>().material = shaderhologram;
        }
    }

    public void changeshadersnozzle2()
    {
        foreach (GameObject obj in nozzlegold2)
        {
            obj.GetComponent<Renderer>().material = shadergold;
        }

        foreach (GameObject obj in nozzlegold3)
        {
            obj.GetComponent<Renderer>().material = shaderhologram;
        }
    }

    public void changeshadersnozzlereset()
    {
        foreach (GameObject obj in nozzlegold1)
        {
            obj.GetComponent<Renderer>().material = shadergold;
        }

        foreach (GameObject obj in nozzlegold2)
        {
            obj.GetComponent<Renderer>().material = shadergold;
        }
    }

    public void changeshaderenginereset()
    {
        foreach (GameObject obj in enginegold)
        {
            obj.GetComponent<Renderer>().material = shadergold;
        }
    }

    public void changeshaderengine()
    {
        foreach (GameObject obj in enginegold)
        {
            obj.GetComponent<Renderer>().material = shaderhologram;
        }

    }

    public void Resetshaders()
    {
        foreach (GameObject obj in nozzlegold1)
        {
            obj.GetComponent<Renderer>().material = shadergold;
        }

        foreach (GameObject obj in nozzlegold2)
        {
            obj.GetComponent<Renderer>().material = shadergold;
        }
        foreach (GameObject obj in nozzlegold3)
        {
            obj.GetComponent<Renderer>().material = shadergold;
        }
        foreach (GameObject obj in enginegold)
        {
            obj.GetComponent<Renderer>().material = shadergold;
        }
    }

}
