using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonsaiLabel : MonoBehaviour
{
    public Transform TextObject;

    public Vector3 start;
    public Vector3 stop;
    // Use this for initialization
    void Start()
    {
        //Vector3 part1 = stop - start;
        //Vector3 part2 = part1 / 2;
        //Vector3 part3 = start + part2;
        transform.position = stop + new Vector3(0,0, 0);
    }
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

}
