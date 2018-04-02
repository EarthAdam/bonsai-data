using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class NavBasic : MonoBehaviour
{
    public float speed = 5.0f;
    private LineRenderer lineRenderer;
    public Rigidbody NaviBase;
    public Vector3 ThrustDirection;
    public float ThrustForce;
    public bool ShowTrustMockup = true;
    public GameObject ThrustMockup;
    public Rigidbody Plane;
    public Transform Earth;
    public Transform cam;


    SteamVR_TrackedObject trackedObj;
    FixedJoint joint;
    GameObject attachedObject;
    Vector3 tempVector;
    Vector3 tempVector2;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        lineRenderer = GetComponent<LineRenderer>();

    }

    void FixedUpdate()
    {

        var device = SteamVR_Controller.Input((int)trackedObj.index);
        lineRenderer = GetComponent<LineRenderer>();

        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            tempVector = Quaternion.Euler(ThrustDirection) * Vector3.forward;
            NaviBase.AddForce(transform.rotation * tempVector * ThrustForce);
            NaviBase.maxAngularVelocity = 2f;
        }

        int rightIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
        int leftIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);

        SteamVR_Controller.Device rightDevice = SteamVR_Controller.Input(rightIndex);
        SteamVR_Controller.Device leftDevice = SteamVR_Controller.Input(leftIndex);


        if (leftDevice.GetTouch(SteamVR_Controller.ButtonMask.Grip))
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
            Plane.AddForce(new Vector3(0f, -1f, 0f) * ThrustForce);
            NaviBase.AddForce(new Vector3(0f, -1f, 0f) * ThrustForce  / 33.333f);

        }
        if (rightDevice.GetTouch(SteamVR_Controller.ButtonMask.Grip))
        {
            Plane.AddForce(new Vector3(0f, 1f, 0f) * ThrustForce);
            NaviBase.AddForce(new Vector3(0f, 1f, 0f) * ThrustForce  / 33.333f);
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));

        }


        // show trust mockup
        if (ShowTrustMockup && ThrustMockup != null)
        {
            if (attachedObject == null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                attachedObject = (GameObject)GameObject.Instantiate(ThrustMockup, Vector3.zero, Quaternion.identity);
                attachedObject.transform.SetParent(this.transform, false);
                attachedObject.transform.Rotate(ThrustDirection);
            }
            else if (attachedObject != null && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                Destroy(attachedObject);
            }
        }
    }
}
