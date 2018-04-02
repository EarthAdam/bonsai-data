/* ========================================================================================================
 * 70:30 SmoothOrbitCam Script - created by D.Michalke / 70:30 / http://70-30.de / info@70-30.de
 * used to orbit smoothly around an object! drag and drop on your camera and drag the targed object on the target slot
 * ========================================================================================================
 */

using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

//add a menu to Component in Unity
[AddComponentMenu("Camera-Control/SmoothOrbitCam")]

public class SmoothOrbitCam : MonoBehaviour
{
	//transform to drag and drop the target object
	public Transform target;

    //useable or not, used for viewchanger
    [HideInInspector]
    public bool useable = true;

    //enable orbiting. deactivate if you need a pan-only cam (i.e. strategy games etc.)
    public bool EnableOrbiting = true;

    public enum OrbitKeyCode { MouseButton1 = 0, MouseButton2 = 1, MouseButton3 = 2, MouseButton4 = 3, MouseButton5 = 4, MouseButton6 = 5, MouseButton7 = 6, MouseButton8 = 7, Spacebar = 8 }
    public OrbitKeyCode orbitKey = OrbitKeyCode.MouseButton1;

    //add the distance variable for zooming in and out with the mouse wheel
    public float distance = 5.0f;
    private float lerpDistance = 0;

	//add speed variables for orbiting speed
	public float xSpeed = 10.0f;
	public float ySpeed = 10.0f;

	//enable or disable axes
	public bool limitToXAxis = false;
	public bool limitToYAxis = false;

	//add limits for the rotation axes
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	public float xMinLimit = -360;
	public float xMaxLimit = 360;
    private float storedLimit = 0;
	
	//add limits for the zoom distance
	public float distanceMin = 3f;
	public float distanceMax = 15f;

	//add the smoothing variable
	public float smoothTime = 2f;

	//define the rotation axes
	float rotationYAxis = 0.0f;
    float rotationXAxis = 0.0f;
    [HideInInspector]
    public Quaternion rotation;

    //define the main velocity
    [HideInInspector]
    public float velocityX = 0.0f;
    [HideInInspector]
    public float velocityY = 0.0f;

	//zoom
	public bool enableZooming = true;
	//add a modifyer for zooming in and out (for both touch and mouse)
	public float zoomSpeed = 1;

	//enable panning - for panning, the camera has to be assigned to target pan cam and the smoothorbitcam.cs script has to be on a parent object with the camera as child object!
	public bool enablePanning = false;
    [HideInInspector]
    public GameObject targetPanCam;
	public enum PanKeycode { MouseButton1 = 0, MouseButton2 = 1, MouseButton3 = 2, MouseButton4 = 3, MouseButton5 = 4, MouseButton6 = 5, MouseButton7 = 6, MouseButton8 = 7, Spacebar = 8 }
	public PanKeycode panKey = PanKeycode.MouseButton2;
	public float panSpeed = 1;
    public bool LimitPan = false;
    public Vector2 PanLimitsLeftRight;
    public Vector2 PanLimitsUpDown;

    //add offset variables to get more control over the cam
    public float xOffset;
	public float yOffset;

	//automatic orbiting
	public bool enableAutomaticOrbiting = false;
	public float orbitingSpeed = 1f;

    //for objects that might be between the target object and the cam
    public bool NoObjectsBetween = false;

    //the minimum distance the cam stays away from a possible ground (for RPGs, racing games, etc to keep the cam in a good position)
    public bool EnableGroundHovering = true;
    public float GroundHoverDistance = 5;

	//if ui should block interaction with orbit cam
	public bool UiBlocksInteraction = false;
	private bool uiBlocking = false;

	//temporary pan position and speed values
	[HideInInspector]
	public Vector3 tempPanPosition;
	[HideInInspector]
	public float velocityPanX;
	[HideInInspector]
	public float velocityPanY;

    private bool doOrbit = false;

    //get the event system
    private EventSystem eventSystem;
	
	void Awake() {

		switch(panKey) 
		{
            case PanKeycode.MouseButton1:
                EnableOrbiting = false; //usually left mouse is used for orbiting, so if you select left mouse, orbiting will be deactivated to focus on pan
                break;
		}
	}

	void Start()
	{
		//define the angle vector3 and assign the axes
		Vector3 angles = transform.eulerAngles;
		rotationYAxis = angles.y;
		rotationXAxis = angles.x;

        //distance application
	    lerpDistance = distance;
		
		// ensure the rigid body does not change rotation
		if (GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}

        //set stored limit to defined limit first
        storedLimit = yMinLimit;

        //get pan cam
        targetPanCam = GetComponentInChildren<Camera>().gameObject;

        //get eventsystem
        if (EventSystem.current != null)
        {
            eventSystem = EventSystem.current;
        }

	}

