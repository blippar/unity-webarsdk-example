using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour {
    public static Controller Instance;
    public GameObject animobj;
    public RuntimeAnimatorController Fuel;
    public RuntimeAnimatorController Nozzle;
    public GameObject hotspot1;
    public GameObject hotspot2;
    public GameObject home;
    public GameObject back;
    public GameObject Txt;
    public bool flag1;
    public bool flag2;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Use this for initialization
    void Start () {
        animobj.SetActive(false);
        home.SetActive(true);
        back.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void nozzleplay()
    {
        //close.SetActive(true);
        animobj.SetActive(true);
        hotspot1.SetActive(false);
        hotspot2.SetActive(false);
        animobj.GetComponent<Animator>().runtimeAnimatorController = Nozzle;
        Txt.GetComponent<Text>().text = "Missing Fuel Injector Nozzle";
        home.SetActive(false);
        back.SetActive(true);
    }

    public void Fuelplay()
    {
        //close.SetActive(true);
        animobj.SetActive(true);
        hotspot1.SetActive(false);
        hotspot2.SetActive(false);
        animobj.SetActive(true);
        animobj.GetComponent<Animator>().runtimeAnimatorController = Fuel;
        Txt.GetComponent<Text>().text = "Engine Rear Mount";
        home.SetActive(false);
        back.SetActive(true);
    }

    public void stopanim()
    {
        if (gameObject.activeInHierarchy)
        {
            animobj.GetComponent<Animator>().runtimeAnimatorController = null;
            animobj.SetActive(false);
            hotspot1.SetActive(true);
            hotspot2.SetActive(true);
            if (back.activeSelf)
            {
                home.SetActive(true);
                back.SetActive(false);
            }
            Txt.GetComponent<Text>().text = "Home";
            AnimationEvent.Instance.Resetshaders();
        }
        
    }  

}
