using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonsaiBranch : MonoBehaviour {

    private LineRenderer lineRenderer;
    public Vector3 start;
    public Vector3 stop;
    public float width;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        //lineRenderer.SetVertexCount(stop - start);
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, stop);
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    void Update()
    {

    }
}