    //to get the current rotation before normal orbit cam mode is active again
    public void ResetValues()
    {
        //define the angle vector3 and assign the axes
        Vector3 angles = transform.eulerAngles;
        rotationYAxis = angles.y;
        rotationXAxis = angles.x;
        velocityX = 0f;
        velocityY = 0f;
        velocityPanX = -(tempPanPosition.x / (distance / 10));
        velocityPanY = -(tempPanPosition.y / (distance / 10));
    }

    void Update()
	{
		//check if UI is blocking
        if (eventSystem != null)
        {
            if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.IsPointerOverGameObject(0) || EventSystem.current.IsPointerOverGameObject(1) || EventSystem.current.IsPointerOverGameObject(2))
            {
                uiBlocking = true;
            }
            else
            {
                uiBlocking = false;
            }
        }
	}

    void CalcPan()
    {
        //function to calculate pan values 
        //on mouse down, calculate the orbital velocity with the given speed values and the axes
        if (UiBlocksInteraction)
        {
            if (!uiBlocking)
            {
                velocityPanX += panSpeed * Input.GetAxis("Mouse X") * 0.2f;
                velocityPanY += panSpeed * Input.GetAxis("Mouse Y") * 0.2f;
            }
        }
        else
        {
            velocityPanX += panSpeed * Input.GetAxis("Mouse X") * 0.2f;
            velocityPanY += panSpeed * Input.GetAxis("Mouse Y") * 0.2f;
        }
    }

    void CalcPanMobile(float veloDeltaX, float veloDeltaY)
    {
        if (UiBlocksInteraction)
        {
            if (!uiBlocking)
            {
                velocityPanX += veloDeltaX;
                velocityPanY += veloDeltaY;
            }
        }
        else
        {
            velocityPanX += veloDeltaX;
            velocityPanY += veloDeltaY;
        }
    }

	void LateUpdate()
	{
		//only if the target exists/is assigned, perform the orbit
		if (target)
		{
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL)
            //ORBIT

            //define keycodes for orbiting
            if (orbitKey == OrbitKeyCode.MouseButton1)
            {
                if (Input.GetMouseButton(0)) { doOrbit = true;}else{doOrbit = false;}
            }
            if (orbitKey == OrbitKeyCode.MouseButton2)
            {
                if (Input.GetMouseButton(1)) { doOrbit = true; } else { doOrbit = false; }
            }
            if (orbitKey == OrbitKeyCode.MouseButton3)
            {
                if (Input.GetMouseButton(2)) { doOrbit = true; } else { doOrbit = false; }
            }
            if (orbitKey == OrbitKeyCode.MouseButton4)
            {
                if (Input.GetMouseButton(3)) { doOrbit = true; } else { doOrbit = false; }
            }
            if (orbitKey == OrbitKeyCode.MouseButton5)
            {
                if (Input.GetMouseButton(4)) { doOrbit = true; } else { doOrbit = false; }
            }
            if (orbitKey == OrbitKeyCode.MouseButton6)
            {
                if (Input.GetMouseButton(5)) { doOrbit = true; } else { doOrbit = false; }
            }
            if (orbitKey == OrbitKeyCode.MouseButton7)
            {
                if (Input.GetMouseButton(6)) { doOrbit = true; } else { doOrbit = false; }
            }
            if (orbitKey == OrbitKeyCode.MouseButton8)
            {
                if (Input.GetMouseButton(7)) { doOrbit = true; } else { doOrbit = false; }
            }
            if (orbitKey == OrbitKeyCode.Spacebar)
            {
                if (Input.GetKey("space")) { doOrbit = true; } else { doOrbit = false; }
            }

            //for mouse/web/standalone
            if (doOrbit)
			{
				if (UiBlocksInteraction)
				{
					if (!uiBlocking)
					{
						//on mouse down, calculate the orbital velocity with the given speed values and the axes
						if (!limitToXAxis)
							velocityX += xSpeed * Input.GetAxis("Mouse X") * 0.2f;
						if (!limitToYAxis)
							velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.2f;
					}
				}
				else
				{
					//on mouse down, calculate the orbital velocity with the given speed values and the axes
					if (!limitToXAxis)
						velocityX += xSpeed * Input.GetAxis("Mouse X") * 0.2f;
					if (!limitToYAxis)
						velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.2f;
				}

			}
			
			//ZOOM
			//calculate the distance by checking the mouse scroll an clamp the value to the set distance limits
            if (enableZooming)
            {
                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * 5, distanceMin, distanceMax);
            }

