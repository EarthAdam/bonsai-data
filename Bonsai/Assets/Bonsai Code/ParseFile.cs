using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;

public class ParseFile : MonoBehaviour {
    private int maxLevels = 0;
    public float desiredConnectedNodeDistance = 1;
    public float connectedNodeForce = 1;
    public float disconnectedNodeForce = 1;
    public List<Node> nodes;
    // Use this for initialization
    public void Start () {
        nodes = new List<Node>();
        ParseTxtFile();
    }
    // Update is called once per frame
    void Update()
    {
        ApplyGraphForce();
        foreach (var node in nodes)
        {
            node.position += node.velocity * Time.deltaTime;
        }
    }
    void ParseTxtFile()
    {
        string text = File.ReadAllText("../docs/output.txt");
        char[] separators = { ',', ';', '|', '\n' };
        string[] strValues = text.Split(separators);
        int[] sizes = new int[strValues.Length];
        List<string> folders = new List<string>();
        for (int i = 0; i < strValues.Length; i++)
        {
            string[] lineValues = strValues[i].Split('\t');                 //Break up string by tab separators
            sizes[i] = Convert.ToInt32(lineValues[0]);                      //Save file size
            string[] fileString = lineValues[1].Split('/');
            string[] parentFolder = new string[fileString.Length];          //Creates an array for the parent folder
            Array.Copy(fileString, parentFolder, fileString.Length - 1);
            //break apart file path by '/' marks
            if (fileString.Length == 1)
            {
                nodes.Add(new Node()
                {
                    pathName = string.Join("", fileString),
                    position = new Vector3(0, 0, 0),
                    velocity = Vector3.zero
                });
            }
            else
            {
                //string[] parentFolder = new string[fileString.Length];          //Creates an array for the parent folder
                Array.Copy(fileString, parentFolder, fileString.Length - 1);    //Copies everything before the last '/' in the file path
                nodes.Add(new Node()
                {
                    pathName = string.Join("", fileString),
                    parentName = string.Join("", parentFolder),
                    position = UnityEngine.Random.insideUnitSphere * 10 + new Vector3(0, 5, 0),
                    velocity = Vector3.zero,
                    //children = string.Join("", parentFolder).ToList()
                });
                Debug.Log(nodes[i].pathName+','+nodes[i].children);
            }
        }
        /*
        foreach(var node in nodes)
        {
            node.children = nodes.Where(node.pathName == node.parentName);
        }
        for (int i = 0; i < strValues.Length; i++)
        {
            string[] fileString = nodes[i].pathName.Split('/');                 //break apart file path by '/' marks
            string[] parentFolder = new string[fileString.Length];          //Creates an array for the parent folder
            Array.Copy(fileString, parentFolder, fileString.Length - 1);
            foreach(var node in nodes)
            {
                if (nodes[i].pathName == node.parentName)
                    nodes[i].children = node.parentName;// s.Where(node => node.pathName == string.Join("", parentFolder)).ToList();
            }
        }*/
    }
    private void ApplyGraphForce()
    {
        foreach (var node in nodes)
        {
            var disconnectedNodes = nodes.Except(node.children);
            foreach (var connectedNode in node.children)
            {
                var difference = node.position - connectedNode.position;
                var distance = (difference).magnitude;
                var appliedForce = connectedNodeForce * Mathf.Log10(distance / desiredConnectedNodeDistance);
                connectedNode.velocity += appliedForce * Time.deltaTime * difference.normalized;
            }
            foreach (var disconnectedNode in disconnectedNodes)
            {
                var difference = node.position - disconnectedNode.position;
                var distance = (difference).magnitude;
                if (distance != 0)
                {
                    var appliedForce = -disconnectedNodeForce / Mathf.Pow(distance, 2);
                    disconnectedNode.velocity += appliedForce * Time.deltaTime * difference.normalized;
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        foreach (var node in nodes)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(node.position, 0.125f);
            Gizmos.color = Color.green;
            foreach (var connectedNode in node.children)
            {
                Gizmos.DrawLine(node.position, connectedNode.position);
            }
        }
    }
}
public class Node
{
    public string pathName;
    public string parentName;
    public Vector3 position;
    public Vector3 velocity;
    public List<Node> children;

}
