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
  public float LeapScale;
  public string m;
  public void Start()
  {
    if (m != null)
    {
      m = name;
      GameObject trunkCylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
      trunkCylinder.transform.position = transform.position;
      trunkCylinder.name = m + "|" + ".:";
      trunkCylinder.transform.localScale = new Vector3(10 * LeapScale, 10 * LeapScale, 10 * LeapScale);
      ParseTxtFile(m);
    }
    
  }
  void ParseTxtFile(string m)
  {
    string text = File.ReadAllText("Repos/"+m + ".txt");
    char[] separators = { ',', ';', '|', '\n' };
    strValues = text.Split(separators);
    GameObject seed = new GameObject();
    LineRenderer seedLine = seed.AddComponent<LineRenderer>();
    seedLine.SetVertexCount(1);
    seedLine.SetPosition(0, transform.position + new Vector3(0, 0, 0));
    seed.name = m + "|" + "./";
    Debug.Log("Did we get to here?");

    GameObject repo = Instantiate(GameObject.Find("TextObject"));
    repo.GetComponent<TextMesh>().text = m;
    repo.GetComponent<TextMesh>().transform.localScale = new Vector3(-LeapScale / 2, LeapScale / 2, LeapScale / 2);
    repo.transform.position = transform.position;

    GrowLocalTree(MatchFolder(".:"));
    int k = 0;
    for (int k2 = k; k2 < folders.Count; k2++)
    {
      GrowLocalTree(MatchFolder(folders[k]));
      k = k2;
      Debug.Log("Did we get to here?");

    }
  }
  int MatchFolder(string root)
  {
    bool match = false;
    int lineNumber = 0;
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
      fileColor = new Color(134f / 255f, 90f / 255f, 4f / 255f, 0.5f);
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
      else if (type == "sh" || type == "sh*" || type == "sample")
        fileColor = Color.red;
      else if (type == "exs")
        fileColor = Color.blue;
      else if (type == "conf")
        fileColor = Color.blue;
      else
        fileColor = Color.gray;
      /*
      //Unity Colors
      if (type == "info" || type == "resource")
          fileColor = Color.gray;
      else if (type == "mat")
          fileColor = Color.magenta;
      else if (type == "meta")
          fileColor = Color.gray;
      else if (type == "png")
          fileColor = Color.yellow;
      else if (type == "prefab")
          fileColor = Color.green;
      else if (type == "shader")
          fileColor = Color.red;
      else if (type == "wav")
          fileColor = Color.blue;
      else
          fileColor = Color.gray;
          */
    }
    fileColor.a = 1;
    return fileColor;
  }
  void GrowLocalTree(int lineNumber)
  {
    Debug.Log("Did we get to here?");
    int scale = SetScale(lineNumber);
    int size = SetFileSize(lineNumber);
    string parent = m + "|" + strValues[lineNumber].TrimEnd(':');
    GameObject trunkCylinder = GameObject.Find(m + "|" + strValues[lineNumber]);
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
        GameObject lineObject = Instantiate(GameObject.Find(parent + "/"));
        lineObject.name = parent + "/" + fileName;
        LineRenderer fileBranch = lineObject.GetComponent<LineRenderer>();
        fileBranch.material = (Material)Resources.Load("2nd Layer", typeof(Material));
        float width;
        if (fileSize == 0)
          width = LeapScale * 0.25f;
        else
          width = LeapScale * Mathf.Log10(fileSize) / 1;
        fileBranch.startWidth = width;
        fileBranch.endWidth = width;
        GameObject.Find(parent + "/").GetComponent<LineRenderer>().startWidth += width;
        fileBranch.positionCount = (fileBranch.positionCount + 2);
        fileBranch.SetPosition(fileBranch.positionCount - 2, fileBranch.GetPosition(fileBranch.positionCount - 3) + new Vector3(0, LeapScale * scale * 30, 0));
        fileBranch.SetPosition(fileBranch.positionCount - 1, fileBranch.GetPosition(fileBranch.positionCount - 3) + new Vector3(
            LeapScale * scale * 50 * Mathf.Cos(angle * 360 / size * Mathf.PI / 180),
            LeapScale * scale * 50,
            LeapScale * scale * 50 * Mathf.Sin(angle * 360 / size * Mathf.PI / 180)
        ));

        Renderer rend = fileBranch.GetComponent<Renderer>();
        Color lineColor = DetermineColor(fileName);
        rend.material.color = lineColor;
        rend.material.SetColor("_EmissionColor", lineColor);

        GameObject fileInfo = Instantiate(GameObject.Find("TextObject"));
        fileInfo.GetComponent<TextMesh>().text = fileName + "\n" + fileSize;
        fileInfo.GetComponent<TextMesh>().transform.localScale = new Vector3(-LeapScale / 10, LeapScale / 10, LeapScale / 10);
        fileInfo.transform.position = fileBranch.GetPosition(fileBranch.positionCount - 3) + new Vector3(
            LeapScale * scale * 50 / 2 * Mathf.Cos(angle * 360 / size * Mathf.PI / 180),
            LeapScale * scale * 40,
            LeapScale * scale * 50 / 2 * Mathf.Sin(angle * 360 / size * Mathf.PI / 180)
        );

        if (fileName[fileName.Length - 1] == '/')
        {
          GameObject newRoot = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
          newRoot.transform.localScale = new Vector3(LeapScale, LeapScale, LeapScale);
          newRoot.name = m + "|" + strValues[lineNumber].TrimEnd(':') + "/" + fileName.TrimEnd('/') + ":";
          newRoot.transform.position = fileBranch.GetPosition(fileBranch.positionCount - 3) + new Vector3(
              LeapScale * scale * 50 * Mathf.Cos(angle * 360 / size * Mathf.PI / 180),
              LeapScale * scale * 50,
              LeapScale * scale * 50 * Mathf.Sin(angle * 360 / size * Mathf.PI / 180)
          );
          folders.Add(strValues[lineNumber].TrimEnd(':') + "/" + fileName.TrimEnd('/') + ":");

          GameObject folderInfo = Instantiate(GameObject.Find("TextObject"));
          folderInfo.GetComponent<TextMesh>().text = fileName + "\n" + fileSize;
          folderInfo.GetComponent<TextMesh>().transform.localScale = new Vector3(-LeapScale / 10, LeapScale / 10, LeapScale / 10);
          folderInfo.transform.position = fileBranch.GetPosition(fileBranch.positionCount - 3) + new Vector3(
                        LeapScale * scale * 50 * Mathf.Cos(angle * 360 / size * Mathf.PI / 180),
                        LeapScale * scale * 50,
                        LeapScale * scale * 50 * Mathf.Sin(angle * 360 / size * Mathf.PI / 180)
                    );
          folderInfo.GetComponent<TextMesh>().color = Color.yellow;
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
