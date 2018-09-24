using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class ParseList : MonoBehaviour
{
    private int maxLevels = 0;
    public float desiredConnectedNodeDistance = 1;
    public float connectedNodeForce = 1;
    public float disconnectedNodeForce = 1;
    public List<Node> nodes;
    public string[] strValues;
    public List<string> folders;
    // Use this for initialization
    public void Start()
    {
        nodes = new List<Node>();
        GameObject trunkCylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        trunkCylinder.name = ".:";
        ParseTxtFile();
    }
    void Update()
    {
    }
    void ParseTxtFile()
    {
        string text = File.ReadAllText("../out.txt");
        char[] separators = { ',', ';', '|', '\n' };
        strValues = text.Split(separators);
        //Debug.Log("Pre 'MatchFolder()' script");
        GrowLocalTree(MatchFolder(".:"));
        //Debug.Log("Folders: " + folders);
        for(int k = 0;k < folders.Count;k++)
        {
            GrowLocalTree(MatchFolder(folders[k]));
        }
        //Debug.Log("Post 'MatchFolder()' script");
    }
    int MatchFolder(string root)
    {
        //Debug.Log("Start 'MatchFolder()' script");
        bool match = false;
        int lineNumber=0;
        for (int i = 0; match == false; i++)
        {
            //Debug.Log("'MatchFolder()' match for loop");
            //Debug.Log("'MatchFolder()' pre match found condition. i = " + i + ", strValues[i] = " + strValues[i] + ", root = " + root);
            if (strValues[i] == root)
            {
                match = true;
                lineNumber = i;
                //Debug.Log("'MatchFolder()' match found condition. i = "+i+", strValues[i] = "+ strValues[i]+", root = "+root);
            }
        }
        //Debug.Log("Pre 'MatchFolder()' Return lineNumber");
        return lineNumber;
    }
    void GrowLocalTree(int lineNumber)
    {
        int scale = 1;
        if (lineNumber == 0)
            scale = 3;
        else
            scale = 1;
        int size = Int32.Parse(Regex.Match(strValues[lineNumber+1], @"\d+").Value);
        GameObject trunkCylinder = GameObject.Find(strValues[lineNumber]);
        //Debug.Log("Trunk name is: " + strValues[lineNumber]);
        Boolean blankLine = false;
        int lineCount = lineNumber + 2;
        int taken = 0;
        while (blankLine == false)
        {
            //Debug.Log("water:" + strValues[lineCount]);
            if (strValues[lineCount] == "")
            {
                blankLine = true;
            }
            else
            {
                string[] lineValues = strValues[lineCount].Split(' ');
                //Debug.Log(lineValues.Length);
                int fileSize = Int32.Parse(lineValues[lineValues.Length - 2]);
                string fileName = lineValues[lineValues.Length - 1];
                //Debug.Log("Size = " + fileSize + ", Name: " + fileName);
                taken += fileSize;
                int range = taken - (taken - fileSize);
                float angle = taken - (float)range / 2;
                //Debug.Log("Range = " + range + "Angle = " + angle);

                GameObject lineObject = new GameObject(fileName);
                LineRenderer fileBranch = lineObject.AddComponent<LineRenderer>();
                fileBranch.SetVertexCount(3);
                if (fileSize == 0)
                {
                    fileBranch.startWidth = 0.25f;
                    fileBranch.endWidth = 0.25f;
                }
                else
                {
                    fileBranch.startWidth = Mathf.Log10(fileSize) / 1;
                    fileBranch.endWidth = Mathf.Log10(fileSize) / 1;
                }
                fileBranch.SetPosition(0, trunkCylinder.transform.position);
                fileBranch.SetPosition(1, trunkCylinder.transform.position + new Vector3(0, scale * 30,0));
                fileBranch.SetPosition(2, trunkCylinder.transform.position + new Vector3(
                    scale * 50 * Mathf.Cos(angle * 360 / size * Mathf.PI / 180),
                    scale * 50,
                    scale * 50 * Mathf.Sin(angle * 360 / size * Mathf.PI / 180)
                ));
                Renderer rend = fileBranch.GetComponent<Renderer>();


                if (fileName[fileName.Length - 1] == '/')
                {
                    GameObject newRoot = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    newRoot.name = strValues[lineNumber].TrimEnd(':') + "/"+fileName.TrimEnd('/') + ":";
                    newRoot.transform.position = trunkCylinder.transform.position + new Vector3(
                        scale * 50 * Mathf.Cos(angle * 360 / size * Mathf.PI / 180),
                        scale * 50,
                        scale * 50 * Mathf.Sin(angle * 360 / size * Mathf.PI / 180)
                    );
                    folders.Add(strValues[lineNumber].TrimEnd(':') + "/" + fileName.TrimEnd('/') + ":");
                }
                else
                {
                    //Debug.Log(fileName + " is not a Folder.");
                }

            }
            lineCount++;
        }
    }
        
}
