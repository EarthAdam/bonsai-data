using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Forces : MonoBehaviour {
    public Rigidbody parent;
    private Vector3 parentForce;
    public Rigidbody rb;
    // Use this for initialization
    public void Start() {
        rb = GetComponent<Rigidbody>();
        LineRenderer branch = gameObject.AddComponent<LineRenderer>();
    }

	// Update is called once per frame
	public void Update () {
        LineRenderer branch = GetComponent<LineRenderer>();
        branch.SetPosition(0, parent.position);
        branch.SetPosition(1, rb.position);
        parentForce = rb.position - parent.position;
        rb.AddForce(parentForce*3);
        //rb.AddForce(new Vector3(0,15,0));
    }
}
