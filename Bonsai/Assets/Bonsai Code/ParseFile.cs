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
            string[] fileString = lineValues[1].Split('/');                 //break apart file path by '/' marks
            string[] parentFolder = new string[fileString.Length];          //Creates an array for the parent folder
            Array.Copy(fileString, parentFolder, fileString.Length - 1);    //Copies everything before the last '/' in the file path
            folders.Add(string.Join("", parentFolder));
            nodes.Add(new Node()
            {
                pathName = string.Join("", fileString),
                children = nodes.Where(node => node.pathName == string.Join("", parentFolder)).ToList(),
                position = UnityEngine.Random.insideUnitSphere * 10,
                velocity = Vector3.zero
            });
            if (folders.Contains(string.Join("", parentFolder)) != true)    //If the parent folder doesn't exist
            {
                nodes.Add(new Node()
                {
                    pathName = string.Join("", parentFolder),
                    position = UnityEngine.Random.insideUnitSphere * 10,
                    velocity = Vector3.zero
                });
            }
            /*
            GameObject lineObject = new GameObject(lineValues[1]);
            LineRenderer fileBranch = lineObject.AddComponent<LineRenderer>();
            fileBranch.startWidth = Mathf.Log10(sizes[i])/10;
            fileBranch.endWidth = Mathf.Log10(sizes[i])/10;
            fileBranch.SetVertexCount(fileString.Length);
            fileBranch.SetPosition(0, new Vector3(0, 0, 0));
            fileBranch.SetPosition(1, new Vector3(0, strValues.Length / 1000, 0));
            for (int j = 2; j < fileString.Length; j++)
            {
                //fileBranch.SetPosition(j, new Vector3(i / 100 - strValues.Length / 2 / 100, strValues.Length / 1000*j, 0));
                fileBranch.SetPosition(j, new Vector3(
                  100 * Mathf.Sin(i * 4 * Mathf.PI / strValues.Length),
                  strValues.Length / 1000 * j,
                  100 * Mathf.Cos(i * 4 * Mathf.PI / strValues.Length)
              ));               
            }
            */
        }

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
