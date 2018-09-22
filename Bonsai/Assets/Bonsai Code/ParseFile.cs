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
        //ApplyGraphForce();
        //foreach (var node in nodes)
        //{
        //    node.position += node.velocity * Time.deltaTime;
        //}
    }
    void ParseTxtFile()
    {
        string text = File.ReadAllText("../output.txt");
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
            /*//break apart file path by '/' marks
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
                string[] parentFolder = new string[fileString.Length];          //Creates an array for the parent folder
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
            }*/
            GameObject lineObject = new GameObject(lineValues[1]);
            LineRenderer fileBranch = lineObject.AddComponent<LineRenderer>();
            fileBranch.startWidth = Mathf.Log10(sizes[i]) / 10;
            fileBranch.endWidth = Mathf.Log10(sizes[i]) / 10;
            fileBranch.SetVertexCount(fileString.Length);
            fileBranch.SetPosition(0, new Vector3(0, 0, 0));
            fileBranch.SetPosition(1, new Vector3(0, strValues.Length / 1000, 0));
            Renderer rend = fileBranch.GetComponent<Renderer>();
            if (lineValues[1].Substring(lineValues[1].Length - 3) == "png" || lineValues[1].Substring(lineValues[1].Length - 3) == "jpg")
            {
                rend.material.color = new Color(1f, 1f, 0f);
                rend.material.SetColor("_EmissionColor", new Color(1f, 1f, 0f));
            }
            else if (lineValues[1].Substring(lineValues[1].Length - 3) == "txt" || lineValues[1].Substring(lineValues[1].Length - 3) == ".md")
            {
                rend.material.color = new Color(0.1f, 0.1f, 1f);
                rend.material.SetColor("_EmissionColor", new Color(0.1f, 0.1f, 1f));
            }
            else if (lineValues[1].Substring(lineValues[1].Length - 3) == ".cs")
            {
                rend.material.color = new Color(0.3f, 1f, 1f);
                rend.material.SetColor("_EmissionColor", new Color(0.1f, 1f, 1f));
            }
            else if (lineValues[1].Substring(lineValues[1].Length - 4) == "meta" )
            {
                rend.material.color = new Color(1f, 1f, 1f);
                rend.material.SetColor("_EmissionColor", new Color(1f, 1f, 1f));
                fileBranch.startWidth = Mathf.Log10(sizes[i]) / 10;
                fileBranch.endWidth = Mathf.Log10(sizes[i]) / 10;
            }
            else if (lineValues[1].Substring(lineValues[1].Length - 3) == "mat")
            {
                rend.material.color = new Color(1f, 0f, 0f);
                rend.material.SetColor("_EmissionColor", new Color(1f, 0f, 0f));
            }
            else if (lineValues[1].Substring(lineValues[1].Length - 3) == "fab")
            {
                rend.material.color = new Color(1f, 1f, 0f);
                rend.material.SetColor("_EmissionColor", new Color(1f, 1f, 0f));
            }
            else if (lineValues[1].Substring(lineValues[1].Length - 3) == "fbx")
            {
                rend.material.color = new Color(0,0,3);
                rend.material.SetColor("_EmissionColor", new Color(0,0,3));
            }
            else if (lineValues[1].Substring(lineValues[1].Length - 3) == "exe")
            {
                rend.material.color = new Color(0f, 1f, 1f);
                rend.material.SetColor("_EmissionColor", new Color(0f, 1f, 1f));
            }
            else if (lineValues[1].Substring(lineValues[1].Length - 3) == "set")
            {
                rend.material.color = new Color(1f, 1f, 1f);
                rend.material.SetColor("_EmissionColor", new Color(1f, 1f, 1f));
            }
            else if (lineValues[1].Substring(lineValues[1].Length - 3) == "wav")
            {
                rend.material.color = new Color(1f, 0f, 1f);
                rend.material.SetColor("_EmissionColor", new Color(1f, 0f, 1f));
            }
            else if (lineValues[1].Substring(lineValues[1].Length - 4) == "info")
            {
                rend.material.color = new Color(0.7f, 1f, 1f);
                rend.material.SetColor("_EmissionColor", new Color(0.7f, 1f, 1f));
            }
            else if (lineValues[1].Substring(lineValues[1].Length - 5) == "unity")
            {
                rend.material.color = new Color(0.7f, 1f, 1f);
                rend.material.SetColor("_EmissionColor", new Color(0.7f, 1f, 1f));
            }
            else if (lineValues[1].Substring(lineValues[1].Length - 5) == "cache")
            {
                rend.material.color = new Color(0.7f, 0.7f, 1f);
                rend.material.SetColor("_EmissionColor", new Color(0.7f, 0.7f, 1f));
            }
            else
            {
                rend.material.color = new Color(0.2f, 0.2f, 0.2f);
                rend.material.SetColor("_EmissionColor", new Color(0.2f, 0.2f, 0.2f));
            }
            //rend.material.SetColor("_EmissionColor", color1);
            for (int j = 2; j < fileString.Length; j++)
            {
                /*if(lineValues[1].Substring(lineValues[1].Length - 4) == "meta")
                {
                    fileBranch.SetPosition(j, new Vector3(i / 100 - strValues.Length / 2 / 100, strValues.Length / 1000 * j, 20));
                }
                else 
                {
                    fileBranch.SetPosition(j, new Vector3(i / 100 - strValues.Length / 2 / 100, strValues.Length / 1000 * j, 0));
                }*/

                fileBranch.SetPosition(j, new Vector3(
                   100 * Mathf.Sin(i * 4 * Mathf.PI / strValues.Length),
                   strValues.Length / 1000 * j,
                   100 * Mathf.Cos(i * 4 * Mathf.PI / strValues.Length)
                ));
            }
        }
        /*
        foreach(var node in nodes)
        {
            node.children = nodes.Where(node.pathName==node.parentName)
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
    
}
public class Node
{
    public string pathName;
    public string parentName;
    public Vector3 position;
    public Vector3 velocity;
    public List<Node> children;

}
