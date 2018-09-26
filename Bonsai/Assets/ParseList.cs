using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class ParseList : MonoBehaviour
{
    public string[] strValues;
    public List<string> folders;
    public int lineCount;
    public void Start()
    {
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
        GameObject seed = new GameObject();
        LineRenderer seedLine = seed.AddComponent<LineRenderer>();
        seedLine.SetVertexCount(1);
        seedLine.SetPosition(0, new Vector3(0, 0, 0));
        seed.name = "./";
        GrowLocalTree(MatchFolder(".:"));
        int k = 0;
        for (int k2 = k; k2 < folders.Count; k2++)
        {
            GrowLocalTree(MatchFolder(folders[k]));
            k = k2;
        }
    }
    int MatchFolder(string root)
    {
        bool match = false;
        int lineNumber=0;
        for (int i = 0; match == false; i++)
        {
            if (strValues[i] == root)
            {
                match = true;
                lineNumber = i;
            }
        }
        return lineNumber;
    }
    int SetFileSize(int lineNumber)
    {
        int size;
        if (Int32.Parse(Regex.Match(strValues[lineNumber + 1], @"\d+").Value) != 0)
            size = Int32.Parse(Regex.Match(strValues[lineNumber + 1], @"\d+").Value);
        else
            size = 1;
        return size;
    }
    int SetScale(int lineNumber)
    {
        int scale = 1;
        if (lineNumber == 0)
            scale = 3;
        else
            scale = 1;
        return scale;
    }
    void GrowLocalTree(int lineNumber)
    {
        int scale = SetScale(lineNumber);
        int size = SetFileSize(lineNumber);
        string parent = strValues[lineNumber].TrimEnd(':');
        Debug.Log("parent: " + parent.TrimEnd(':'));
        GameObject trunkCylinder = GameObject.Find(strValues[lineNumber]);
        Boolean blankLine = false;
        lineCount = lineNumber + 2;
        int taken = 0;
        while (blankLine == false)
        {
            if (strValues[lineCount] == "")
            {
                blankLine = true;
            }
            else
            {
                string slightlyShorterLine = strValues[lineCount].TrimStart(' ');
                string[] lineValues = slightlyShorterLine.Split(' ');
                int fileSize = Int32.Parse(lineValues[0]);
                string fileName_ws = slightlyShorterLine.TrimStart(lineValues[0].ToCharArray());
                string fileName = fileName_ws.TrimStart(' ');
                Debug.Log("FileName: " + fileName);
                taken += fileSize;
                int range = taken - (taken - fileSize);
                float angle = taken - (float)range / 2;
                //
                // Basically grab the parent folder name, and use that to look up a line with that name
                // 
                // Then copy that line, and append more points to it
                // 
                // Then rename it to include the fileName
                //
                // Then maybe add this "size" to the parent "size"?
                //
                GameObject lineObject = Instantiate(GameObject.Find(parent+"/"));
                lineObject.name = parent + "/" + fileName;
                Debug.Log("Line Name = " + lineObject.name);
                LineRenderer fileBranch = lineObject.GetComponent<LineRenderer>();
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
                fileBranch.SetVertexCount(fileBranch.positionCount+2);
                fileBranch.SetPosition(fileBranch.positionCount-2, fileBranch.GetPosition(fileBranch.positionCount-3) + new Vector3(0, scale * 30,0));
                fileBranch.SetPosition(fileBranch.positionCount-1, fileBranch.GetPosition(fileBranch.positionCount-3) + new Vector3(
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
