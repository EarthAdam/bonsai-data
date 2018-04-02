using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterPlot : MonoBehaviour {
    private Vector3[] scatterData = new[] {
        new Vector3(0.5f,20,0),
        new Vector3(0.83f,12,0),
        new Vector3(0.2864f,19.2657f,0),
        new Vector3(0.1008f,19.1871f,0),
        new Vector3(0.8117f,7.9296f,0),
        new Vector3(0.3146f,0.7414f,0),
        new Vector3(0.7435f,9.4884f,0),
        new Vector3(0.7848f,8.5324f,0),
        new Vector3(0.7877f,6.9044f,0),
        new Vector3(0.6522f,3.0646f,0),
        new Vector3(0.7593f,11.3906f,0),
        new Vector3(0.532f,11.823f,0),
        new Vector3(0.606f,5.3875f,0),
        new Vector3(0.811f,2.1973f,0),
        new Vector3(0.7542f,7.7854f,0),
        new Vector3(0.6745f,10.685f,0),
        new Vector3(0.8208f,9.3401f,0),
        new Vector3(0.6838f,5.1258f,0),
        new Vector3(0.7841f, 10.7949f, 0) }; 
        
    // Use this for initialization
	void Start () {
        for (int i= 0;i<= 19; i++){
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = Vector3.Scale(scatterData[i],new Vector3(10, 1, 1) );
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
