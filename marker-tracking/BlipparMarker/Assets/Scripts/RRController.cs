using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RRController : MonoBehaviour {
    public static RRController Instance;
    public GameObject animobj;
    public RuntimeAnimatorController RRAnim;
    public RuntimeAnimatorController ReverseAnim;
    public GameObject hotspot1;
    public GameObject home;
    public GameObject back;
    public GameObject Txt;
    public GameObject close;

    public GameObject marker1, marker2;
    public GameObject canvas;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Use this for initialization
    void Start () {
        home.SetActive(true);
        back.SetActive(false);
        canvas.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (marker1.activeSelf || marker2.activeSelf)
        {
            canvas.SetActive(true);
        }
        else if (!marker1.activeSelf || !marker2.activeSelf)
        {
            canvas.SetActive(false);
            stopanim();
        }
	}

    public void PlayAnim()
    {
        Debug.Log("clicked........................");
        hotspot1.SetActive(false);
        animobj.GetComponent<Animator>().runtimeAnimatorController = RRAnim;
        Txt.GetComponent<Text>().text = "Disassemble Engine";
        home.SetActive(false);
        back.SetActive(true);
        close.SetActive(true);
    }

    public void IdealAnim()
    {
        close.SetActive(false);
        animobj.SetActive(true);
        hotspot1.SetActive(false);
        animobj.SetActive(true);
        animobj.GetComponent<Animator>().runtimeAnimatorController = ReverseAnim;
        Txt.GetComponent<Text>().text = "Assemble Engine";
        home.SetActive(false);
        back.SetActive(true);
    }

    public void stopanim()
    {
        animobj.GetComponent<Animator>().runtimeAnimatorController = null;
        animobj.SetActive(true);
        hotspot1.SetActive(true);
        if(back.activeSelf)
        {
            home.SetActive(true);
            back.SetActive(false);
        }
        Txt.GetComponent<Text>().text = "Home";
        close.SetActive(false);
    }

    private void OnDisable()
    {
        canvas.SetActive(false);
        stopanim();
    }
}
