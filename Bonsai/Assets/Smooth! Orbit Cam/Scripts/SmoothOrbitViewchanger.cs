using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
/* ========================================================================================================
 * 70:30 SmoothOrbitViewchanger Script - created by D.Michalke / 70:30 / http://70-30.de / info@70-30.de
 * used to trigger a camera perspective change for Smooth!
 * possible triggers: by click/touch on object, UI element with this script on it - OR - by calling public function TriggerViewchange()
 * ========================================================================================================
 */
public class SmoothOrbitViewchanger : MonoBehaviour, IPointerUpHandler {

    //the data for the viewchange: enter in inspector
    public Vector3 Rotation;

    private Quaternion RotaQuat;
    public float Distance;

    public Vector2 PanValues;


    //speed
    public float speed = 1;

    //to get the camera control script
    private SmoothOrbitCam smoothOrbitCam;

    //movement bool
    private bool moving = false;

	void Start ()
    {
        //get camera system
        smoothOrbitCam = FindObjectOfType<SmoothOrbitCam>().gameObject.GetComponent<SmoothOrbitCam>();
        RotaQuat.eulerAngles = Rotation;

        //apply speed
        speed = speed / 10;
	}
	

	void Update ()
    {
        if (moving)
        {
            //get origin values//lerp to target values
            Quaternion rot = Quaternion.Lerp(smoothOrbitCam.transform.rotation,RotaQuat, speed);
            float dis = Mathf.Lerp(smoothOrbitCam.distance, Distance,speed);
            Vector3 pan = Vector3.Lerp(smoothOrbitCam.targetPanCam.transform.localPosition,new Vector3(PanValues.x,PanValues.y,0), speed);
            rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, 0);

            smoothOrbitCam.rotation = rot;
            smoothOrbitCam.distance = dis;
            smoothOrbitCam.targetPanCam.transform.localPosition = pan;
        }
	}

    public void OnPointerUp(PointerEventData e)
    {
        StartCoroutine(ViewChange());
    }

    void OnMouseUp()
    {
        StartCoroutine(ViewChange());
    }

    public void TriggerViewChange() //if the viewchange should be called from code somewhere
    {
        StartCoroutine(ViewChange());
    }

    private IEnumerator ViewChange()
    {
        //clean existing cam system values
        //smoothOrbitCam.ResetValues();

        //perform
        moving = true;
        smoothOrbitCam.useable = false;

        //wait for the movement to finish
        yield return new WaitForSeconds(1.2f);

        //stop performing
        moving = false;
        smoothOrbitCam.useable = true;

        //overwrite existing values
        //avoid reset of the pan value after viewchange
        smoothOrbitCam.tempPanPosition = PanValues;
        //clean values again to give them free for the normal controls again
        smoothOrbitCam.ResetValues();
    }
}
