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
        PopulateNodes();
        FindParents();
    }
    // Update is called once per frame
    void Update()
    {
    }
    void ParseTxtFile()
    {
        string text = File.ReadAllText("../docs/output.txt");
        char[] separators = { ',', ';', '|', '\n' };
        string[] strValues = text.Split(separators);
        int[] sizes = new int[strValues.Length];
        List<string> folders = new List<string>();
        for (int i = 0; i < strValues.Length-1; i++)
        {
            //Debug.Log("Entering loop #: "+i+1);
            string[] lineValues = strValues[i].Split('\t');                 //Break up string by tab separators
            sizes[i] = Convert.ToInt32(lineValues[0]);                      //Save file size
            string[] fileString = lineValues[1].Split('/');                 //break apart file path by '/' marks
            if (fileString.Length == 1)
            {
                nodes.Add(new Node()
                {
                    pathName = string.Join("", fileString),
                    parentName = "base",
                    position = new Vector3(0, 0, 0)
                });
                //Debug.Log(nodes[i].pathName + ',' + nodes[i].parentName);
                //Debug.Log(fileString.Length);
            }
            else
            {
                string[] parentFolder = new string[fileString.Length];          //Creates an array for the parent folder
                Array.Copy(fileString, parentFolder, fileString.Length - 1);    //Copies everything before the last '/' in the file path
                nodes.Add(new Node()
                {
                    pathName = string.Join("", fileString),
                    parentName = string.Join("", parentFolder),
                    position = UnityEngine.Random.insideUnitSphere * 10 + new Vector3(0, 5, 0)
                    //children = string.Join("", parentFolder).ToList()
                });
                //Debug.Log(nodes[i].pathName+','+nodes[i].parentName);
            }
        }
    }
    void PopulateNodes()
    {
        Debug.Log("The PopulateNodes() script started");
        foreach (var node in nodes)
        {
            node.sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            node.sphere.name = node.pathName;
            node.sphere.AddComponent<Rigidbody>();
            node.sphere.AddComponent<SpringJoint>();
            node.sphere.transform.position = node.position;
            Debug.Log(node.pathName + " sphere created.");
        }
    }
    void FindParents()
    {
        Debug.Log("The FindParents() script started");
        foreach (var node in nodes)
        {
            if (node.parentName != "base")
            {
                Debug.Log("This node is not the base");
                //SpringJoint localSpring = node.sphere.GetComponent<SpringJoint>();
                node.sphere.GetComponent<SpringJoint>().connectedBody = nodes.Find(i => i.pathName == node.parentName).sphere.GetComponent<Rigidbody>();
                Debug.Log("I think we've successfully found the node's SpringJoint");
                //localSpring.connectedBody = nodes.Find(i => i.pathName == node.parentName).sphere.GetComponent<Rigidbody>();
                //node.parent = nodes.Find(i => i.pathName == node.parentName).sphere;
                Debug.Log("Parent node was found");
                LineRenderer branch = node.sphere.AddComponent<LineRenderer>();
                branch.SetPosition(0, node.sphere.transform.position);
                branch.SetPosition(1, node.parent.transform.position);
                Debug.Log(node.pathName + " parent found");
            }
            else
            {
                Debug.Log("This node is the base");
            }
        }
    }
}
public class Node
{
    public GameObject sphere;
    public GameObject parent;
    public string pathName;
    public string parentName;
    public Vector3 position;
    public List<Node> children;

}
