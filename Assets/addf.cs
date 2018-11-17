using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addf : MonoBehaviour
{
    private Rigidbody rb;
	// Use this for initialization
	void Start ()
	{
	    rb = transform.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        //float h = Input.GetAxis("Mouse X");
        //float v = Input.GetAxis("Mouse Y");
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //Debug.Log(h);
        if (Input.GetMouseButton(0))
        {
            Debug.Log(h + " " + v);
	        rb.velocity = new Vector3(h , v, 0) * 10f;
        }
	}
}
