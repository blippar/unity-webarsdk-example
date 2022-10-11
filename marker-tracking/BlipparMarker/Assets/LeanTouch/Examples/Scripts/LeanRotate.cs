using UnityEngine;
using System;
using System.Collections;


// This script allows you to transform the current GameObject
public class LeanRotate : MonoBehaviour
{
    public static LeanRotate Instance;
    public Transform myObj;
    public float perspectiveZoomSpeed = 0.25f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.25f;        // The rate of change of the orthographic size in orthographic mode.
    public Texture btnTexture;
    private Quaternion originalRotationValue;
    private float cameraField;
    private bool cameraFieldFlag = false;
    float native_width = 1920f;
    float native_height = 1080f;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        originalRotationValue = myObj.transform.rotation;

    }
    void Update()
    {
        //if (PlayAnim.instance.animationplaying)
        //{
            if (Input.touchCount == 1)
            {
                // GET TOUCH 0
                Touch touch0 = Input.GetTouch(0);

                // APPLY ROTATION
                if (touch0.phase == TouchPhase.Moved)
                {
                    //To rotate object
                    myObj.transform.Rotate(0f, -touch0.deltaPosition.x, 0f);
                    //to Position Object
                    if (cameraFieldFlag)
                        myObj.transform.Translate(0f, Mathf.Clamp(touch0.deltaPosition.y * 0.03f, -0.3f, 0.3f), 0f);
                }

                // APPLY SCALE
                //if (touch0.phase == TouchPhase.Stationary) 
            }
        //}

    }

}
