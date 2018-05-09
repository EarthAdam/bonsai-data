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
    public GameObject nodePrefab;
    // Use this for initialization
    public void Start () {
        nodes = new List<Node>();
        ParseTxtFile();
        PopulateNodes();
        //FindParents();
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
                    pathName = lineValues[1],
                    parentName = "Base",
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
                    pathName = lineValues[1],
                    parentName = string.Join("/", parentFolder).TrimEnd('/'),
                    position = UnityEngine.Random.insideUnitSphere * 10 + new Vector3(0, 5, 0)
                    //children = string.Join("", parentFolder).ToList()
                });
                Debug.Log(nodes[i].pathName+','+nodes[i].children);
            }
        }
    }
    public void AddBranch(GameObject prefab, string branchName, string parentName, Transform branchTX)
    {
        Instantiate(prefab,branchTX);
        //prefab.GetComponent<SpringJoint>().connectedBody = GameObject.Find(parentName).GetComponent<Rigidbody>();
        //prefab.GetComponent<LineRenderer>().SetPosition(0,GameObject.Find(parentName).GetComponent<Rigidbody>().transform.position);
    }
    void PopulateNodes()
    {
        foreach (var node in nodes)
        {
            GameObject newBranch = Instantiate(nodePrefab);
            newBranch.name = node.pathName;
            newBranch.GetComponent<Forces>().parentName = node.parentName;
            newBranch.GetComponent<Forces>().parent = GameObject.Find(node.parentName);
            //newBranch.GetComponent<SpringJoint>().connectedBody = GameObject.Find(node.parentName).GetComponent<Rigidbody>();
            //AddBranch(nodePrefab,node.pathName,node.parentName,node.sphere.transform);
        }
    }/*
    void FindParents()
    {
        Debug.Log("The FindParents() script started");
        foreach (var node in nodes)
        {
            if (node.parentName != "base")
            {
                Debug.Log("This node is not the base");
                LineRenderer branch = node.sphere.AddComponent<LineRenderer>();
                branch.SetPosition(0, node.sphere.transform.position);
                branch.SetPosition(1, new Vector3(0,0,0));
                //node.parent = GameObject.Find(node.parentName).GetComponent<Rigidbody>();
                Debug.Log("Line Added");
                Debug.Log(node.pathName);
                Debug.Log(node.parentName);
                String tempName = node.parentName;
                //IEnumerable<Node> localParent = nodes.Where(node => node.pathName == tempName);
                Debug.Log("This is what I think should be the parent name: "+ nodes.Where(i => i.pathName == node.parentName));
                //Debug.Log("This is what I think should be the parent name: " + nodes.Find(j => j.pathName == node.parentName).parent);

                //SpringJointlocal node.sphere.GetComponent<SpringJoint>().connectedBody = nodes.Find(i => i.pathName == node.parentName).sphere.GetComponent<Rigidbody>();
                SpringJoint localSpring = node.sphere.AddComponent<SpringJoint>();
                Debug.Log("SpringJoint Added");

                //node.sphere.GetComponent<SpringJoint>().connectedBody = nodes.Find(i => i.pathName == node.parentName).sphere.GetComponent<Rigidbody>();
                Debug.Log("I think we've successfully found the node's SpringJoint");
                //localSpring.connectedBody.position = nodes.Where(i => i.pathName == node.parentName).transform.position;// GetComponent<Rigidbody>();
                
                //Debug.Log("Parent node was found");
                
                //Debug.Log(node.pathName + " parent found");
            }
            else
            {
                Debug.Log("This node is the base");
                Debug.Log(node.pathName);
                Debug.Log(node.parentName);
            }
        }
    }*/
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
