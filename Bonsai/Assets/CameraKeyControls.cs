using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CameraKeyControls : MonoBehaviour {

    public float speed = 5.0f;
  public GameObject cam;
  // Use this for initialization
  private void Start()
  {
  }
  void FixedUpdate () {
    // BACKWARDS
    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
    {
      transform.Translate(cam.transform.forward.x * -0.1f, 0, cam.transform.forward.z * -0.1f);
    }
    if ((Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift)) || (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftShift)))
    {
      transform.Translate(cam.transform.forward.x * -1f, 0, cam.transform.forward.z * -1f);
    }
    //  FORWARDS
    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
    {
      transform.Translate(cam.transform.forward.x * 0.1f, 0, cam.transform.forward.z * 0.1f);
    }
    if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)) || (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftShift)))
    {
      transform.Translate(cam.transform.forward.x * 1f, 0, cam.transform.forward.z * 1f);
    }
    //  UP
    if (Input.GetKey(KeyCode.E) )
    {
      transform.Translate(0,0.1f, 0);
    }
    if (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.LeftShift))
      {
      transform.Translate(0,1f, 0);
    }
    //  DOWN
    if (Input.GetKey(KeyCode.Q))
    {
      transform.Translate(0, -0.1f, 0);
    }
    if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift))
    {
      transform.Translate(0, -1f, 0);
    }
  }
}
