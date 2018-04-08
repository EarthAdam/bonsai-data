using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ParseFile : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ParseTxtFile();
	}
    public void ParseTxtFile()
    {
        string text = File.ReadAllText("Assets/Bonsai Code/combined.txt");

        char[] separators = { ',', ';', '|' };
        string[] strValues = text.Split(separators);

        List<string> stringValues = new List<string>();
        foreach (string str in strValues)
        {
                stringValues.Add(str);
                Debug.Log(str);
          

        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
