using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class Ring3Renderer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        for (int i = 0; i <= 360; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(18 + 25 * Mathf.Sin(i * Mathf.PI / 180), 30, -11.5f - 25 * Mathf.Cos(i * Mathf.PI / 180)));
        }
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
    }
    void Update()
    {

    }
}