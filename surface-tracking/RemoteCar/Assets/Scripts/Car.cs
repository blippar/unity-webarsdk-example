using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float acceleration;
    public float maxSpeed;
    public float turnSpeed;
    private float curSpeed;

    public bool doAccelerate;

    private Rigidbody rig;

    // instance
    public static Car instance;

    void Awake ()
    {
        instance = this;
        rig = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        if(doAccelerate)
        {
            curSpeed = Mathf.Clamp(curSpeed + (Time.deltaTime * acceleration), 0.0f, maxSpeed);
        }
        else
        {
            curSpeed = Mathf.Clamp(curSpeed - (Time.deltaTime * acceleration), 0.0f, maxSpeed);
        }

        rig.velocity = transform.forward * curSpeed;
    }

    public void Turn (float rate)
    {
        transform.Rotate(Vector3.up, rate * turnSpeed * Time.deltaTime);
    }
}