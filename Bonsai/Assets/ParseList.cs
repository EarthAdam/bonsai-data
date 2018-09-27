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
    Color DetermineColor(string fileString)
    {
        Color fileColor;
        if (fileString[fileString.Length - 1] == '/')
            fileColor = new Color(134f / 255f, 90f / 255f, 4f/255f, 0.5f);
        else
        {
            string[] fileArray = fileString.Split('.');
            string type = fileArray[fileArray.Length - 1];

            //SCOS Colors
            if (type == "tf" || type == "tfvars" || type == "tfstate")
                fileColor = Color.green;
            else if (type == "md")
                fileColor = Color.magenta;
            else if (type == "yaml")
                fileColor = Color.yellow;
            else if (type == "js")
                fileColor = Color.magenta;
            else if (type == "sh" || type == "sample")
                fileColor = Color.red;
            else if (type == "exs")
                fileColor = Color.blue;
            else if (type == "conf")
                fileColor = Color.blue;
            else
                fileColor = Color.gray;
            
            ////Unity Colors
            //if (type == "info" || type == "resource")
            //    fileColor = Color.gray;
            //else if (type == "mat")
            //    fileColor = Color.magenta;
            //else if (type == "meta")
            //    fileColor = Color.gray;
            //else if (type == "png")
            //    fileColor = Color.yellow;
            //else if (type == "prefab")
            //    fileColor = Color.green;
            //else if (type == "shader")
            //    fileColor = Color.red;
            //else if (type == "wav")
            //    fileColor = Color.blue;
            //else
            //    fileColor = Color.gray;
        }
        fileColor.a = 1;
        return fileColor;
    }
    void GrowLocalTree(int lineNumber)
    {
        int scale = SetScale(lineNumber);
        int size = SetFileSize(lineNumber);
        string parent = strValues[lineNumber].TrimEnd(':');
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
                taken += fileSize;
                int range = taken - (taken - fileSize);
                float angle = taken - (float)range / 2;
                GameObject lineObject = Instantiate(GameObject.Find(parent+"/"));
                lineObject.name = parent + "/" + fileName;
                LineRenderer fileBranch = lineObject.GetComponent<LineRenderer>();
                fileBranch.material = (Material)Resources.Load("2nd Layer", typeof(Material));
                float width;
                if (fileSize == 0)
                    width = 0.25f;
                else
                    width = Mathf.Log10(fileSize) / 1;
                fileBranch.startWidth = width;
                fileBranch.endWidth = width;
                GameObject.Find(parent + "/").GetComponent<LineRenderer>().startWidth += width;
                fileBranch.positionCount = (fileBranch.positionCount+2);
                fileBranch.SetPosition(fileBranch.positionCount-2, fileBranch.GetPosition(fileBranch.positionCount-3) + new Vector3(0, scale * 30,0));
                fileBranch.SetPosition(fileBranch.positionCount-1, fileBranch.GetPosition(fileBranch.positionCount-3) + new Vector3(
                    scale * 50 * Mathf.Cos(angle * 360 / size * Mathf.PI / 180),
                    scale * 50,
                    scale * 50 * Mathf.Sin(angle * 360 / size * Mathf.PI / 180)
                ));

                Renderer rend = fileBranch.GetComponent<Renderer>();
                Color lineColor = DetermineColor(fileName);
                rend.material.color = lineColor;
                rend.material.SetColor("_EmissionColor", lineColor);


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