#endif
#if ((UNITY_ANDROID || UNITY_IOS || UNITY_WP8 || UNITY_WP8_1) && !UNITY_EDITOR)
			//ORBIT
			if(Input.touchCount == 1)
			{
				if (UiBlocksInteraction)
				{
					if (!uiBlocking)
					{
						//on touch down calculate the velocities from the input touch position and the modifyers
						if (!limitToXAxis)
							velocityX += xSpeed * Input.GetTouch (0).deltaPosition.x * (Time.deltaTime / (Input.GetTouch(0).deltaTime+0.001f)) * 0.01f;
						if (!limitToYAxis)
							velocityY += ySpeed * Input.GetTouch (0).deltaPosition.y * (Time.deltaTime / (Input.GetTouch(0).deltaTime+0.001f)) * 0.01f;
					}
				} 
				else
				{
					//on touch down calculate the velocities from the input touch position and the modifyers
					if (!limitToXAxis)
						velocityX += xSpeed * Input.GetTouch (0).deltaPosition.x * (Time.deltaTime / (Input.GetTouch(0).deltaTime+0.001f)) * 0.01f;
					if (!limitToYAxis)
						velocityY += ySpeed * Input.GetTouch (0).deltaPosition.y * (Time.deltaTime / (Input.GetTouch(0).deltaTime+0.001f)) * 0.01f;
				}
			}
			
			//ZOOM
			//zooming with tapping / 2 finger gesture
			if (Input.touchCount == 2)
			{
				// Store both touches.
				Touch touchZero = Input.GetTouch(0);
				Touch touchOne = Input.GetTouch(1);
				
				// Find the position in the previous frame of each touch.
				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
				
				// Find the magnitude of the vector (the distance) between the touches in each frame.
				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
				
				// Find the difference in the distances between each frame.
				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
				
				//distance calculation for mobile/touch
				if (enableZooming)
				{
					if (UiBlocksInteraction)
					{
						if (!uiBlocking)
						{
							distance = Mathf.Lerp(distance,Mathf.Clamp(distance + deltaMagnitudeDiff*100000*zoomSpeed, distanceMin, distanceMax),Time.deltaTime*smoothTime*0.1f*zoomSpeed); //FIXME
						}
					}
					else
					{
						distance = Mathf.Lerp(distance,Mathf.Clamp(distance + deltaMagnitudeDiff*100000*zoomSpeed, distanceMin, distanceMax),Time.deltaTime*smoothTime*0.1f*zoomSpeed); //FIXME
					}
				}
			}
#endif

            //give the calculated values to the rotation axes
            rotationYAxis += velocityX;
			rotationXAxis -= velocityY;

            //clamp the rotation by the set limits and assign the rotation to the x axis
            if (yMinLimit != 0 || yMaxLimit != 0)
                rotationXAxis = Mathf.Clamp(rotationXAxis, yMinLimit, yMaxLimit);
            if (xMinLimit != 0 || xMaxLimit != 0)
                rotationYAxis = Mathf.Clamp(rotationYAxis, xMinLimit, xMaxLimit);

            //define the target rotation (including the calculated rotation axes)
            Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
			//give over the rotation
            if (useable && EnableOrbiting)
            {
                rotation = toRotation;
            }

            //PAN SETTINGS 
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL)
            //right mouseclick (or selected other keycode) for panning in non-mobile applications

            //define keycodes for panning
            if (panKey == PanKeycode.MouseButton1 & useable)
            {
                if (Input.GetMouseButton(0) && enablePanning)
                {
                    CalcPan();
                }
            }
            if (panKey == PanKeycode.MouseButton2 && useable)
			{
				if (Input.GetMouseButton (1) && enablePanning) 
				{
                    CalcPan();
				}
			}
			if (panKey == PanKeycode.MouseButton3 && useable)
			{
				if (Input.GetMouseButton (2) && enablePanning) 
				{
                    CalcPan();
                }
			}
            if (panKey == PanKeycode.MouseButton4 & useable)
            {
                if (Input.GetMouseButton(3) && enablePanning)
                {
                    CalcPan();
                }
            }
            if (panKey == PanKeycode.MouseButton5 & useable)
            {
                if (Input.GetMouseButton(4) && enablePanning)
                {
                    CalcPan();
                }
            }
            if (panKey == PanKeycode.MouseButton6 & useable)
            {
                if (Input.GetMouseButton(5) && enablePanning)
                {
                    CalcPan();
                }
            }
            if (panKey == PanKeycode.MouseButton7 & useable)
            {
                if (Input.GetMouseButton(6) && enablePanning)
                {
                    CalcPan();
                }
            }
            if (panKey == PanKeycode.MouseButton8 & useable)
            {
                if (Input.GetMouseButton(7) && enablePanning)
                {
                    CalcPan();
                }
            }
            if (panKey == PanKeycode.Spacebar && useable)
			{
				if (Input.GetKey("space") && enablePanning) 
				{
                    CalcPan(); 
                }
			}
