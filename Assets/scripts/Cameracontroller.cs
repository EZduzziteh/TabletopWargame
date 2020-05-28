using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameracontroller : MonoBehaviour
{

    public float horizontalspeed;
    public float forwardspeed;
    public float verticalspeed;
    public float rotatespeed;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame


    private void Update()
    {
        
    }
    void FixedUpdate()
    {
       // Debug.Log(Input.GetAxis("Horizontal"));
      //  Debug.Log(Input.GetAxis("Vertical"));
        rb.AddForce(transform.right *Input.GetAxis("Horizontal")* horizontalspeed*Time.deltaTime);
        rb.AddForce(transform.up * Input.GetAxis("Vertical") * verticalspeed*Time.deltaTime);
        rb.AddForce(transform.forward * Input.GetAxis("Forward") * forwardspeed* Time.deltaTime);
        rb.AddRelativeTorque(Vector3.up * Input.GetAxis("Rotate")*rotatespeed);

    }
}
