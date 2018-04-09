using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ParseFile : MonoBehaviour {
    //public int[] sizes;
	// Use this for initialization
	void Start () {
        ParseTxtFile();
	}
    public void ParseTxtFile()
    {
        string text = File.ReadAllText("../output2.txt");
        char[] separators = { ',', ';', '|','\n' };
        string[] strValues = text.Split(separators);
        int[] sizes = new int[strValues.Length];
        List<string> stringValues = new List<string>();
        for (int i = 0; i < strValues.Length; i++)
        {
            string[] lineValues = strValues[i].Split('\t');         //Break up string by tab separators
            sizes[i] = Convert.ToInt32(lineValues[0]);              //Save file size
            string[] fileString = lineValues[1].Split('/');         //break apart file path by '/' marks
            int level = fileString.Length;                          //count the number of folders and assign each file a level
            Debug.Log(fileString[0]+','+fileString[1] + ',' + fileString[2]);
            Debug.Log(sizes[i]);
            Debug.Log(level);
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
