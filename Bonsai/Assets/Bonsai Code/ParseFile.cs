using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ParseFile : MonoBehaviour {
    private int maxLevels = 0;
    // Use this for initialization
    public void Start () {
        ParseTxtFile();
	}
    public void ParseTxtFile()
    {
        string text = File.ReadAllText("../output2.txt");
        char[] separators = { ',', ';', '|', '\n' };
        string[] strValues = text.Split(separators);
        int[] sizes = new int[strValues.Length];
        List<string> stringValues = new List<string>();
        

        for (int i = 0; i < strValues.Length; i++)
        {
            string[] lineValues = strValues[i].Split('\t');           //Break up string by tab separators
            sizes[i] = Convert.ToInt32(lineValues[0]);                //Save file size
            string[] fileString = lineValues[1].Split('/');           //break apart file path by '/' marks
            if (fileString.Length > maxLevels)
                maxLevels = fileString.Length;
            //assign each file a level based on how many folders it's in
            //count the number of repeated folders in each level
            GameObject lineObject = new GameObject(lineValues[1]);
            LineRenderer fileBranch = lineObject.AddComponent<LineRenderer>();
            fileBranch.startWidth = Mathf.Log10(sizes[i])/10;
            fileBranch.endWidth = Mathf.Log10(sizes[i])/10;
            fileBranch.SetVertexCount(fileString.Length);
            fileBranch.SetPosition(0, new Vector3(0, 0, 0));
            fileBranch.SetPosition(1, new Vector3(0, strValues.Length / 100, 0));
            for (int j = 2; j < fileString.Length; j++)
            {
                fileBranch.SetPosition(j, new Vector3(i / 10 - strValues.Length / 2 / 10, strValues.Length / 100*j, 0));
            }

            //Debug.Log(fileString[0]+','+fileString[1] + ',' + fileString[2]);
            Debug.Log(sizes[i]);
            Debug.Log(fileString.Length);
            //Next: count the number of times a folder path is repeated for each level
        }
        foreach (string str in strValues)
        {
            
             
            
            //Create New Line
            //Assign file size to thickness of line

            stringValues.Add(str);
            Debug.Log(str);
          

        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