#endif
#if ((UNITY_ANDROID || UNITY_IOS || UNITY_WP8 || UNITY_WP8_1) || UNITY_EDITOR)
			//for touch:
			if (Input.touchCount == 2 && enablePanning && useable && (panKey != PanKeycode.MouseButton1)) 
			{
				
				//on touch down calculate the velocities from the input touch position and the modifyers
				float tempVeloXa = panSpeed * Input.GetTouch (0).deltaPosition.x * (Time.deltaTime / (Input.GetTouch(0).deltaTime+0.001f)) * 0.05f;
				float tempVeloXb = panSpeed * Input.GetTouch (1).deltaPosition.x * (Time.deltaTime / (Input.GetTouch(0).deltaTime+0.001f)) * 0.05f;
				float tempVeloYa = panSpeed * Input.GetTouch (0).deltaPosition.y * (Time.deltaTime / (Input.GetTouch(0).deltaTime+0.001f)) * 0.05f;
				float tempVeloYb = panSpeed * Input.GetTouch (1).deltaPosition.y * (Time.deltaTime / (Input.GetTouch(0).deltaTime+0.001f)) * 0.05f;
				
				float veloDeltaX = (tempVeloXa + tempVeloXb) / 2;
				float veloDeltaY = (tempVeloYa + tempVeloYb) / 2;

                CalcPanMobile(veloDeltaX,veloDeltaY);
            }
            //if "left-mouse" is selected, disable orbiting and use touch to pan on mobile
            if (Input.touchCount == 1 && enablePanning && useable && panKey == PanKeycode.MouseButton1)
            {
                float veloDeltaX = panSpeed * Input.GetTouch(0).deltaPosition.x * (Time.deltaTime / (Input.GetTouch(0).deltaTime + 0.001f)) * 0.05f;
                float veloDeltaY = panSpeed * Input.GetTouch(0).deltaPosition.y * (Time.deltaTime / (Input.GetTouch(0).deltaTime + 0.001f)) * 0.05f;

                CalcPanMobile(veloDeltaX,veloDeltaY);
            }
#endif



            //include a raycast for potential other objects (between the target and the cam) obscuring the view

            if (NoObjectsBetween)
            {
                RaycastHit hit;
                if (Physics.Linecast(target.position, transform.position, out hit))
                {
                    float tempDistance = distance;
                    tempDistance -= hit.distance;

                    if (tempDistance < distanceMin)
                    {
                        tempDistance = distanceMin;
                    }
                    distance = Mathf.Lerp(distance, tempDistance, Time.deltaTime * smoothTime * 0.5f);
                }
            }

            //ground hovering: CURRENTLY IN DEVELOPMENT FOR MORE SMOOTHNESS
            if (EnableGroundHovering)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position,Vector3.down, out hit))
                {
                    float storedAngle = rotationXAxis;
                    if (hit.distance > GroundHoverDistance)
                    {

                        yMinLimit = storedLimit;
                    }
                    if (hit.distance < GroundHoverDistance)
                    {
                        yMinLimit = storedAngle;
                        rotationXAxis = storedAngle;
                    }

                }
            }


            //set the temporary position
            if (target != null)
            {
                //lerp the distance
                lerpDistance = Mathf.Lerp(lerpDistance, distance, smoothTime * Time.deltaTime);

                //create the inverted distance to move the cam away from the object
                Vector3 negDistance = new Vector3(0.0f, 0.0f, -lerpDistance);

                Vector3 position = rotation * negDistance + target.position;
                //create yet another vec3 to include the defined offset
                Vector3 offsetPosition = new Vector3(position.x + xOffset, position.y + yOffset, position.z);

                //finaly set the transform by giving over the temporary position/rotation to the object transform
                transform.rotation = rotation;
                transform.position = offsetPosition;
            }

			//orbiting mode
			if (enableAutomaticOrbiting == true)
			{
				velocityX = Mathf.Lerp(velocityX,orbitingSpeed, Time.deltaTime * smoothTime);
			}

            //assign the smoothing effect to the velocity with lerp
            velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
            velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);

            //panning
            tempPanPosition = new Vector3(-velocityPanX * distance / 10, -velocityPanY * distance / 10, 0);

            //apply panning
            if (targetPanCam != null && useable && enablePanning)
            {
                targetPanCam.transform.localPosition = Vector3.Lerp(targetPanCam.transform.localPosition, tempPanPosition, Time.deltaTime * smoothTime * 1.5f);
                //pan limitations
                if (LimitPan) 
                {
                    float clampX = Mathf.Clamp(targetPanCam.transform.localPosition.x, PanLimitsLeftRight.x, PanLimitsLeftRight.y);
                    float clampY = Mathf.Clamp(targetPanCam.transform.localPosition.y, PanLimitsUpDown.x, PanLimitsUpDown.y);
                    targetPanCam.transform.localPosition = new Vector3(clampX, clampY, targetPanCam.transform.localPosition.z);
                }
            }
		}

	}
}
