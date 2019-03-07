using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class DirectoryScanner : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string[] repos = File.ReadAllLines("Repos/scos-repos.list");
        for (int i = 0; i < repos.Length; i++)
        {
            GameObject tree = Instantiate(GameObject.Find("List Tree"));
            tree.transform.position = new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f));
            string tempName = repos[i].TrimEnd(' ');
            tree.name = tempName;
            tree.GetComponent<ParseList>().m = tempName;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
