using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Forces : MonoBehaviour {
    private Vector3 parentForce;
    public Rigidbody rb;
    public GameObject parent;
    public string parentName;
    // Use this for initialization
    public void Start() {
        rb = GetComponent<Rigidbody>();
        LineRenderer branch = gameObject.AddComponent<LineRenderer>();
        string[] fileName = name.Split('/');
        parent = GameObject.Find(parentName);
        GetComponent<SpringJoint>().connectedBody = parent.GetComponent<Rigidbody>();
    }

	// Update is called once per frame
	public void Update () {
       
            string[] fileName = name.Split('/');
            parent = GameObject.Find(parentName);
            GetComponent<SpringJoint>().connectedBody = parent.GetComponent<Rigidbody>();
        
        LineRenderer branch = GetComponent<LineRenderer>();
        branch.SetPosition(0, parent.transform.position);
        branch.SetPosition(1, rb.position);
        parentForce = rb.position - parent.transform.position;
        rb.AddForce(parentForce*3);
        //rb.AddForce(new Vector3(0,15,0));
    }
}
