using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.IO;

public class PieLineRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public float start;
    public float stop;
    public float range;

    public Vector3[] coordinates;
    public float radius = 0.1f;
    void Start()
    {
        range = start + (stop - start) / 2;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        for (int i = (int)start; i <= (int)start + (int)range; i +=3)
        {
            lineRenderer.SetPosition(i,new Vector3(0, 8, 0) + new Vector3(18.0f, 0, -11.5f));
            lineRenderer.SetPosition(i+1,new Vector3(5*Mathf.Cos(i*Mathf.PI/180),8, 5 * Mathf.Sin(start * Mathf.PI / 180)) + new Vector3(18.0f, 0, -11.5f));
            lineRenderer.SetPosition(i+2,new Vector3(0, 8, 0) + new Vector3(18.0f, 0, -11.5f));
        }
        //lineRenderer.startWidth = 0.05f;
        //lineRenderer.endWidth = 0.05f;
    }
    void Update()
    {
       
    }
}
