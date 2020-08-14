using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCode : MonoBehaviour
{
    Rigidbody rigidbody;
    float speed = 1;
    float acelleration = 1;
    // Start is called before the first frame update

    public void SetSpeed(float s,float a)
    {
        speed = s;
        acelleration = a;
    }
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = transform.forward*speed;
        speed = speed +(acelleration*Time.deltaTime);
    }
}
